import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})

export class EventListComponent implements OnInit {
  year: number;
  month: number;
  day: number;

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
