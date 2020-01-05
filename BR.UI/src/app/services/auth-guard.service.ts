import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, Route } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private usersService: UsersService, private router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.usersService.isAuthenticated().pipe(map(result => {
      if (result) {
        return true;
      }
      else {
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});

        return false;
      }
    }));
  }
}