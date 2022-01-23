import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'auth-button',
  templateUrl: './auth-button.component.html',
})
export class AuthButtonComponent {
  constructor(public authService: AuthService) {}
}
