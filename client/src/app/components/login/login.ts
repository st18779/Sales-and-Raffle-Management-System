import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { User } from '../../models/User.model';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule,CommonModule,RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  email: string = '';
  password: string = '';
  showEmailError: boolean = false;
  showPasswordError: boolean = false;
  showErrorLogin: boolean = false

  authService: AuthService= inject(AuthService) 

  constructor(private router:Router){}

  IsValidEmail(){
   const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(this.email);

  }

  Login() {
      this.showEmailError = !this.IsValidEmail();
      this.showPasswordError = this.password.length < 8;
    if(!this.showEmailError && !this.showPasswordError){

       this.authService.login(this.email,this.password).subscribe({
        next: (data)=>{
          console.log('Login successful',data);
    
          this.router.navigate(['/gifts']);
        },
        error: (error) => {
          console.error('Login failed',error);
        this.showErrorLogin=true;
        }
    });
  }
}
}
