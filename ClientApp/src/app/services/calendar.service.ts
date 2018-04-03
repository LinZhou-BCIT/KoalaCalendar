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

export class CalendarLite {
  calendarID: string;
  name: string;
}

export class CalendarGroups {
  owned: Calendar[];
  subbed: Calendar[];
}

@Injectable()
export class CalendarService {
  private site: string;

  constructor(private http: Http, private remoteService: RemoteConnectService) {
    this.site = "https://apiserver20180208041703.azurewebsites.net/api/calendarapi/";
  }

  getCalendarsForMonth(month: number, year: number): Observable<CalendarGroups>  {
    let key: string = year + "-" + month;
    let item = sessionStorage.getItem(key);
    if (item == null) {
      let userInfo: UserInfo = this.remoteService.getUserInfo();
      let token: string = userInfo.token;
      
      let params = new URLSearchParams();
      params.set('year', String(year));
      params.set('month', String(month));
      let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 
  
      headers.append( 'Authorization', 'Bearer ' + token)
      let options = new RequestOptions({
          headers: headers,
          search: params
      });
      // console.log(headers);
  
      let dataUrl = this.site + 'getcalendar';  
      return this.http.get(dataUrl, options)
          .map(this.extractCalendars)
          .catch(this.handleError);
    }
    else {
      return Observable.of(JSON.parse(item));
    }

  }

  subToCalendar(accessCode: string) : Observable<Comment[]>  {
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    let token: string = userInfo.token;
    
    let params = new URLSearchParams();
    params.set('accessCode', accessCode);
    let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 

    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers,
        search: params
    });
    // console.log(headers);

    let dataUrl = this.site + 'subToCalendar';  
    return this.http.get(dataUrl, options)
        .map(this.extractData)
        .catch(this.handleError);
  }

  unsubFromCalendar(id: string) : Observable<Comment[]> {
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    let token: string = userInfo.token;
    
    let params = new URLSearchParams();
    params.set('calendarID', id);
    let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 

    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers,
        search: params
    });
    // console.log(headers);

    let dataUrl = this.site + 'unsubFromCalendar';  
    return this.http.get(dataUrl, options)
        .map(this.extractData)
        .catch(this.handleError);
  }

  createCalendar(name: string) : Observable<Comment[]> {
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    // maybe check the role here? or leave it to the server
    let token: string = userInfo.token;
    
    let content = new URLSearchParams();
    content.set('calendarName', name);
    let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 

    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers
    });
    // console.log(headers);

    let dataUrl = this.site + 'createCalendar';  
    return this.http.post(dataUrl, content.toString(), options)
        .map(this.extractData)
        .catch(this.handleError);
  }

  deleteCalendar(id: string) : Observable<Comment[]> {
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    // maybe check the role here? or leave it to the server
    let token: string = userInfo.token;
    
    let params = new URLSearchParams();
    params.set('calendarID', id);
    let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 

    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers,
        search: params
    });
    // console.log(headers);

    let dataUrl = this.site + 'deleteCalendar';  
    return this.http.delete(dataUrl, options)
        .map(this.extractData)
        .catch(this.handleError);
  }

  // For events: check user with calendar owner?
  addEvent(event: Event) : Observable<Comment[]> {
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    let token: string = userInfo.token;
    
    let headers = new Headers({ 'Content-Type': 'application/json' }); 

    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers
    });
    // console.log(headers);

    let dataUrl = this.site + 'addEvent';  
    return this.http.post(dataUrl, event, options)
        .map(this.extractData)
        .catch(this.handleError);
  }

  updateEvent(event: Event) : Observable<Comment[]> {
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    let token: string = userInfo.token;
    
    let headers = new Headers({ 'Content-Type': 'application/json' }); 

    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers
    });
    // console.log(headers);

    let dataUrl = this.site + 'updateEvent';  
    return this.http.put(dataUrl, event, options)
        .map(this.extractData)
        .catch(this.handleError);
  }

  deleteEvent(id: string) : Observable<Comment[]> {
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    let token: string = userInfo.token;
    
    let params = new URLSearchParams();
    params.set('eventID', id);
    let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 

    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers,
        search: params
    });
    // console.log(headers);

    let dataUrl = this.site + 'deleteEvent';  
    return this.http.delete(dataUrl, options)
        .map(this.extractData)
        .catch(this.handleError);
  }

  private extractCalendars(res: Response) {
    let body = res.json();
    let key = body.month;
    let calendars = body.calendars;
    
    let ownedCalendarsLite : CalendarLite[] = calendars.owned.map(this.calendarToLite);
    let subbedCalendarsLite : CalendarLite[] = calendars.subbed.map(this.calendarToLite);
    sessionStorage.setItem('ownedCalendars', JSON.stringify(ownedCalendarsLite));
    sessionStorage.setItem('subbedCalendars', JSON.stringify(subbedCalendarsLite));
    sessionStorage.setItem(key, JSON.stringify(calendars));
    return calendars;
  }

  private calendarToLite(calendar: Calendar) {
    let calendarLite = new CalendarLite();
    calendarLite.calendarID = calendar.calendarID;
    calendarLite.name = calendar.name;
    return calendarLite;
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
