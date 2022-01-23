import { Component, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { InputComponent } from 'src/app/ui/input/input.component';
import { User } from 'src/types/users';
import { client } from 'src/utils/client';
import { Router } from '@angular/router';

@Component({
  selector: 'auth-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  @ViewChild('username') username?: InputComponent;
  @ViewChild('firstname') firstname?: InputComponent;
  @ViewChild('lastname') lastname?: InputComponent;
  @ViewChild('email') email?: InputComponent;
  @ViewChild('confirmPassword') confirmPassword?: InputComponent;
  @ViewChild('password') password?: InputComponent;

  errors: Record<string, string> = {};

  constructor(private authService: AuthService, private router: Router) {
    if (this.authService.isLoggedIn) {
      this.router.navigate(['/auth/view']);
    }
  }

  onSubmit() {
    const user = {
      username: this.username?.getValue(),
      password: this.password?.getValue(),
      firstname: this.firstname?.getValue(),
      lastname: this.lastname?.getValue(),
      email: this.email?.getValue(),
    };
    if (!user.username) {
      this.errors.username = 'Username is required';
      return;
    }
    if (!user.password) {
      this.errors.password = 'Password is required';
      return;
    }
    if (!user.email) {
      this.errors.email = 'Email is required';
      return;
    }
    if (user.password !== this.confirmPassword?.getValue()) {
      this.errors.confirmPassword = 'Passwords do not match';
      return;
    }

    client<User>('users/register', { data: user })
      .then((user) => {
        this.authService.login(user);
        this.router.navigate(['/auth/view']);
      })
      .catch((err) => {
        this.errors = err;
      });
  }
}
