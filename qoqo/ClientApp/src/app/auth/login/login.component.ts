import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { InputComponent } from 'src/app/ui/input/input.component';
import { User } from '../../../types/users';
import { client } from '../../../utils/client';
import { AuthService } from '../../services/auth.service';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'auth-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  @ViewChild('username') username?: InputComponent;
  @ViewChild('password') password?: InputComponent;

  loginForm = this.formBuilder.group({ username: '', password: '' });

  errors = {
    username: '',
    password: '',
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private formBuilder: FormBuilder
  ) {
    if (this.authService.isLoggedIn) {
      this.router.navigate(['/auth/view']);
    }
  }

  onSubmit() {
    const user = {
      username: this.username?.getValue(),
      password: this.password?.getValue(),
    };
    if (!user.username) {
      this.errors.username = 'Username is required';
      return;
    }
    if (!user.password) {
      this.errors.password = 'Password is required';
      return;
    }

    client<User>('users/login', { data: user })
      .then((user) => {
        this.authService.login(user);
        this.router.navigate(['/auth/view']);
      })
      .catch(() => {
        this.errors.password = 'Username or password is incorrect';
      });
  }
}
