import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (localStorage.getItem('token')) {
      return true;
    }
    console.log(state.url);
    this.router.navigate(['account/login'], {
      queryParams: { returnUrl: state.url },
    });
    return false;
    // return this.accountService.currentUser$.pipe(
    //   map((auth) => {
    //     console.log(auth);
    //     if (auth) {
    //       return true;
    //     }
    //     this.router.navigate(['account/login'], {
    //       queryParams: { returnUrl: state.url },
    //     });
    //     return false;
    //   })
    // );
  }
}
