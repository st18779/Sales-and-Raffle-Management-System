import { Component, inject } from '@angular/core';
import { DonorService } from '../../services/donor.service';
import { BehaviorSubject, finalize, switchMap } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-donors',
  imports: [CommonModule,FormsModule,RouterLink],
  templateUrl: './donors-list.html',
  styleUrl: './donors-list.css',
})
export class DonorsList {
  donorService: DonorService = inject(DonorService)

  // שגיאה לכל תורם בנפרד
  donorErrors: Record<string, string> = {};
  // טיימרים שמסתירים הודעת שגיאה אחרי 10 שניות
  private errorTimers = new Map<string, ReturnType<typeof setTimeout>>();
  // תורמים שבקשת המחיקה שלהם עדיין בדרך - מונע לחיצה כפולה
  deleting = new Set<string>();

  // הרשימה נטענת מחדש אחרי מחיקה (במקום רענון של כל הדף)
  private refresh$ = new BehaviorSubject<void>(undefined);
  donorsList$ = this.refresh$.pipe(switchMap(() => this.donorService.getDonorsList()));

  deleteDonor(donorId: string) {
    if (this.deleting.has(donorId)) return;
    this.deleting.add(donorId);
    this.clearError(donorId);
    this.donorService.deleteDonor(donorId).pipe(finalize(() => this.deleting.delete(donorId))).subscribe({
      next: () => this.refresh$.next(),
      error: (err) => {
        // כבר נמחק - פשוט מרעננים והשורה נעלמת
        if (err.status === 404) { this.refresh$.next(); return; }
        if (err.status === 409) {
          this.showTimedError(donorId, 'לא ניתן למחוק: לתורם יש מתנות בקטלוג. יש למחוק אותן או להעביר לתורם אחר קודם.');
        } else {
          this.donorErrors[donorId] = 'שגיאה במחיקה. נסה שנית.';
        }
      }
    });
  }

  // מציג הודעה ל-10 שניות. הצגה חוזרת מבטלת את הטיימר הקודם ומתחילה את הספירה מחדש
  private showTimedError(donorId: string, message: string) {
    this.clearError(donorId);
    this.donorErrors[donorId] = message;
    this.errorTimers.set(donorId, setTimeout(() => this.clearError(donorId), 10_000));
  }

  private clearError(donorId: string) {
    clearTimeout(this.errorTimers.get(donorId));
    this.errorTimers.delete(donorId);
    delete this.donorErrors[donorId];
  }

  // לא משאירים טיימרים פעילים אחרי שהרכיב נסגר
  ngOnDestroy() {
    this.errorTimers.forEach(t => clearTimeout(t));
  }

}
