import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CalendarService, Event, EventRequestModel } from '../../services/calendar.service';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})

export class EventListComponent implements OnInit {
  id: string;
  date: Date;
  events: Event[];

  constructor(private router: Router,
    private route: ActivatedRoute,
    private calendarService: CalendarService) { 
  }

  ngOnInit() {
    this.route.params.forEach((params: Params) => {
      let calendarYear = params['year'];
      let calendarMonth = params['month'];
      let calendarDay = params['day'];
      this.date = new Date(calendarYear, calendarMonth, calendarDay);
    });
    let nextDay = new Date(this.date);
      nextDay.setDate(nextDay.getDate() + 1);
      let request: EventRequestModel = {
        // catch all
        calendarIDs: [],
        startTime: this.date,
        endTime: nextDay
      }
      this.calendarService.getEventsForTimeRange(request).subscribe(
        data => {
          this.events = data.events.map((event) => {
            // timezone fix
            event.startTime = event.startTime + 'Z';
            event.endTime = event.endTime + 'Z';
            return event;
          });
          console.log(this.events);
        },
        error => {
          alert(error);
        }
      );
  }

}
