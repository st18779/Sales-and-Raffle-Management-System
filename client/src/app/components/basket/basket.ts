import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PurchasesService } from '../../services/purchases.service';
import { User } from '../../models/User.model';
import { ActivatedRoute, RouterLink } from "@angular/router";
import { CommonModule } from '@angular/common';
import { BasketModel } from '../../models/BasketModel.model'
import { map, Observable } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-basket',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './basket.html',
  styleUrl: './basket.css',
})
export class Basket implements OnInit, OnChanges {
  purchasesService: PurchasesService = inject(PurchasesService)
  private activatedRoute = inject(ActivatedRoute);
  basket$ = this.purchasesService.basket$;
  totalPrice$ = this.purchasesService.totalPrice$;

  @Input()
  curUserId!: string


  ngOnInit() {
    console.log(this.basket$);

    if (this.curUserId) {
      this.purchasesService.loadBasket(this.curUserId);
    }
    
  }


  ngOnChanges(changes: SimpleChanges) {
    if (changes['curUserId'] && this.curUserId) {
      this.purchasesService.loadBasket(this.curUserId);
    }
  }


  add(giftId: number) {
    this.purchasesService.addGiftToBasket(this.curUserId, giftId).subscribe();
  }

  remove(giftId: number) {
    this.purchasesService.removeGiftFromBasket(this.curUserId, giftId).subscribe()
  }


}
