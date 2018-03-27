import { Component, OnInit } from '@angular/core';
import { Router, Event, NavigationStart } from '@angular/router';
import { RemoteConnectService } from '../../services/remote-connect.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './app-navbar.component.html',
  styleUrls: ['./app-navbar.component.css']
})
export class AppNavbarComponent implements OnInit {
  isLoggedIn: boolean = false;
  constructor(private router : Router, private remoteService: RemoteConnectService) { }

  ngOnInit() {
    
    this.router.events.subscribe( 
      (event : Event ) => { if(event instanceof NavigationStart) 
      {
        if (this.remoteService.getLoginStatus()){
          this.isLoggedIn = true;
        }else{
          this.isLoggedIn = false;
        }
      }}
  );

  }

}
