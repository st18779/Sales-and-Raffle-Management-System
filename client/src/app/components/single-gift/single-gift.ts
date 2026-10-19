import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { GiftService } from '../../services/gift.service';
import { Gift } from '../../models/Gift.model';
import { CommonModule } from '@angular/common';
import { ImgFallback } from '../../core/img-fallback.directive';
import { PurchasesService } from '../../services/purchases.service';

@Component({
  selector: 'app-single-gift',
  imports: [RouterLink,CommonModule,ImgFallback],
  templateUrl: './single-gift.html',
  styleUrl: './single-gift.css',
})
export class SingleGift implements OnInit{
  giftId!: number;
  curGift!: Gift
  isAdmin!: boolean
  curUserId!: string
  errorMessage = '';

  constructor(private activatedRoute:ActivatedRoute){}
  giftService: GiftService = inject(GiftService)
  purchasesService: PurchasesService = inject(PurchasesService)

  ngOnInit(): void {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    this.curUserId = user.id || '';
    this.isAdmin = !!user.isAdmin;

    this.activatedRoute.params.subscribe(params => {
      this.giftId = params['id'];
      this.getGiftDetails(this.giftId)
    })
  }
  getGiftDetails(id:number) {
  this.giftService.getGiftById(id).subscribe({
    next: (data: Gift) => {
      this.curGift = data;
    }
  });
  }

  addToBasket() {
    this.errorMessage = '';
    this.purchasesService.addGiftToBasket(this.curUserId, this.curGift.id).subscribe({
      error: () => this.errorMessage = 'לא ניתן להוסיף לסל כרגע. נסה שנית.'
    });
  }

}
