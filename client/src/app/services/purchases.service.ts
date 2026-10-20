import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { BasketModel } from '../models/BasketModel.model';




@Injectable({
  providedIn: 'root',
})
export class PurchasesService {
  BASE_URL = 'http://localhost:8088/api/Purchases';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  private basketSubject = new BehaviorSubject<BasketModel[]>([]);
  basket$ = this.basketSubject.asObservable();

  totalPrice$ = this.basket$.pipe(
    map(items => items.reduce((sum, item) => sum + (item.price * item.totalTickets), 0))
  );

  // סך כל הכרטיסים בסל - לתג על כפתור הסל ובתפריט
  itemCount$ = this.basket$.pipe(
    map(items => (items ?? []).reduce((sum, i) => sum + i.totalTickets, 0))
  );

loadBasket(userId: string) {
  this.http
    .get<BasketModel[]>(`${this.BASE_URL}/GetBasket/${userId}`)
    .subscribe(b => this.basketSubject.next(b));
}

  addGiftToBasket(userId: string, giftId:number):Observable<string>{
    return this.http.post<string>(`${this.BASE_URL}/AddGiftToBasket?userId=${userId}&giftId=${giftId}`,null).pipe(
      tap(() => this.loadBasket(userId))
    );
  }

  removeGiftFromBasket(userId: string, giftId:number):Observable<string>{
    return this.http.put<string>(`${this.BASE_URL}/DeleteGiftFromBasket?userId=${userId}&giftId=${giftId}`,null).pipe(
      tap(() => this.loadBasket(userId))
    );
  }

  getBasket(userId: string) : Observable<BasketModel[]>{
    return this.http.get<BasketModel[]>(`${this.BASE_URL}/GetBasket/${userId}`);
  }

  checkOut(userId: string): Observable<any>{
    return this.http.put<any>(`${this.BASE_URL}/CheckOut/${userId}`, null).pipe(
      tap(() => this.loadBasket(userId))   // הסל מתרוקן רק אחרי הצלחה
    );
  }
}
