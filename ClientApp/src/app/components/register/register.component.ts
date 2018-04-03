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

  resolved(captchaResponse: string) {
    console.log(`Resolved captcha with response ${captchaResponse}:`);
}

}
