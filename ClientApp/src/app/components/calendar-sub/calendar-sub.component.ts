import { Component, OnInit } from '@angular/core';
import { CalendarService } from '../../services/calendar.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-calendar-sub',
  templateUrl: './calendar-sub.component.html',
  styleUrls: ['./calendar-sub.component.css']
})
export class CalendarSubComponent implements OnInit {

  private accessCode: string;
  constructor(private router: Router, private calendarService: CalendarService) { }

  ngOnInit() {
  }

  subscribe() {
    this.calendarService.subToCalendar(this.accessCode).subscribe(
      result => {
        if(result['success']) {
          // process the message from server
          this.router.navigate(['/calendar']);
        } else {
          // process and display the message from server
        }
      },
      error => {
        // process and display the message from server?
        alert(error);
      }
    );
  }

}
