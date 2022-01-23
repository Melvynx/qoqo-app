import { Component, ViewChild } from '@angular/core';
import { InputComponent } from 'src/app/ui/input/input.component';
import { User } from 'src/types/users';
import { client } from 'src/utils/client';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'auth-view',
  templateUrl: './view.component.html',
})
export class ViewComponent {
  @ViewChild('firstname') firstname?: InputComponent;
  @ViewChild('lastname') lastname?: InputComponent;
  @ViewChild('email') email?: InputComponent;
  @ViewChild('street') street?: InputComponent;
  @ViewChild('npa') npa?: InputComponent;
  @ViewChild('city') city?: InputComponent;
  errors: Record<string, string> = {};

  constructor(public authService: AuthService, private router: Router) {}

  onLogout() {
    this.authService.logout().then(() => this.router.navigate(['/auth/login']));
  }

  onSubmit() {
    const user = {
      firstname: this.firstname?.getValue(),
      lastname: this.lastname?.getValue(),
      email: this.email?.getValue(),
      username: this.authService.user?.userName,
      street: this.street?.getValue(),
      npa: this.npa?.getValue(),
      city: this.city?.getValue(),
    };

    client<User>(`users/${this.authService.user?.userId}`, {
      method: 'PUT',
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
