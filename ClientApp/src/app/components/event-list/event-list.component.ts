import { Component, OnInit, ChangeDetectionStrategy, 
  Input,
  Output,
  EventEmitter,
  SimpleChanges,
  OnChanges  } from '@angular/core';
  import {
    getSeconds,
    getMinutes,
    getHours,
    getDate,
    getMonth,
    getYear,
    setSeconds,
    setMinutes,
    setHours,
    setDate,
    setMonth,
    setYear
  } from 'date-fns';
  import {
    NgbDateStruct,
    NgbTimeStruct
  } from '@ng-bootstrap/ng-bootstrap';
  import { ActivatedRoute, Params }       from '@angular/router';
import { AppComponent } from '../../app.component';
import { CalendarEvent } from 'angular-calendar';
import * as moment from 'moment';
import * as _ from 'lodash';

export interface CalendarDate {
  mDate: moment.Moment;
  selected?: boolean;
  today?: boolean;
}

@Component({
  selector: 'app-event-list',
  // changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})

export class EventListComponent implements OnInit {
  // eventID: string;
  // name: string;
  // startTime: string;
  // endTime: string;
  // calendarID: string;
  view: string = 'month';
  viewDate:Date = new Date();
  events: Event[] = [];
  
  clickedDate: Date;

  day: Date;
  month: Date;
  year: Date;

  calendarDates: CalendarDate[];
  date: CalendarDate[];

  constructor(private route: ActivatedRoute) { 
  }

  ngOnInit() {
    this.route.params.forEach((params: Params) => {
      let calendarYear = params['year'];
      this.year = calendarYear;

      let calendarMonth = params['month'];
      this.month = calendarMonth;

      let calendarDay = params['day'];
      this.day = calendarDay;

    })
  }



}
