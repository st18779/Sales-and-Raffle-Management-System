import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { User } from '../models/User.model';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
   BASE_URL = 'http://localhost:8088/api/Auth';

   http: HttpClient = inject(HttpClient);
   router: Router = inject(Router);
   constructor(){ }
   private userSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('user') || 'null'));

   user$ = this.userSubject.asObservable();

   register(user: User): Observable<User> {
    return this.http.post<User>(`${this.BASE_URL}/register`, user);
   }
   
   login(email:string,password:string):Observable<any>{
    return this.http.post<any>(`${this.BASE_URL}/login`,{email,password}).pipe(
     
      tap(response =>{
        if(response && response.token){
          localStorage.setItem('token',response.token);
          localStorage.setItem('user',JSON.stringify(response.user));
          this.userSubject.next(response.user);
        }
      })
      ) 
   
  }
  logout() {
  localStorage.removeItem('token');
  localStorage.removeItem('user');
  this.userSubject.next(null);
  this.router.navigate(['/login']);
}

}
