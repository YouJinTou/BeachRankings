import { Component, OnInit } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';
import { RegisterModel } from 'src/app/models/register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  username: string;
  email: string;
  password: string;
  confirmedPassword: string;
  resultVisible: boolean;
  resultMessage: string;

  constructor(private usersService: UsersService) { }

  ngOnInit() {
  }

  register() {
    let model = new RegisterModel(this.username, this.email, this.password, this.confirmedPassword);

    this.usersService.register(model).subscribe(r => {
      this.resultMessage = r.message;
      this.resultVisible = true;
    }, errorObject => {
      this.resultMessage = errorObject.error.message;
      this.resultVisible = true;
    });
  }
}
