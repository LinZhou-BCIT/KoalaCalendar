import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router'

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {
  message: string;

  constructor(private router: Router) { }

  ngOnInit() {
    this.message = sessionStorage.getItem('message');
    sessionStorage.removeItem('message');
  }

  logout(){
    sessionStorage.setItem('auth_token', null);
    this.message = "You are logged out.";
  }

}
