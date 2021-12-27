import { Injectable } from '@angular/core';
import { User } from '../../types/users';
import { getLocalStorage, setLocalStorage } from '../../utils/localStorage';
import { client } from '../../utils/client';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  user?: User = undefined;
  // keep isLoggedIn in localStorage to avoid useless request
  isAuthenticated: boolean;

  constructor() {
    this.isAuthenticated = getLocalStorage<boolean>('isLoggedIn', false);
    if (this.isAuthenticated) {
      client<User>('users/me')
        .then((user) => {
          this.user = user;
        })
        .catch(() => {
          this.setLoggedIn(false);
        });
    }
  }

  login(user: User) {
    this.user = user;
    console.log({ user });

    const token = user.token;
    if (token) {
      this.setLoggedIn(true);
    }
  }

  logout(): Promise<void> {
    return new Promise((resolve, reject) => {
      client('users/logout', { method: 'POST', json: false })
        .then(() => {
          this.user = undefined;
          console.log('Logged out', this);
          this.setLoggedIn(false);
          resolve();
        })
        .catch(reject);
    });
  }

  private setLoggedIn(loggedIn: boolean) {
    this.isAuthenticated = loggedIn;
    setLocalStorage('isLoggedIn', loggedIn);
  }
}
