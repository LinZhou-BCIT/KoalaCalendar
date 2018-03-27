import { Injectable } from '@angular/core';

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
  private calendars: Calendar[];
  constructor() {
    this.site = "https://apiserver20180208041703.azurewebsites.net/api/calendarapi/";
  }

  getCalendarFromServer() {

  }

}
