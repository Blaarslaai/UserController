import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponent } from '../../app.component';
import { DatabaseUsersModel } from '../../../assets/models/UserModel/users.model';

@Component({
  selector: 'app-components-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class AppComponentsHeaderComponent {
  constructor(private route: Router, private apc: AppComponent) {}

  async logout() {
    this.apc.target = 'UsersControl';
    this.apc.payload.action = 'UserLogout';

    await this.apc.post();

    let result = this.apc.rstModel;

    console.log(result);

    if (!result.status) {
      alert(result.message);
      return;
    }

    this.apc.UserModel = new DatabaseUsersModel();
    this.apc.IsHomePage = result.status;
    this.route.navigateByUrl('home');
  }
}
