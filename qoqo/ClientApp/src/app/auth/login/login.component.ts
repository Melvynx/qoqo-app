import { Component, OnInit, ViewChild } from '@angular/core';
import { InputComponent } from 'src/app/ui/input/input.component';
import { getBaseUrl } from 'src/main';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  @ViewChild('username') username?: InputComponent;
  @ViewChild('password') password?: InputComponent;

  errors = {
    username: '',
    password: '',
  };

  constructor() {}

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

    fetch(`${getBaseUrl()}api/users/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
      },
      body: JSON.stringify(user),
    })
      .then((r) => {
        if (r.status !== 200) {
          this.errors.password = 'Username or password is incorrect';
        }
        return r.json();
      })
      .then((r) => {
        console.log(r);
      });
  }
}
