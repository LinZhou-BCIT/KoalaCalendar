import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';
import { RemoteConnectService, UserInfo } from '../../services/remote-connect.service';
import { CalendarService, Calendar, Event} from '../../services/calendar.service';

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.css']
})
export class EventDetailComponent implements OnInit {
  event: Event;
  owned: boolean = false;

  constructor(private location: Location,
    private router: Router, private route: ActivatedRoute,
    private calendarService: CalendarService,
    private remoteService: RemoteConnectService) { }

  ngOnInit() {
    this.event = new Event();
    this.route.params.forEach((params: Params) => {
      let id = params['id'];
      this.calendarService.getEventById(id).subscribe(
        data => {
          var event = data.event;
          // timezone fix
          event.startTime = event.startTime + 'Z';
          event.endTime = event.endTime + 'Z';
          this.event = event;
          this.checkOwner();
        },
        error => {
          console.log(error);
          this.location.back();
        }
      )
    });
    
  }

  checkOwner(){
    // there should be better ways to check ownership but this'll do for now
    let userInfo: UserInfo = this.remoteService.getUserInfo();
    let email: string = userInfo.email;
    console.log(this.event);
    this.calendarService.getCalendarById(this.event.calendarID).subscribe(
      data => {
        if (data.calendar.ownerEmail == email) {
          this.owned = true;
        }
      },
      error => {
        console.log(error);
        this.location.back();
      }
    )
  }

  update() {
    alert('this function does not work right now.');
  }
  
  delete() {
    if (confirm("Are you sure you want to delete this event?")){
      this.calendarService.deleteEvent(this.event.eventID).subscribe(
        data => {
          console.log(data["message"]);
          this.location.back();
        },
        error => {
          alert(error);
          console.log(error);
        }
      );
    }
  }
}
