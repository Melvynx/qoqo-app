import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'auth-button',
  templateUrl: './auth-button.component.html',
  styleUrls: ['./auth-button.component.css'],
})
export class AuthButtonComponent implements OnInit {
  constructor(public authService: AuthService) {}

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  ngOnInit(): void {}

  getButtonText() {
    return this.authService.isAuthenticated ? 'Logout' : 'Login';
  }
}
