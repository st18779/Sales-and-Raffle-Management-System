import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Gift } from '../models/Gift.model';
import { Observable } from 'rxjs';
import { Category } from '../models/Category.model';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  BASE_URL = 'http://localhost:8088/api/Category';

  http: HttpClient = inject(HttpClient);
  constructor() { }

  getCategoriesList(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.BASE_URL}/categoriesList`);
  }

  getCategoryById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.BASE_URL}/${id}`);
  }

  updateCategory(id: number,category:Category): Observable<Gift> {
    return this.http.put<Gift>(`${this.BASE_URL}/${id}`, category);
  }

  deleteCategory(id: number): Observable<Category> {
    return this.http.delete<Category>(`${this.BASE_URL}/${id}`);
  }

  createCategory(category:Category): Observable<Category> {
    return this.http.post<Category>(`${this.BASE_URL}`, category);
  }


  

}
