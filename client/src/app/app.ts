import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { User } from './models/User.model';
import { Basket } from './components/basket/basket';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,RouterLink,Basket],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('client');

  isAdmin: boolean = false;
  isUserLoggedIn: boolean = false;
  basketOpen: boolean = false;
  user= new User()
  private authService = inject(AuthService);
  
  async ngOnInit() {
      this.authService.user$.subscribe(userData => {
      this.isUserLoggedIn = !!userData;
      this.user = userData || new User();
      this.isAdmin = userData?.isAdmin || false;
      if (!userData) this.basketOpen = false;
    });
  
  }

  logOut(){
    this.authService.logout()
  }

}
