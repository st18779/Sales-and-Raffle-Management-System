import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { GiftService } from '../../services/gift.service';
import { Gift } from '../../models/Gift.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-single-gift',
  imports: [RouterLink,CommonModule],
  templateUrl: './single-gift.html',
  styleUrl: './single-gift.css',
})
export class SingleGift implements OnInit{
  giftId!: number;
  curGift!: Gift

  constructor(private activatedRoute:ActivatedRoute){}
  giftService: GiftService = inject(GiftService)

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.giftId = params['id'];
      this.getGiftDetails(this.giftId)
    })
  }
  getGiftDetails(id:number) {
  this.giftService.getGiftById(id).subscribe({
    next: (data: Gift) => {
      this.curGift = data;
      console.log('הנתונים התקבלו בהצלחה:', data);
    },
    error: (err) => {
      console.error("שגיאה בטעינת הפרטים: ", err);
    }
  });
  }

   deleteGift(giftId: number) {
    this.giftService.deleteGift(giftId).subscribe({
      next: (res) => {
        console.log('נמחק בהצלחה:', res);
      },
      error: (err) => {
        console.error('שגיאה במחיקה:', err);
      }
    });
  }

}
