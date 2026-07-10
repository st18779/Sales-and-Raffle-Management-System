import { Component, inject, OnInit } from '@angular/core';
import { PurchasesService } from '../../services/purchases.service';
import { Router } from "@angular/router";
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-checkOut',
  imports: [CommonModule],
  templateUrl: './checkOut.html',
  styleUrl: './checkOut.css',
})
export class checkOut{
  purchasesService: PurchasesService = inject(PurchasesService)

  curUserId!: string
  showCheckOutError: boolean = false
  totalPrice$ = this.purchasesService.totalPrice$;

  constructor(private router: Router) { }

  ngOnInit() {
    this.curUserId = JSON.parse(localStorage.getItem('user')!).id;
    
  }

  checkOut() {
    this.purchasesService.checkOut(this.curUserId).subscribe({
      next: (res) => {
        console.log('שולם בהצלחה:', res);
        this.router.navigate(['gifts/endCheckOut']);
      },
      error: (err) => {
        console.error('שגיאה בתשלום:', err);
        this.showCheckOutError = true;
      }
    });
  }
}