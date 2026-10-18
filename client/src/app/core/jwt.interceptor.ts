import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = localStorage.getItem('token');

  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((err: HttpErrorResponse) => {
      // השרת דחה את הטוקן (לא תקף / פג תוקף) - מנתקים ושולחים להתחברות.
      // לא על בקשות התחברות/הרשמה, שם 401 פירושו סיסמה שגויה.
      if (err.status === 401 && token && !req.url.includes('/api/Auth/')) {
        authService.logout();
      }
      return throwError(() => err);
    })
  );
};
