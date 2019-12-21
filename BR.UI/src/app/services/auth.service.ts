import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { LoginResult } from '../models/login.result';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient: HttpClient) { }

  login(email: string, password: string) {
    let data = {
      'email': email,
      'password': password
    };

    this.httpClient.post(environment.loginUrl, data).subscribe((o: LoginResult) => console.log(o));
  }

  auth(loginResult: LoginResult) {
    let data = {
      'username': loginResult.username,
      'email': loginResult.email,
      'accessToken': loginResult.accessToken
    };

    this.httpClient.post(environment.authUrl, data).subscribe(o => console.log(o));
  }
}
