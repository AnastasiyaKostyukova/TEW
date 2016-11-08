import { Component } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { HttpService } from './services/httpService';
import { User } from './models/user';
import { Router} from '@angular/router';

@Component({
    selector: 'my-app',
    templateUrl: '../../scripts/angularjs/app/templates/app.html'
})

export class AppComponent {
    private userName: string;

    constructor(private httpService: HttpService, private router: Router) {
        ConstantStorage.setYandexTranslaterApiKey('dict.1.1.20160904T125311Z.5e2c6c9dfb5cd3c3.71b0d5220878e340d60dcfa0faf7f649af59c65f');

        this.userName = '';
        var url = '/api/UserInfo';
        this.httpService.processGet<User>(url).subscribe(response => this.setUserInfo(response));
    }

    private setUserInfo(user: User) {
        ConstantStorage.setUserName(user.Email);
        ConstantStorage.setUserId(user.Id);

        this.userName = user.Email;
        this.router.navigate(['/home']);
    }

    private logOff() {
        if (confirm("log out?")) {
            window.location.href = '/account/SignOff';
        }
    }
}