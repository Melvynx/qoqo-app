import { Location } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { InputComponent } from 'src/app/ui/input/input.component';
import { User } from 'src/types/users';
import { client } from 'src/utils/client';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'auth-view',
  templateUrl: './view.component.html',
  styleUrls: ['./view.component.css'],
})
export class ViewComponent implements OnInit {
  @ViewChild('firstname') firstname?: InputComponent;
  @ViewChild('lastname') lastname?: InputComponent;
  @ViewChild('email') email?: InputComponent;
  errors: Record<string, string> = {};

  constructor(public authService: AuthService, private location: Location) {}

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  ngOnInit(): void {}

  onLogout() {
    this.authService
      .logout()
      .then(() => this.location.replaceState('/auth/login'));
  }

  onSubmit() {
    const user = {
      firstname: this.firstname?.getValue(),
      lastname: this.lastname?.getValue(),
      email: this.email?.getValue(),
      username: this.authService.user?.userName,
    };

    client<User>(`users/${this.authService.user?.id}`, {
      method: 'PATCH',
      data: user,
    })
      .then((user) => {
        this.authService.login(user);
      })
      .catch((err) => {
        this.errors = err;
      });
  }
}
