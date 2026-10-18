import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
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

  // מודיע לאפליקציה כשהסל נפתח/נסגר כדי להזיז את תוכן העמוד הצידה
  @Output()
  openChange = new EventEmitter<boolean>();

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

  toggle() { this.setOpen(!this.isOpen); }
  close() { this.setOpen(false); }

  private setOpen(open: boolean) {
    this.isOpen = open;
    this.openChange.emit(open);
  }

  add(giftId: number) {
    this.purchasesService.addGiftToBasket(this.curUserId, giftId).subscribe();
  }

  remove(giftId: number) {
    this.purchasesService.removeGiftFromBasket(this.curUserId, giftId).subscribe();
  }
}