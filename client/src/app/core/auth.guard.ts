import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
    const authService = inject(AuthService);

    if (authService.hasValidToken()) {
        return true;
    }
    authService.clearSession();
    return inject(Router).createUrlTree(['/login']);
};
