import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { Router, ActivatedRoute } from '@angular/router';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email: string;
  password: string;
  private returnUrl: string;

  constructor(
    private usersService: UsersService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  login() {
    this.usersService.login(this.email, this.password).subscribe(r => {
      if (r['isSuccess']) {
        this.router.navigateByUrl(this.returnUrl);
      } else {
        console.log('CANOT LOGIN.');
      }
    });
  }
}
