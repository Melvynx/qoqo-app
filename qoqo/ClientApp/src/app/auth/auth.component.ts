import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent {
  _authService: AuthService;

  constructor(authService: AuthService) {
    console.log('AuthComponent.constructor()', authService);
    this._authService = authService;
  }
}
