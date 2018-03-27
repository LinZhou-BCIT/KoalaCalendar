import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RemoteConnectService } from '../../services/remote-connect.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {
  //message: string;

  constructor(private router: Router, private remoteService: RemoteConnectService) { }

  ngOnInit() {
    // this.message = sessionStorage.getItem('message');
    // sessionStorage.removeItem('message');

    this.remoteService.logout(); 
    // sessionStorage.setItem('auth_token', null);
    // sessionStorage.setItem('logged_in', 'false');
    // this.message = "You are logged out.";
    this.router.navigate(['/account/login']);
  }

}
