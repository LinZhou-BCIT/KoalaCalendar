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
      data => {
          // Store token with session data.
          sessionStorage.setItem('auth_token', data["token"]);
          sessionStorage.setItem('logged_in', 'true');
          console.log(data);   
          this.router.navigate(['/calendar']);
      },
      // Error.
      error => {
          alert(error);
      });
  }
}
