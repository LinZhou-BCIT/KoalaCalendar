import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';
import { CalendarService, Calendar, Event} from '../../services/calendar.service';

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.css']
})
export class EventDetailComponent implements OnInit {

  event: Event;
  constructor(private location: Location,
    private router: Router, private route: ActivatedRoute,
    private calendarService: CalendarService) { }

  ngOnInit() {
    this.event = new Event();
    this.route.params.forEach((params: Params) => {
      let id = params['id'];
      this.calendarService.getEventById(id).subscribe(
        data => {
          this.event = data.event;
        },
        error => {
          console.log(error);
          this.location.back();
        }
      )
    });
  }

  update() {
    alert('this function does not work right now.');
  }
  delete() {

  }
}
