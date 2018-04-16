import { Component, OnInit } from '@angular/core';
import { CalendarService, CalendarCreateDto } from '../../services/calendar.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-calendar-create',
  templateUrl: './calendar-create.component.html',
  styleUrls: ['./calendar-create.component.css']
})
export class CalendarCreateComponent implements OnInit {

  name: string;
  constructor(private router: Router, private calendarService: CalendarService) { }

  ngOnInit() {
  }

  create() {
    let calendarToCreate: CalendarCreateDto = {
      name: this.name
    }
    this.calendarService.createCalendar(calendarToCreate).subscribe(
      result => {
        // process the message from server
        this.router.navigate(['/calendar']);
      },
      error => {
        // process and display the message from server?
        alert(error);
      }
    );
  }
}
