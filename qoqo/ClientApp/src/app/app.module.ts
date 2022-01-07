import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { ClickButtonComponent } from './offers/click-button/click-button.component';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './auth/login/login.component';
import { InputComponent } from './ui/input/input.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthComponent } from './auth/auth.component';
import { ViewComponent } from './auth/view/view.component';
import { AuthButtonComponent } from './auth/button/auth-button.component';
import { AuthGuard } from './guard/auth.guard';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ClickProfileComponent } from './auth/click/click-profile.component';
import { OrderProfileComponent } from './auth/order/order-profile.component';
import { AdminComponent } from './admin/admin.component';
import { AdminGuard } from './guard/admin.guard';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { OffersComponent } from './admin/offers/offers.component';
import { OfferCrudComponent } from './admin/offers/crud/offer-crud.component';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { OrdersComponent } from './admin/orders/orders.component';
import { OrderViewComponent } from './admin/orders/view/order-view.component';
import { NoOfferComponent } from './offers/no-offer/no-offer.component';
import { UsersComponent } from './admin/users/users.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { ClickComponent } from './offers/click/click.component';
import { MatIconModule } from '@angular/material/icon';

const ROUTES: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'offers/:id', component: HomeComponent },
  { path: 'nothing', component: NoOfferComponent },
  { path: 'counter', component: CounterComponent },
  { path: 'users', component: UsersComponent },
  {
    path: 'auth',
    component: AuthComponent,
    children: [
      { path: 'login', component: LoginComponent },
      {
        path: 'register',
        component: RegisterComponent,
      },
      {
        path: 'view',
        component: ViewComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'click',
        component: ClickProfileComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'order',
        component: OrderProfileComponent,
        canActivate: [AuthGuard],
      },
    ],
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AdminGuard],
    children: [
      { path: '', component: DashboardComponent },
      { path: 'dashboard', component: DashboardComponent },
      {
        path: 'offers',
        component: OffersComponent,
      },
      {
        path: 'offers/:id',
        component: OfferCrudComponent,
      },
      {
        path: 'orders/:id',
        component: OrderViewComponent,
      },
      {
        path: 'orders',
        component: OrdersComponent,
      },
      {
        path: 'users',
        component: UsersComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    UsersComponent,
    ClickProfileComponent,
    ClickButtonComponent,
    FooterComponent,
    InputComponent,
    AuthComponent,
    ViewComponent,
    LoginComponent,
    RegisterComponent,
    AuthButtonComponent,
    ClickProfileComponent,
    OrderProfileComponent,
    AdminComponent,
    DashboardComponent,
    OffersComponent,
    OfferCrudComponent,
    OrdersComponent,
    OrderViewComponent,
    NoOfferComponent,
    ClickComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(ROUTES),
    BrowserAnimationsModule,
    MatSnackBarModule,
    MatInputModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    MatExpansionModule,
    MatIconModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
