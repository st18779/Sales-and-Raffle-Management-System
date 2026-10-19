import { Component, inject } from '@angular/core';
import { GiftService } from '../../services/gift.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, finalize, switchMap } from 'rxjs';
import { RouterLink } from '@angular/router';
import { PurchasesService } from '../../services/purchases.service';
import { ImgFallback } from '../../core/img-fallback.directive';


@Component({
  selector: 'app-gifts-list',
  imports: [CommonModule, FormsModule, RouterLink, ImgFallback],
  templateUrl: './gifts-list.html',
  styleUrl: './gifts-list.css',
})
export class GiftsList {

  giftService: GiftService = inject(GiftService)
  purchasesService: PurchasesService = inject(PurchasesService)
  isAdmin!: boolean
  curUserId!: string

  // שגיאה לכל מתנה בנפרד (במקום משתנה אחד לכל הדף)
  giftErrors: Record<number, string> = {};

  // טיימרים שמסתירים הודעת שגיאה אחרי 10 שניות
  private errorTimers = new Map<number, ReturnType<typeof setTimeout>>();

  ngOnInit() {

    const user = JSON.parse(localStorage.getItem('user') || '{}')
    this.curUserId = user.id || "";
    this.isAdmin = !!user.isAdmin;
  }

  // הרשימה נטענת מחדש אחרי מחיקה/הגרלה
  private refresh$ = new BehaviorSubject<void>(undefined);
  giftsList$ = this.refresh$.pipe(switchMap(() => this.giftService.getGiftsList()));

  add(giftId: number) {
    this.clearError(giftId);
    this.purchasesService.addGiftToBasket(this.curUserId, giftId).subscribe({
      error: () => this.giftErrors[giftId] = 'לא ניתן להוסיף לסל כרגע. נסה שנית.'
    });
  }

 remove(giftId: number) {
    this.purchasesService.removeGiftFromBasket(this.curUserId, giftId).subscribe()
  }

  // מתנות שבקשת המחיקה שלהן עדיין בדרך - מונע לחיצה כפולה
  deleting = new Set<number>();

  deleteGift(giftId: number) {
    if (this.deleting.has(giftId)) return;
    this.deleting.add(giftId);
    this.clearError(giftId);
    this.giftService.deleteGift(giftId).pipe(finalize(() => this.deleting.delete(giftId))).subscribe({
      next: () => this.refresh$.next(),
      error: (err) => {
        // כבר נמחקה (למשל מחלון אחר) - פשוט מרעננים והכרטיס נעלם
        if (err.status === 404) { this.refresh$.next(); return; }
        if (err.status === 409) {
          this.showTimedError(giftId, 'לא ניתן למחוק: כבר נקנו כרטיסים למתנה הזו.');
        } else {
          this.giftErrors[giftId] = 'שגיאה במחיקה. נסה שנית.';
        }
      }
    });
  }

  random(giftId: number) {
    this.clearError(giftId);
    this.giftService.randomWinner(giftId).subscribe({
      // השרת מחזיר תשובה ריקה כשאין רכישות ששולמו
      next: (winner) => winner
        ? this.refresh$.next()
        : this.giftErrors[giftId] = 'אין עדיין רכישות ששולמו למתנה הזו - אי אפשר להגריל.',
      error: () => this.giftErrors[giftId] = 'שגיאה בהגרלה. נסה שנית.'
    });
  }

  // מציג הודעה ל-10 שניות. הצגה חוזרת מבטלת את הטיימר הקודם ומתחילה את הספירה מחדש
  private showTimedError(giftId: number, message: string) {
    this.clearError(giftId);
    this.giftErrors[giftId] = message;
    this.errorTimers.set(giftId, setTimeout(() => this.clearError(giftId), 10_000));
  }

  private clearError(giftId: number) {
    clearTimeout(this.errorTimers.get(giftId));
    this.errorTimers.delete(giftId);
    delete this.giftErrors[giftId];
  }

  // לא משאירים טיימרים פעילים אחרי שהרכיב נסגר
  ngOnDestroy() {
    this.errorTimers.forEach(t => clearTimeout(t));
  }

}
