import { Injectable } from '@angular/core';
import { CanActivate, Router}    from '@angular/router';

@Injectable()
export class AuthGuardService implements CanActivate  {
  constructor(private router: Router) {}
  canActivate(): boolean 
  {
    if (sessionStorage.getItem('logged_in') == 'true'){
      return true;
    }
    sessionStorage.setItem('message', 'Please Login First.');
    this.router.navigate(['/account/login']);
    return false;
  }
}
