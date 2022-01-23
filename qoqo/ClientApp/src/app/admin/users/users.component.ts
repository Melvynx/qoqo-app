import { Component } from '@angular/core';
import { User } from '../../../types/users';
import { client } from '../../../utils/client';
import { SnackbarService } from '../../services/snackbar.service';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { ClientMessage } from '../../../types/api';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
})
export class UsersComponent {
  users: User[] = [];

  adminOnly = false;
  username = '';
  private timeout?: number;

  constructor(private snackbar: SnackbarService) {
    this.getUsers();
  }

  getUsers() {
    let url = 'admin/users';
    if (this.adminOnly) {
      url += '?isAdmin=true';
    }
    if (this.username) {
      url += (url.includes('?') ? '&' : '?') + 'username=' + this.username;
    }
    client<User[]>(url)
      .then((users) => {
        this.users = users;
      })
      .catch((err) => {
        this.snackbar.openError(err.message);
      });
  }

  onUserNameChange(event: Event) {
    this.username = (<HTMLInputElement>event.target).value;
    this.handleTimeout();
  }

  handleTimeout() {
    clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {
      this.getUsers();
    }, 300);
  }

  onAdminChange(event: MatCheckboxChange) {
    this.adminOnly = event.checked;
    this.handleTimeout();
  }

  onHandleAdmin(userId: number) {
    const user = this.users.find((u) => u.userId === userId);
    if (!user) return;
    client<ClientMessage>('admin/users/' + userId, {
      method: 'PUT',
      data: { isAdmin: !user.isAdmin },
    })
      .then((r) => {
        user.isAdmin = !user.isAdmin;
        this.snackbar.openError(r.message);
      })
      .catch((err) => {
        this.snackbar.openError(err.message);
      });
  }
}
