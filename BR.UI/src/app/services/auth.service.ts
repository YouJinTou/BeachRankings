import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../environments/environment';
import { LoginResult } from '../models/login.result';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient: HttpClient, private cookieService: CookieService) { }

  login(email: string, password: string) {
    let data = {
      'email': email,
      'password': password
    };

    this.httpClient.post(environment.loginUrl, data).subscribe((o: LoginResult) => {
      this.cookieService.set('BR_LOGIN_RESULT', JSON.stringify(o));
      this.auth();
    });
  }

  auth() {
    let loginResult = JSON.parse(this.cookieService.get('BR_LOGIN_RESULT'));
    let data = {
      'username': loginResult['username'],
      'email': loginResult['email'],
      'accessToken': loginResult['accessToken']
    };

    this.httpClient.post(environment.authUrl, data).subscribe(o => console.log(o));
  }
}
