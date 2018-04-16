import { Injectable }     from '@angular/core';
import { Component }      from '@angular/core';
import { URLSearchParams, QueryEncoder} from '@angular/http';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw'; 

export class LoginModel {
  Email: string;
  Password: string;
}

export class RegisterModel {
  Email: string;
  Password: string;
  ConfirmPassword: string;
  Role: string;
}

export class ConfirmEmailModel {
  Email: string;
  Code: string;
}

export class ForgotPasswordModel {
  Email: string;
}

export class ResetPasswordModel {
  Email: string;
  Password: string;
  ConfirmPassword: string;
  Code: string;
}

export class ChangePasswordModel {
  Email: string;
  Password: string;
  ConfirmPassword: string;
}

export class UserInfo {
  token: string;
  email: string;
  role: string;
}

@Injectable()
export class RemoteConnectService {
  private site: string;

  constructor(private http: Http) { 
     this.site = "https://koalacalendar.azurewebsites.net/api/accountapi/";
  }

  getUserInfo() {
    return JSON.parse(sessionStorage.getItem("userInfo"));
  }

  postRegister(regInfo: RegisterModel): Observable<Comment[]> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let url     = this.site + "register";

    return this.http.post(url, regInfo, options)
        .map(this.handleLogin) 
        .catch(this.handleError); 
  } 

  postLogin(loginInfo: LoginModel): Observable<Comment[]> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let url     = this.site + "login";

    return this.http.post(url, loginInfo, options)
        .map(this.handleLogin) 
        .catch(this.handleError); 
  } 

  getLoginStatus() {
    if(sessionStorage.getItem("userInfo") == null) {
      return false;
    }else {
      return true;
    }
  }

  logout() {
    sessionStorage.removeItem('userInfo');
  }

  confirmEmail(confirmInfo: ConfirmEmailModel) {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let url     = this.site + "confirmemail";

    return this.http.post(url, confirmInfo, options)
        .map(this.extractData) 
        .catch(this.handleError); 
  }

  forgotPassword(forgotPWInfo: ForgotPasswordModel) {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let url     = this.site + "forgotpassword";

    return this.http.post(url, forgotPWInfo, options)
        .map(this.extractData) 
        .catch(this.handleError); 
  }

  resetPassword(resetPWInfo: ResetPasswordModel) {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let url     = this.site + "resetpassword";

    return this.http.post(url, resetPWInfo, options)
        .map(this.extractData) 
        .catch(this.handleError); 
  }

  changePassword(changePWInfo: ChangePasswordModel) {
    let userInfo: UserInfo = this.getUserInfo();
    let token: string = userInfo.token;

    let headers = new Headers({ 'Content-Type': 'application/json' }); 

    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers
    });

    let url     = this.site + "changepassword";
    return this.http.post(url, changePWInfo, options)
        .map(this.extractData) 
        .catch(this.handleError); 
  }

  // Retreival of JSON from .NET is a success.
  private extractData(res: Response) {
    let body = res.json();
    return body;
  }

  private handleLogin(res: Response) {
    let body = res.json();
    if (body["token"] == null || body["userInfo"] == null) {
      return false;
    }
    let userInfo: UserInfo = new UserInfo();
    userInfo.token = body["token"];
    userInfo.email = body["userInfo"]["email"];
    userInfo.role = body["userInfo"]["role"];
    sessionStorage.setItem("userInfo", JSON.stringify(userInfo));
    return true;
  }

  // An error occurred. Notify the user.
  private handleError(error: any) {
      let errMsg = (error.message) ? error.message :
          error.status ? `${error.status} - ${error.statusText}` : 'Server error';
      return Observable.throw(errMsg);
  }

}
