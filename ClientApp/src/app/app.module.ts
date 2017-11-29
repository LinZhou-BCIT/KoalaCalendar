import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { CalendarCreateComponent } from './components/calendar-create/calendar-create.component';
import { CalendarListComponent } from './components/calendar-list/calendar-list.component';
import { CalendarSubComponent } from './components/calendar-sub/calendar-sub.component';
import { EventCreateComponent } from './components/event-create/event-create.component';


@NgModule({
  declarations: [
    AppComponent,
    CalendarCreateComponent,
    CalendarListComponent,
    CalendarSubComponent,
    EventCreateComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
