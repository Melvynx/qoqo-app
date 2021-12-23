import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'auth-main',
  templateUrl: './auth-main.component.html',
  styleUrls: ['./auth-main.component.css'],
})
export class AuthMainComponent implements OnInit {
  type: 'register' | 'login' = 'login';
  _authService: AuthService;

  constructor(authService: AuthService) {
    this._authService = authService;
  }

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  ngOnInit(): void {}

  handle() {
    this.type = this.type === 'register' ? 'login' : 'register';
  }
}
