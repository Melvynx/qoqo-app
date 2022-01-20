import { EventEmitter, Injectable } from '@angular/core';
import { User } from '../../types/users';
import { getLocalStorage, setLocalStorage } from '../../utils/localStorage';
import { client } from '../../utils/client';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  user?: User = undefined;
  // keep isLoggedIn in localStorage to avoid useless request
  isLoggedIn: boolean;
  isLoading = true;
  userLoadFinish = new EventEmitter<User>();

  get isAuthenticated(): boolean {
    if (this.isLoading) {
      return this.isLoggedIn;
    }
    return Boolean(this.user);
  }

  constructor() {
    this.isLoggedIn = getLocalStorage<boolean>('isLoggedIn', false);
    if (this.isLoggedIn) {
      client<User>('users/me')
        .then((user) => {
          this.userLoadFinish.emit(user);
          this.user = user;
        })
        .catch(() => {
          // don't update the local storage to avoid false-positive
          this.isLoggedIn = false;
        })
        .finally(() => {
          this.isLoading = false;
        });
    } else {
      this.isLoading = false;
      this.userLoadFinish.emit();
    }
  }

  login(user: User) {
    this.user = user;

    const token = user.token;
    if (token) {
      this.setLoggedIn(true);
    }
  }

  logout(): Promise<void> {
    // I can't avoid this, because I use the custom `client`
    return new Promise((resolve, reject) => {
      client('users/logout', { method: 'POST', json: false })
        .then(() => {
          this.user = undefined;
          this.setLoggedIn(false);
          resolve();
        })
        .catch(reject);
    });
  }

  private setLoggedIn(loggedIn: boolean) {
    this.isLoggedIn = loggedIn;
    setLocalStorage('isLoggedIn', loggedIn);
  }
}
