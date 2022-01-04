import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent {
  constructor(public authService: AuthService) {}
}
