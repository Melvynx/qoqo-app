import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { UsersComponent } from './users/users.component';
import { OfferPageComponent } from './offer-page/offer-page.component';
import { ClickComponent } from './offer/click/click.component';
import { ClickButtonComponent } from './offer/click-button/click-button.component';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './auth/login/login.component';
import { InputComponent } from './ui/input/input.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthMainComponent } from './auth/main/auth-main.component';
import { ViewComponent } from './auth/view/view.component';
import { AuthButtonComponent } from './auth/button/auth-button.component';
import { AuthGuard } from './guard/auth.guard';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthClickComponent } from './auth/auth-click/auth-click.component';
import { AuthOrderComponent } from './auth/auth-order/auth-order.component';

const ROUTES: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'users', component: UsersComponent },
  { path: 'offer', component: OfferPageComponent },
  {
    path: 'auth',
    component: AuthMainComponent,
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
        component: AuthClickComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'order',
        component: AuthOrderComponent,
        canActivate: [AuthGuard],
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
    FetchDataComponent,
    UsersComponent,
    OfferPageComponent,
    ClickComponent,
    ClickButtonComponent,
    FooterComponent,
    InputComponent,
    AuthMainComponent,
    ViewComponent,
    LoginComponent,
    RegisterComponent,
    AuthButtonComponent,
    AuthClickComponent,
    AuthOrderComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(ROUTES),
    NoopAnimationsModule,
    MatSnackBarModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
