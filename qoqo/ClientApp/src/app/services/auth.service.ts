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
  isAuthenticated: boolean;
  isLoading = true;
  userLoadFinish = new EventEmitter<User>();

  constructor() {
    this.isAuthenticated = getLocalStorage<boolean>('isLoggedIn', false);
    if (this.isAuthenticated) {
      client<User>('users/me')
        .then((user) => {
          this.userLoadFinish.emit(user);
          this.user = user;
        })
        .catch(() => {
          const numberOfRetry = getLocalStorage("logged-retry", 0)
          if (numberOfRetry >= 3) {
            this.setLoggedIn(false);
          } else {
            setLocalStorage("logged-retry", numberOfRetry + 1)
            this.isLoading = false;
          }
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
    this.isAuthenticated = loggedIn;
    setLocalStorage('isLoggedIn', loggedIn);
  }
}
