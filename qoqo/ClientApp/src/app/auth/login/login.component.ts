import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { InputComponent } from 'src/app/ui/input/input.component';
import { client } from '../../../utils/client';
import { User } from '../../../types/users';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'auth-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  @ViewChild('username') username?: InputComponent;
  @ViewChild('password') password?: InputComponent;

  @Output() onRegister = new EventEmitter();

  private _authService: AuthService;

  errors = {
    username: '',
    password: '',
  };

  constructor(authService: AuthService) {
    this._authService = authService;
  }

  ngOnInit(): void {}

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
        this._authService.login(user);
      })
      .catch(() => {
        this.errors.password = 'Username or password is incorrect';
      });
  }
}
