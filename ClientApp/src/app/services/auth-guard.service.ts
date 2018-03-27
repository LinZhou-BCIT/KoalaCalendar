import { Injectable } from '@angular/core';
import { CanActivate, Router}    from '@angular/router';
import { RemoteConnectService } from './remote-connect.service';

@Injectable()
export class AuthGuardService implements CanActivate  {

  constructor(private router: Router, private remoteService: RemoteConnectService) {}
  canActivate(): boolean 
  {
    if (this.remoteService.getLoginStatus()){
      return true;
    }

    sessionStorage.setItem('message', 'Please Login First.');
    this.router.navigate(['/account/login']);
    return false;
  }
}
