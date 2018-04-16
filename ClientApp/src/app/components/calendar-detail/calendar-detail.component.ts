import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CalendarService, Calendar} from '../../services/calendar.service';

@Component({
  selector: 'app-calendar-detail',
  templateUrl: './calendar-detail.component.html',
  styleUrls: ['./calendar-detail.component.css']
})
export class CalendarDetailComponent implements OnInit {
  id: string;
  owned: boolean = false;
  calendar: Calendar;

  constructor(private router: Router,
    private route: ActivatedRoute,
    private calendarService: CalendarService) { }

  ngOnInit() {
    this.route.params.forEach((params: Params) => {
      let calType = params['type'];
      if (calType == 'own') {
        this.owned = true;
      }
      this.id = params['id'];
      this.calendarService.getCalendarById(this.id).subscribe(
        data => {
          this.calendar = data.calendar;
          console.log(this.calendar);
        },
        error => {
          console.log(error);
          this.router.navigate(['/calendar']);
        }
      )
    });
  }

  update(){

  }

  unsub(){
    if (confirm("Are you sure you want to unsubscribe from this calendar?")){
      this.calendarService.unsubFromCalendar(this.id).subscribe(
        data => {
          console.log(data["message"]);
          this.router.navigate(['/calendar']);
        },
        error => {
          alert(error);
          console.log(error);
        }
      );
    }
  }

  delete(){
    if (confirm("Are you sure you want to delete this calendar?")){
      this.calendarService.deleteCalendar(this.id).subscribe(
        data => {
          console.log(data["message"]);
          this.router.navigate(['/calendar']);
        },
        error => {
          alert(error);
          console.log(error);
        }
      );
    }
  }

}
