import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { AuthService } from '../services/auth.service';
import { User } from '../../types/users';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean | Promise<boolean> {
    if (this.authService.isLoading) {
      return new Promise((r) =>
        this.authService.userLoadFinish.subscribe((user) =>
          r(this.checkUser(user))
        )
      );
    } else {
      return this.checkUser(this.authService.user);
    }
  }

  checkUser(user?: User): boolean {
    if (!user) {
      this.router.navigateByUrl('/auth/login');
      return false;
    }
    return true;
  }
}
