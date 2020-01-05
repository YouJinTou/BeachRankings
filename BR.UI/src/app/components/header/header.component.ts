import { Component, OnInit } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';
import { LoginResult } from 'src/app/models/login.result';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  isLoggedIn: boolean;
  user: LoginResult;

  constructor(private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.isAuthenticated().subscribe(r => this.isLoggedIn = r);

    this.usersService.currentUser.subscribe(r => this.user = r);
  }

  logout() {
    this.usersService.logout();

    this.isLoggedIn = false;
  }
}
