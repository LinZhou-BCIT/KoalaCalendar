import { Component, OnInit } from '@angular/core';
import { Router, Event, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './app-navbar.component.html',
  styleUrls: ['./app-navbar.component.css']
})
export class AppNavbarComponent implements OnInit {
  isLoggedIn: boolean = false;
  constructor(private router : Router) { }

  ngOnInit() {
    
    this.router.events.subscribe( 
      (event : Event ) => { if(event instanceof NavigationStart) 
      {
        if (sessionStorage.getItem('logged_in') == 'true'){
          this.isLoggedIn = true;
        }else{
          this.isLoggedIn = false;
        }
      }}
  );

  }

}
