import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, FormGroup, FormControl, ReactiveFormsModule, Validators }    from '@angular/forms';
import { HttpModule} from '@angular/http';
import { RecaptchaModule, RECAPTCHA_SETTINGS } from 'ng-recaptcha';
import { RecaptchaFormsModule } from 'ng-recaptcha/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { AppNavbarComponent } from './components/app-navbar/app-navbar.component';
import { AboutComponent } from './components/about/about.component';

import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ForgetPasswordComponent } from './components/forget-password/forget-password.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component'

import { CalendarListComponent } from './components/calendar-list/calendar-list.component';
import { CalendarCreateComponent } from './components/calendar-create/calendar-create.component';
import { CalendarSubComponent } from './components/calendar-sub/calendar-sub.component';
import { EventCreateComponent } from './components/event-create/event-create.component';

import { AppRoutingModule } from './app-routing/app-routing.module';

import { RemoteConnectService } from './services/remote-connect.service'
import { AuthGuardService } from './services/auth-guard.service';
import { CalendarService } from './services/calendar.service';
import { EventListComponent } from './components/event-list/event-list.component'
import { CalendarModule } from 'angular-calendar';

@NgModule({
  declarations: [
    AppComponent,
    CalendarCreateComponent,
    CalendarListComponent,
    CalendarSubComponent,
    EventCreateComponent,
    AboutComponent,
    AppNavbarComponent,
    LoginComponent,
    RegisterComponent,
    ForgetPasswordComponent,
    ProfileComponent,
    LogoutComponent,
    ResetPasswordComponent,
    ChangePasswordComponent,
    EventListComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    RecaptchaModule.forRoot(),
    RecaptchaFormsModule,
    ReactiveFormsModule,
    CalendarModule.forRoot()
  ],
  providers: [
    RemoteConnectService,
    AuthGuardService,
    CalendarService,
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: { 
        siteKey: '6LfgsVAUAAAAABBh9wfepmS00ZzVDd_dVW-ymYqR',
      }
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
