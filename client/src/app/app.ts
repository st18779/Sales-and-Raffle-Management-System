import { Component, inject, signal, ViewChild } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { User } from './models/User.model';
import { Basket } from './components/basket/basket';
import { AuthService } from './services/auth.service';
import { PurchasesService } from './services/purchases.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,RouterLink,RouterLinkActive,AsyncPipe,Basket],
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
  itemCount$ = inject(PurchasesService).itemCount$;

  // הסל הצף - נפתח גם מהתפריט
  @ViewChild(Basket) basket?: Basket;

  async ngOnInit() {
      this.authService.user$.subscribe(userData => {
      this.isUserLoggedIn = !!userData;
      this.user = userData || new User();
      this.isAdmin = userData?.isAdmin || false;
      if (!userData) this.basketOpen = false;
    });

  }

  toggleBasket() {
    this.basket?.toggle();
  }

  logOut(){
    this.authService.logout()
  }

}
