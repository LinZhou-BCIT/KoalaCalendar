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
  calendarName: string;
}

export class Calendar {
  calendarID: string;
  name: string;
  accessCode: string;
  ownerID: string;
  ownerEmail: string;
}

export class EventRequestModel {
  calendarIDs: string[];
  startTime: Date;
  endTime: Date;
}

export class CalendarCreateDto {
  name: string;
}

@Injectable()
export class CalendarService {
  private site: string;

  constructor(private http: Http, private remoteService: RemoteConnectService) {
    this.site = "https://apiserver20180208041703.azurewebsites.net/api/calendarapi/";
  }

  getCalendarById(id: string): Observable<any> {
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
      let dataUrl = this.site + 'getcalendar';  
      return this.http.get(dataUrl, options)
          .map(this.extractData)
          .catch(this.handleError);
  }

  getOwnedCalendars(): Observable<any> {
      let userInfo: UserInfo = this.remoteService.getUserInfo();
      let token: string = userInfo.token;
      let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 
    
      headers.append( 'Authorization', 'Bearer ' + token)
      let options = new RequestOptions({
          headers: headers
      });
      let dataUrl = this.site + 'getownedcalendars';  
      return this.http.get(dataUrl, options)
          .map(this.extractData)
          .catch(this.handleError);
  }

  getSubbedCalendars(): Observable<any> {
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    let token: string = userInfo.token;
    let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 
  
    headers.append( 'Authorization', 'Bearer ' + token)
    let options = new RequestOptions({
        headers: headers
    });
    let dataUrl = this.site + 'getsubbedcalendars';  
    return this.http.get(dataUrl, options)
        .map(this.extractData)
        .catch(this.handleError);
  }
  
  createCalendar(calendarToCreate: CalendarCreateDto) : Observable<Comment[]> {
      let userInfo: UserInfo = this.remoteService.getUserInfo();
      // maybe check the role here? or leave it to the server
      let token: string = userInfo.token;
      
      let headers = new Headers({ 'Content-Type': 'application/json' }); 
      headers.append( 'Authorization', 'Bearer ' + token)
      let options = new RequestOptions({
          headers: headers
      });

      let dataUrl = this.site + 'createCalendar';  
      return this.http.post(dataUrl, calendarToCreate, options)
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

  getEventsForTimeRange(request: EventRequestModel): Observable<any>  {
      let userInfo: UserInfo = this.remoteService.getUserInfo();
      let token: string = userInfo.token;

      let headers = new Headers({ 'Content-Type': 'application/json' }); 
  
      headers.append( 'Authorization', 'Bearer ' + token)
      let options = new RequestOptions({
          headers: headers
      });
  
      let dataUrl = this.site + 'GetEventsOfTimeRange';  
      return this.http.post(dataUrl, request, options)
          .map(this.extractData)
          .catch(this.handleError);
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

    let dataUrl = this.site + 'subscribetocalendar';  
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

    let dataUrl = this.site + 'unsubscribefromcalendar';  
    return this.http.get(dataUrl, options)
        .map(this.extractData)
        .catch(this.handleError);
  }

  
  // For events: check user with calendar owner?
  getEventById(id: string): Observable<any> {
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
    let dataUrl = this.site + 'geteventbyid';  
    return this.http.get(dataUrl, options)
        .map(this.extractData)
        .catch(this.handleError);
    }

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
