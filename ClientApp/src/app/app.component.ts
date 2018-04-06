import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, 
  ChangeDetectionStrategy,
  ViewChild,
  TemplateRef } from '@angular/core';

import * as moment from 'moment';
import * as _ from 'lodash';
import { RouterModule, Routes, Router }  from '@angular/router';

export interface CalendarDate {
  mDate: moment.Moment;
  selected?: boolean;
  today?: boolean;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnChanges{
  title = 'app';

  public now: Date = new Date();

  currentDate = moment();
  dayNames = ['S', 'M', 'T', 'W', 'T', 'F', 'S'];
  weeks: CalendarDate[][] = [];
  sortedDates: CalendarDate[] = [];

  @Input() selectedDates: CalendarDate[] = [];
  @Output() onSelectDate = new EventEmitter<CalendarDate>();

  constructor(private _router: Router) {}
  
    ngOnInit(): void {
      this.generateCalendar();
    }

    ngOnChanges(changes: SimpleChanges): void {
      if (changes.selectedDates &&
          changes.selectedDates.currentValue &&
          changes.selectedDates.currentValue.length  > 1) {
        // sort on date changes for better performance when range checking
        this.sortedDates = _.sortBy(changes.selectedDates.currentValue, (m: CalendarDate) => m.mDate.valueOf());
        this.generateCalendar();
      }
    }

      // date checkers
  isToday(date: moment.Moment): boolean {
    return moment().isSame(moment(date), 'day');
  }

  isSelected(date: moment.Moment): boolean {
    return _.findIndex(this.selectedDates, (selectedDate) => {
      return moment(date).isSame(selectedDate.mDate, 'day');
    }) > -1;
  }

  isSelectedMonth(date: moment.Moment): boolean {
    return moment(date).isSame(this.currentDate, 'month');
  }

  //

  selectDate(date: CalendarDate): void {
    this.onSelectDate.emit(date);
    //////
    console.log(date);
    // day = date.mdate.getday()
    var year = date.mDate.get('year');
    var month = date.mDate.get('month');
    var day = date.mDate.get('date');
    // var monthName = date.mDate.get('month');
   
    // console.log(monthName);

    // console.log(year);
    // console.log(month);
    // console.log(day);
    this._router.navigate(['/event/list', year, month, day]);
    //get month
    // get yeae
    // this.router.navigate ...' /event-list/'+year+'/'+month+'/+'day
  }

  // actions from calendar
  prevMonth(): void {
    this.currentDate = moment(this.currentDate).subtract(1, 'months');
    this.generateCalendar();
  }

  nextMonth(): void {
    this.currentDate = moment(this.currentDate).add(1, 'months');
    this.generateCalendar();
  }

  firstMonth(): void {
    this.currentDate = moment(this.currentDate).startOf('year');
    this.generateCalendar();
  }

  lastMonth(): void {
    this.currentDate = moment(this.currentDate).endOf('year');
    this.generateCalendar();
  }

  prevYear(): void {
    this.currentDate = moment(this.currentDate).subtract(1, 'year');
    this.generateCalendar();
  }

  nextYear(): void {
    this.currentDate = moment(this.currentDate).add(1, 'year');
    this.generateCalendar();
  }

  // generate the calendar grid
  generateCalendar(): void {
    const dates = this.fillDates(this.currentDate);
    const weeks: CalendarDate[][] = [];
    while (dates.length > 0) {
      weeks.push(dates.splice(0, 7));
    }
    this.weeks = weeks;
  }

  fillDates(currentMoment: moment.Moment): CalendarDate[] {
    const firstOfMonth = moment(currentMoment).startOf('month').day();
    const firstDayOfGrid = moment(currentMoment).startOf('month').subtract(firstOfMonth, 'days');
    const start = firstDayOfGrid.date();
    return _.range(start, start + 42)
            .map((date: number): CalendarDate => {
              const d = moment(firstDayOfGrid).date(date);
              return {
                today: this.isToday(d),
                selected: this.isSelected(d),
                mDate: d,
              };
            });
  }
}

