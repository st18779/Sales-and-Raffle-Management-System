import { Component, inject } from '@angular/core';
import { GiftService } from '../../services/gift.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { tap } from 'rxjs';
import { RouterLink } from '@angular/router';
import { User } from '../../models/User.model';
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
  showErrorOnDelete: boolean = false;
  showErrorOnRandom: boolean = false
  curUserId!: string

  ngOnInit() {

    const user = JSON.parse(localStorage.getItem('user') || '{}')
    this.curUserId = JSON.parse(localStorage.getItem('user') || '{}').id ||"";
    this.isAdmin = !!user.isAdmin;
  }

  giftsList$ = this.giftService.getGiftsList().pipe(
    tap(gifts => console.log(gifts)));

    add(giftId: number) {
      console.log("jjjjjjjjj");
      
    this.purchasesService.addGiftToBasket(this.curUserId, giftId).subscribe({
      next: (res) => {
        console.log('המתנה נוספה לסל בהצלחה:', res);
      },
      error: (err) => {
        console.error('שגיאה בהוספת המתנה לסל:', err);
        this.showErrorOnDelete = true;
      }
    });
  }

 remove(giftId: number) {
    this.purchasesService.removeGiftFromBasket(this.curUserId, giftId).subscribe()
  }

  deleteGift(giftId: number) {
    this.giftService.deleteGift(giftId).subscribe({
      next: (res) => {
        console.log('נמחק בהצלחה:', res);
      },
      error: (err) => {
        console.error('שגיאה במחיקה:', err);
        this.showErrorOnDelete = true;
      }
    });
  }

  random(giftId: number) {
    this.giftService.randomWinner(giftId).subscribe({
      next: (user: User) => {
        console.log(user);  
        //window.location.reload()
      },
      error: (err) => {
        console.error('שגיאה בהגרלה:', err);
        this.showErrorOnRandom = true;
      }
    });
  }

}




