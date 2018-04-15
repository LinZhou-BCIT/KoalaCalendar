import { Component, OnInit } from '@angular/core';
import { RemoteConnectService, UserInfo} from '../../services/remote-connect.service';
import { CalendarService, Calendar} from '../../services/calendar.service';

@Component({
  selector: 'app-calendar-list',
  templateUrl: './calendar-list.component.html',
  styleUrls: ['./calendar-list.component.css']
})
export class CalendarListComponent implements OnInit {
  ownedCalendars: Calendar[];
  subbedCalendars: Calendar[];
  isProf: boolean = false;

  constructor(private remoteService: RemoteConnectService, 
    private calendarService: CalendarService) { }

  ngOnInit() {
      let userInfo: UserInfo = this.remoteService.getUserInfo();
      let userRole = userInfo.role;
      if (userRole == 'PROFESSOR') {
        this.isProf = true;
      }
      this.calendarService.getOwnedCalendars().subscribe(
        data => {
          this.ownedCalendars = data.calendars;
          console.log(data);
        },
        error => {
          console.log(error);
        }
      );
      this.calendarService.getSubbedCalendars().subscribe(
        data => {
          this.subbedCalendars = data.calendars;
          console.log(data);
        },
        error => {
          console.log(error);
        }
      );
  }

}
