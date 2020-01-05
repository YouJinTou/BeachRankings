import { Component, OnInit } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  isLoggedIn: boolean;

  constructor(private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.isAuthenticated().subscribe(r => this.isLoggedIn = r);
  }

  logout() {
    this.usersService.logout();

    this.isLoggedIn = false;
  }
}
