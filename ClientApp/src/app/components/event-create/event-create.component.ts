import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CalendarService, Calendar, Event} from '../../services/calendar.service';

@Component({
  selector: 'app-event-create',
  templateUrl: './event-create.component.html',
  styleUrls: ['./event-create.component.css']
})
export class EventCreateComponent implements OnInit {
  ownedCalendars: Calendar[];
  toAdd: Event;
  constructor(private router: Router, private calendarService: CalendarService) { }

  ngOnInit() {
    this.toAdd = new Event();
    this.calendarService.getOwnedCalendars().subscribe(
      data => {
        this.ownedCalendars = data.calendars;
        console.log(data);
      },
      error => {
        console.log(error);
      }
    );
  }

  create() {
    console.log(this.toAdd);
    this.calendarService.addEvent(this.toAdd).subscribe(
      result => {
        console.log(result);
        this.router.navigate(['/calendar']);
      },
      error => {
        alert(error);
      }
    )
  }

}
