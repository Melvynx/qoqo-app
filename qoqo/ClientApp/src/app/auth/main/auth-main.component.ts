import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'auth-main',
  templateUrl: './auth-main.component.html',
  styleUrls: ['./auth-main.component.css'],
})
export class AuthMainComponent implements OnInit {
  type: 'register' | 'login' = 'register';
  _authService: AuthService;

  constructor(authService: AuthService) {
    this._authService = authService;
  }

  ngOnInit(): void {}

  handle() {
    this.type = this.type === 'register' ? 'login' : 'register';
  }
}
