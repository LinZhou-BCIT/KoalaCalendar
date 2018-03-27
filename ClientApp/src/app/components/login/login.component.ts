import { Component, OnInit } from '@angular/core';
import { RemoteConnectService, LoginModel } from '../../services/remote-connect.service';
import { Router } from '@angular/router'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [RemoteConnectService]
})
export class LoginComponent implements OnInit {
  email: string;
  password: string;
  message: string;

  constructor(private remoteService: RemoteConnectService, 
              private router: Router) { }

  ngOnInit() {
    this.message = sessionStorage.getItem('message');
    sessionStorage.removeItem('message');
  }
  login(){
    let loginInfo: LoginModel = {
      Email : this.email,
      Password : this.password
    };
    this.remoteService.postLogin(loginInfo).subscribe(
      // Success.
      success => {
          if(success) {
            this.router.navigate(['/calendar']);
          }          
      },
      // Error.
      error => {
          alert(error);
      });
  }
}
