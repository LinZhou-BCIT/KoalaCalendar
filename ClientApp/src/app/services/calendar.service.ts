import { Injectable } from '@angular/core';
import { URLSearchParams, QueryEncoder} from '@angular/http';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw'; 
import 'rxjs/add/observable/of'; 
import { RemoteConnectService, UserInfo } from './remote-connect.service';

export class Event {
  eventID: string;
  name: string;
  startTime: Date;
  endTime: Date;
  calendarID: string;
}

export class Calendar {
  calendarID: string;
  name: string;
  accessCode: string;
  owner: string; // just the email
  events: Event[];
}

@Injectable()
export class CalendarService {
  private site: string;

  constructor(private http: Http, private remoteService: RemoteConnectService) {
    this.site = "https://apiserver20180208041703.azurewebsites.net/api/calendarapi/";
  }

  getCalendarForMonth(month: number, year: number): Observable<Calendar[]>  {
    let keyForSession: string = "cal-" + year + "-" + month;
    let item = sessionStorage.getItem(keyForSession);
    if (item == null) {
      let userInfo: UserInfo = this.remoteService.getUserInfo();
      let token: string = userInfo.token;
  
      let headers = new Headers({ 'Content-Type': 'application/json' }); 
  
      headers.append( 'Authorization', 'Bearer ' + token)
      let options = new RequestOptions({
          headers: headers
      });
      console.log(headers);
  
      let dataUrl = this.site + 'getcalendar?year=' + year + "&month=" + month;  
      return this.http.get(dataUrl, options)
          .map(this.extractData)
          .catch(this.handleError);
    }
    else {
      return Observable.of(JSON.parse(item));
    }

  }

  private extractData(res: Response) {
    let body = res.json();
    return body;
  }

  private handleError(error: any) {
    let errMsg = (error.message) ? error.message :
        error.status ? `${error.status} - ${error.statusText}` : 'Server error';
    return Observable.throw(errMsg);
}

}
