import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'auth-button',
  templateUrl: './auth-button.component.html',
  styleUrls: ['./auth-button.component.css'],
})
export class AuthButtonComponent implements OnInit {
  _authService: AuthService;

  constructor(authService: AuthService) {
    this._authService = authService;
  }

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  ngOnInit(): void {}

  getButtonText() {
    return this._authService.isAuthenticated ? 'Logout' : 'Login';
  }
}
