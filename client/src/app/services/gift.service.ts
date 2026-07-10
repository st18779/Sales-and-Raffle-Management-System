import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Gift } from '../models/Gift.model';
import { Observable } from 'rxjs';
import { GiftFormModel } from '../models/GiftFormModel.model';
import { User } from '../models/User.model';

@Injectable({
  providedIn: 'root',
})
export class GiftService {
  BASE_URL = 'http://localhost:8088/api/Gift';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  getGiftsList(): Observable<Gift[]> {
    return this.http.get<Gift[]>(`${this.BASE_URL}/giftsList`);
  }

  getGiftById(id: number): Observable<Gift> {
    return this.http.get<Gift>(`${this.BASE_URL}/${id}`);
  }

  updateGift(id: number,gift:GiftFormModel): Observable<Gift> {
    return this.http.put<Gift>(`${this.BASE_URL}/${id}`, gift);
  }

  deleteGift(id: number): Observable<string> {
    return this.http.delete<string>(`${this.BASE_URL}/${id}`);
  }

  getGiftsByCategory(categoryId: number): Observable<Gift[]> {
    return this.http.get<Gift[]>(`${this.BASE_URL}/GiftsByCategory/${categoryId}`);
  }

   SearchGiftsByName(name: string): Observable<Gift[]> {
    const params = new HttpParams().set('name',name)
    return this.http.get<Gift[]>(`${this.BASE_URL}/SearchGiftByName`,{params});
  }

  SearchGiftsByDonorName(name: string): Observable<Gift[]> {
    const params = new HttpParams().set('name',name)
    return this.http.get<Gift[]>(`${this.BASE_URL}/SearchGiftByDonorName`,{params});
  }

  createGift(gift:GiftFormModel): Observable<Gift> {
    return this.http.post<Gift>(`${this.BASE_URL}`, gift);
  }

  randomWinner(giftId:number): Observable<User> {
    return this.http.post<User>(`${this.BASE_URL}/randomWinner`, giftId);
  }

   getGiftsOrderByPrice(): Observable<Gift[]> {
    return this.http.get<Gift[]>(`${this.BASE_URL}/GiftsOrderByPrice`);
  }

   getGiftsOrderByCategory(): Observable<Gift[]> {
    return this.http.get<Gift[]>(`${this.BASE_URL}/GetGiftsOrderByCategory`);
  }

    getGiftWithWinner(giftId: number): Observable<Gift> {
    return this.http.get<Gift>(`${this.BASE_URL}/${giftId}`);
  }

   getGiftsWithWinners(giftId: number): Observable<Gift[]> {
    return this.http.get<Gift[]>(`${this.BASE_URL}/GetGiftsWithWinners`);
  }

}
