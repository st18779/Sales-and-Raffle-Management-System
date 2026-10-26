import { Component, inject, OnInit } from '@angular/core';
import { PurchasesService } from '../../services/purchases.service';
import { Router } from "@angular/router";
import { CommonModule } from '@angular/common';
import { take } from 'rxjs';
import { BasketModel } from '../../models/BasketModel.model';


@Component({
  selector: 'app-checkOut',
  imports: [CommonModule],
  templateUrl: './checkOut.html',
  styleUrl: './checkOut.css',
})
export class checkOut{
  purchasesService: PurchasesService = inject(PurchasesService)

  curUserId!: string
  errorMessage = '';
  totalPrice$ = this.purchasesService.totalPrice$;

  constructor(private router: Router) { }

  ngOnInit() {
    this.curUserId = JSON.parse(localStorage.getItem('user')!).id;

  }

  checkOut() {
    this.errorMessage = '';
    // שומרים את הסל והסכום לפני שהסל מתרוקן, כדי להציג סיכום בדף הסיום
    let items: BasketModel[] = [], total = 0;
    this.purchasesService.basket$.pipe(take(1)).subscribe(b => items = b);
    this.purchasesService.totalPrice$.pipe(take(1)).subscribe(t => total = t);

    this.purchasesService.checkOut(this.curUserId).subscribe({
      next: () => this.router.navigate(['/gifts/endCheckOut'], { state: { items, total } }),
      error: (err) => this.errorMessage = err.status === 400
        ? 'הסל ריק - אין פריטים לתשלום.'
        : 'התשלום נכשל. הסל שלך נשמר - נסה שוב.'
    });
  }
}
