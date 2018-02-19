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
}

@Injectable()
export class RemoteConnectService {
  public site: string;
  constructor(private http: Http) { 
     this.site = "https://apiserver20180208041703.azurewebsites.net/api/accountapi/"
  }

  postRegister(regInfo: RegisterModel): Observable<Comment[]> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let url     = this.site + "register";

    return this.http.post(url, regInfo, options)
        .map(this.extractData) 
        .catch(this.handleError); 
  } 

  postLogin(loginInfo: LoginModel): Observable<Comment[]> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let url     = this.site + "login";

    return this.http.post(url, loginInfo, options)
        .map(this.extractData) 
        .catch(this.handleError); 
  } 

  getUsers(): Observable<Comment[]> {
    let headers = new Headers({ 'Content-Type': 'application/json' }); 

    // Need to include 'Authorization' property with token in header.
    // Read token value from the JavaScript session.
    headers.append( 'Authorization', 'Bearer ' 
                 + sessionStorage.getItem('auth_token'))
    let options = new RequestOptions({
        headers: headers
    });
    console.log(headers);

    let dataUrl = this.site + 'users';  
    return this.http.get(dataUrl, options)
        .map(this.extractData)
        .catch(this.handleError);
  } 
  // Retreival of JSON from .NET is a success.
  private extractData(res: Response) {
    let body = res.json();
    return body;
  }

  // An error occurred. Notify the user.
  private handleError(error: any) {
      let errMsg = (error.message) ? error.message :
          error.status ? `${error.status} - ${error.statusText}` : 'Server error';
      return Observable.throw(errMsg);
  }

}
