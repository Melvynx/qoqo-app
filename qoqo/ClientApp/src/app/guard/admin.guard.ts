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
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean | Promise<boolean> {
    if (this.authService.isLoading) {
      return new Promise((r) =>
        this.authService.userLoadFinish.subscribe((user) =>
          r(this.checkAdmin(user))
        )
      );
    } else {
      return this.checkAdmin(this.authService.user);
    }
  }

  checkAdmin(user?: User): boolean {
    if (!user?.isAdmin) {
      this.router.navigateByUrl('/');
      return false;
    }
    return true;
  }
}
