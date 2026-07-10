import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  id: string = '';
  fullName: string = '';
  phone!: string;
  email: string = '';
  password: string = '';
  showIdError: boolean = false;
  showFullNameError: boolean = false;
  showPhoneError: boolean = false;
  showEmailError: boolean = false;
  showPasswordError: boolean = false;
  showErrorRegister: boolean = false

  authService: AuthService = inject(AuthService)

  constructor(private router: Router) { }

  IsValidEmail() {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(this.email);
  }

  IsValidPhone() {
    if (this.phone.length !== 9 && this.phone.length !== 10)
      return false;
    if (this.phone[0] != '0')
      return false
    return true
  }

  IsValidId() {
    if (this.id.length != 9)
      return false
    return true
  }

  Register() {
    this.showEmailError = !this.IsValidEmail();
    this.showPhoneError = !this.IsValidPhone()
    this.showPasswordError = this.password.length < 8;
    this.showIdError = this.id.length != 9

    if (!this.showEmailError && !this.showPasswordError && !this.showFullNameError && !this.showPhoneError && !this.showIdError) {
      {

        this.authService.register({ id: this.id, fullName: this.fullName, phone: this.phone, email: this.email, password: this.password }).subscribe({
          next: (res) => {

            console.log('נרשם בהצלחה:', res);
            this.router.navigate(['/login']);
          },
          error: (err) => {

            console.error('שגיאה ברישום:', err);
            this.showErrorRegister = true;
          }
        });


      }}}
    }
