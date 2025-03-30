import { Component } from '@angular/core';
import { AppComponent } from '../../../../app.component';
import {
  DatabaseUsersModel,
  DBUsersListModel,
  GetDBLimitsModel,
  UserLoginModel,
} from '../../../../../assets/models/UserModel/users.model';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { AppPagesApplicationComponent } from '../../../application/application.component';
import { waitForAsync } from '@angular/core/testing';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pages-components-users-userslist',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './userslist.component.html',
  styleUrl: './userslist.component.scss',
})
export class AppPagesComponentsUsersUserslistComponent {
  UserList: DBUsersListModel[] = [];
  DBLimitsModel: GetDBLimitsModel = new GetDBLimitsModel();
  UserModel: DatabaseUsersModel = new DatabaseUsersModel();

  LimnitNumber: number = 30;
  ToNumber: number = 0;
  blockPrev: boolean = true;
  blockNext: boolean = false;

  sleep = (ms: number) => new Promise((res) => setTimeout(res, ms));

  constructor(
    private route: Router,
    private apc: AppComponent,
    private app: AppPagesApplicationComponent
  ) {
    this.DBLimitsModel.limitedNumber = this.LimnitNumber;
    this.ToNumber = this.LimnitNumber;
    this.SetupPage();
  }

  async SetupPage() {
    this.apc.target = 'UsersControl';
    this.apc.payload.action = 'GetAllUsers';
    this.apc.payload.payload = this.DBLimitsModel;

    await this.apc.post();

    let result = this.apc.rstModel;

    if (!result.status) {
      alert(result.message);
      return;
    }

    this.DBLimitsModel.totalCount = result.message;
    this.UserList = result.payload;
    console.log(
      `${this.DBLimitsModel.offSetNumber} to ${this.LimnitNumber} of ${this.DBLimitsModel.totalCount}`
    );
  }

  ViewProfile(profile: DBUsersListModel) {
    this.app.UserProfileState = false;
    this.app.UserProfileData = profile;
    this.app.DisplayPage = 'UsersDetails';
  }

  UpdateProfile(profile: DBUsersListModel) {
    this.app.UserProfileState = true;
    this.app.UserProfileData = profile;
    this.app.DisplayPage = 'UsersDetails';
  }

  async DeleteProfile(profile: DBUsersListModel) {
    this.apc.target = 'UsersControl';
    this.apc.payload.action = 'DeleteSingleUser';
    this.apc.payload.payload = profile;

    await this.apc.post();

    let result = this.apc.rstModel;

    if (!result.status) {
      alert(result.message);
      return;
    }

    if (this.apc.UserModel.userId !== profile.userId) {
      this.SetupPage();
    } else {
      this.apc.UserModel = new DatabaseUsersModel();
      this.apc.IsHomePage = true;
      this.route.navigateByUrl('home');
    }
  }

  async DeleteAllUsers() {
    this.apc.target = 'UsersControl';
    this.apc.payload.action = 'DeleteAllUsers';

    await this.apc.post();

    let result = this.apc.rstModel;

    if (!result.status) {
      alert(result.message);
      return;
    }

    this.SetupPage();
  }

  async RegisterMultiUsers() {
    for (let i = 0; i < 1000; i++) {
      await this.RegisterUsers();
    }

    this.SetupPage();
  }

  async RegisterUsers() {
    this.apc.target = 'UsersControl';
    this.apc.payload.action = 'RegisterUser';
    this.UserModel = new DatabaseUsersModel();
    this.UserModel.GenNewUserNumber();
    this.apc.payload.payload = this.UserModel;

    await this.apc.post();

    let result = this.apc.rstModel;

    if (!result.status) {
      alert(result.message);
      return;
    }
  }

  MoveIndexPrevious() {
    if (this.blockPrev) {
      return;
    }

    this.blockNext = false;

    let a_prevNumber = this.DBLimitsModel.offSetNumber;
    let a_toNumber = this.ToNumber;
    let a_limnitNumber = this.LimnitNumber;
    let a_totalCount = this.DBLimitsModel.totalCount;

    a_prevNumber -= a_limnitNumber;
    a_totalCount -= a_limnitNumber;
    a_toNumber -= a_limnitNumber;

    if (a_prevNumber < 0) {
      a_prevNumber = 0;
      a_toNumber = a_limnitNumber;
      this.blockPrev = true;
    }

    console.log(`${a_prevNumber} to ${a_toNumber} of ${a_totalCount}`);

    this.DBLimitsModel.offSetNumber = a_prevNumber;
    this.ToNumber = a_toNumber;

    this.SetupPage();
  }

  MoveIndexNext() {
    if (this.blockNext) {
      return;
    }

    this.blockPrev = false;

    let b_prevNumber = this.DBLimitsModel.offSetNumber;
    let b_toNumber = this.ToNumber;
    let b_limnitNumber = this.LimnitNumber;
    let b_totalCount = this.DBLimitsModel.totalCount;

    b_prevNumber += b_limnitNumber;
    b_toNumber += b_limnitNumber;

    if (b_prevNumber >= b_totalCount) {
      b_prevNumber = b_totalCount - b_limnitNumber;
      b_totalCount = this.DBLimitsModel.totalCount;
      this.blockNext = true;
    }

    if (b_toNumber > b_totalCount) {
      b_toNumber = b_totalCount;
    }

    console.log(`${b_prevNumber} to ${b_toNumber} of ${b_totalCount}`);

    this.DBLimitsModel.offSetNumber = b_prevNumber;
    this.ToNumber = b_toNumber;

    this.SetupPage();
  }
}
