import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { GiftsNav } from './components/gifts-nav/gifts-nav';
import { GiftsList } from './components/gifts-list/gifts-list';
import { GiftForm } from './components/gift-form/gift-form';
import { authGuard } from './core/auth.guard';
import { SingleGift } from './components/single-gift/single-gift';
import { checkOut } from './components/checkOut/checkOut';
import { endCheckOut } from './components/endCheckOut/endCheckOut';
import { DonorsNav } from './components/donors-nav/donors-nav';
import { DonorsList } from './components/donors-list/donors-list';
import { DonorForm } from './components/donor-form/donor-form';
import { Basket } from './components/basket/basket';

export const routes: Routes = [
    {path: '',component: Home},
    {path:'login',component:Login},
    {path:'register',component:Register},
    {path:'basket/:userId',component:Basket},
    {path:'gifts', component:GiftsNav, canActivate:[authGuard], children:[
        {path:'',component:GiftsList},
        {path:'add',component:GiftForm},
        {path:'edit/:id',component:GiftForm},
        {path: 'checkOut', component: checkOut},
        {path: 'endCheckOut', component: endCheckOut},
        {path:':id', component: SingleGift},
    ]},
    {path:'donors', component:DonorsNav, canActivate:[authGuard], children:[
        {path:'',component:DonorsList},
        {path:'add',component:DonorForm},
        {path:'edit/:id',component:DonorForm}, 
    ]}
];
