import { Component, OnInit } from '@angular/core';
import { RemoteConnectService, RegisterModel } from '../../services/remote-connect.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [RemoteConnectService]
})
export class RegisterComponent implements OnInit {
  email: string;
  password: string;
  confirmPassword: string;
  role: string;

  constructor(private remoteService: RemoteConnectService, 
    private router: Router) { }

  ngOnInit() {
  }
  register(){
    let regInfo: RegisterModel = {
      Email : this.email,
      Password : this.password,
      ConfirmPassword: this.confirmPassword,
      Role: this.role
    };
    this.remoteService.postRegister(regInfo).subscribe(
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
