import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponent } from '../../app.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  DatabaseUsersModel,
  UserLoginModel,
} from '../../../assets/models/UserModel/users.model';

@Component({
  selector: 'app-pages-home',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class AppPagesHomeComponent {
  UserModel: DatabaseUsersModel = new DatabaseUsersModel();
  LogonModel: UserLoginModel = new UserLoginModel();

  RegisterActive: boolean = false;
  loginBoxHeight: number = 200;

  constructor(private route: Router, private apc: AppComponent) {
    this.apc.IsHomePage = true;

    this.LogonModel.userName = 'johan.botha@payteq.com1743326282902';
    this.LogonModel.password = '5404271';
    //this.LoginUser();
  }

  async RegisterUser() {
    this.apc.target = 'UsersControl';
    this.apc.payload.action = 'RegisterUser';
    this.UserModel.GenNewUserNumber();
    this.apc.payload.payload = this.UserModel;

    await this.apc.post();

    let result = this.apc.rstModel;

    if (!result.status) {
      alert(result.message);
      return;
    }

    alert('User registered please login');
    this.LogonModel.userName = this.UserModel.emailAddress;
    this.ActivateRegistering();
  }

  async LoginUser() {
    this.apc.target = 'UsersControl';
    this.apc.payload.action = 'UserLogin';
    this.apc.payload.payload = this.LogonModel;

    await this.apc.post();

    let result = this.apc.rstModel;

    console.log(result);

    if (!result.status) {
      alert(result.message);
      return;
    }

    this.apc.UserModel = result.payload.profile;
    this.apc.IsHomePage = false;
    this.route.navigateByUrl('application');
  }

  ActivateRegistering() {
    if (this.loginBoxHeight == 200) {
      this.loginBoxHeight = 270;
      this.RegisterActive = true;
    } else {
      this.loginBoxHeight = 200;
      this.RegisterActive = false;
    }
  }
}
