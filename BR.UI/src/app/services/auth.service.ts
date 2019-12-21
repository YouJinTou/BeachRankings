import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../environments/environment';
import { LoginResult } from '../models/login.result';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private httpClient: HttpClient, 
    private cookieService: CookieService,
    private route: ActivatedRoute,
    private router: Router) { 
    }

  public isAuthenticated(): Observable<boolean> {
    if (!this.cookieService.check('BR_LOGIN_RESULT')) {
      return of(false);
    }

    let loginResult = JSON.parse(this.cookieService.get('BR_LOGIN_RESULT'));
    let isValidResult = 
      loginResult['username'] && loginResult['email'] && loginResult['accessToken']
    
    if (!isValidResult) {
      return of(false);
    }

    let data = {
      'username': loginResult['username'],
      'email': loginResult['email'],
      'accessToken': loginResult['accessToken']
    };
    console.log(data)
    return this.httpClient.post(environment.authUrl, data).pipe(map(r => {
      return r['isSuccess'];
    }))
  }

  public login(email: string, password: string): Observable<LoginResult> {
    let data = {
      'email': email,
      'password': password
    };

    return this.httpClient.post<LoginResult>(environment.loginUrl, data).pipe(map(r => {
      this.cookieService.set('BR_LOGIN_RESULT', JSON.stringify(r));

      return r;
    }));
  }
}
