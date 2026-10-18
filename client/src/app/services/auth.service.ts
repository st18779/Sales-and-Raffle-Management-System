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
   constructor(){
    // טוקן שפג תוקפו מכניסה קודמת - לא מציגים את המשתמש כמחובר
    if (!this.hasValidToken()) this.clearSession();
   }
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
  // האם יש טוקן שעדיין בתוקף (לפי שדה exp בטוקן)
  hasValidToken(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')));
      return typeof payload.exp === 'number' && payload.exp * 1000 > Date.now();
    } catch {
      return false; // טוקן פגום
    }
  }

  // מוחק את פרטי ההתחברות בלי לנווט
  clearSession() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.userSubject.next(null);
  }

  logout() {
    this.clearSession();
    this.router.navigate(['/login']);
  }

}
