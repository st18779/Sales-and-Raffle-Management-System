import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PurchasesService } from '../../services/purchases.service';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { map } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-basket',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './basket.html',
  styleUrl: './basket.css',
})
export class Basket implements OnInit, OnChanges {
  purchasesService: PurchasesService = inject(PurchasesService);
  basket$ = this.purchasesService.basket$;
  totalPrice$ = this.purchasesService.totalPrice$;

  // סך כל הכרטיסים בסל - להצגה בתג על הכפתור
  itemCount$ = this.basket$.pipe(
    map(items => (items ?? []).reduce((sum, i) => sum + i.totalTickets, 0))
  );

  isOpen = false;

  @Input()
  curUserId!: string;

  ngOnInit() {
    if (this.curUserId) {
      this.purchasesService.loadBasket(this.curUserId);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['curUserId'] && this.curUserId) {
      this.purchasesService.loadBasket(this.curUserId);
    }
  }

  toggle() { this.isOpen = !this.isOpen; }
  close() { this.isOpen = false; }

  add(giftId: number) {
    this.purchasesService.addGiftToBasket(this.curUserId, giftId).subscribe();
  }

  remove(giftId: number) {
    this.purchasesService.removeGiftFromBasket(this.curUserId, giftId).subscribe();
  }
}