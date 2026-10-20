import { Component, ElementRef, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PurchasesService } from '../../services/purchases.service';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ImgFallback } from '../../core/img-fallback.directive';

@Component({
  selector: 'app-basket',
  imports: [RouterLink, CommonModule, FormsModule, ImgFallback],
  templateUrl: './basket.html',
  styleUrl: './basket.css',
  host: {
    '(document:click)': 'onDocumentClick($event)',
    '(document:keydown.escape)': 'close()',
  },
})
export class Basket implements OnInit, OnChanges {
  purchasesService: PurchasesService = inject(PurchasesService);
  basket$ = this.purchasesService.basket$;
  totalPrice$ = this.purchasesService.totalPrice$;

  itemCount$ = this.purchasesService.itemCount$;

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

  private host: HTMLElement = inject(ElementRef).nativeElement;

  // הסל נסגר כשעוברים דף
  private closeOnNavigation = inject(Router).events
    .pipe(filter(e => e instanceof NavigationEnd), takeUntilDestroyed())
    .subscribe(() => this.close());

  toggle() { this.setOpen(!this.isOpen); }
  close() { if (this.isOpen) this.setOpen(false); }

  // לחיצה מחוץ לסל סוגרת אותו - חוץ מכפתור "הסל שלי" בתפריט (פותח/סוגר בעצמו)
  // וכפתורי "הוספה לסל" (מסומנים ב-keeps-basket-open), כדי לראות את הסל מתעדכן
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (this.isOpen && !this.host.contains(target) && !target.closest('.basket-toggle, .keeps-basket-open')) {
      this.close();
    }
  }

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