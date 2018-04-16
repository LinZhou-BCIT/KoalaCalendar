import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes }  from '@angular/router';

import { CalendarCreateComponent } from '../components/calendar-create/calendar-create.component';
import { CalendarListComponent } from '../components/calendar-list/calendar-list.component';
import { CalendarSubComponent } from '../components/calendar-sub/calendar-sub.component'; 
import { EventCreateComponent } from '../components/event-create/event-create.component';
import { AboutComponent } from '../components/about/about.component';

import { LogoutComponent } from '../components/logout/logout.component';
import { LoginComponent } from '../components/login/login.component';
import { RegisterComponent } from '../components/register/register.component';
import { ResetPasswordComponent } from '../components/reset-password/reset-password.component';
import { ForgetPasswordComponent } from '../components/forget-password/forget-password.component';
import { ChangePasswordComponent } from '../components/change-password/change-password.component';
import { ProfileComponent } from '../components/profile/profile.component';
import { AuthGuardService } from '../services/auth-guard.service'
import { EventListComponent } from '../components/event-list/event-list.component';
import { EventDetailComponent } from '../components/event-detail/event-detail.component';
import { CalendarDetailComponent } from '../components/calendar-detail/calendar-detail.component';

const appRoutes: Routes = [
  { path: 'calendar', component: CalendarListComponent, canActivate: [AuthGuardService]  },
  { path: 'calendar/create', component: CalendarCreateComponent, canActivate: [AuthGuardService]},
  { path: 'calendar/list', component: CalendarListComponent, canActivate: [AuthGuardService]  },
  { path: 'calendar/:type/:id', component: CalendarDetailComponent, canActivate: [AuthGuardService] },
  { path: 'calendar/sub', component: CalendarSubComponent, canActivate: [AuthGuardService] },
  { path: 'event/create', component: EventCreateComponent, canActivate: [AuthGuardService] },
  { path: 'event/list/:year/:month/:day', component: EventListComponent, canActivate: [AuthGuardService] },
  { path: 'event/:id', component: EventDetailComponent, canActivate: [AuthGuardService] },
  { path: 'about', component: AboutComponent },
  { path: 'account/login', component: LoginComponent },
  { path: 'account/logout', component: LogoutComponent, canActivate: [AuthGuardService] },
  { path: 'account/register', component: RegisterComponent },
  { path: 'account/resetPassword', component: ResetPasswordComponent },
  { path: 'account/changePassword', component: ChangePasswordComponent, canActivate: [AuthGuardService]  },
  { path: 'account/forgetPassword', component: ForgetPasswordComponent },
  { path: 'account/profile', component: ProfileComponent, canActivate: [AuthGuardService] },
  { path: '', redirectTo: '/calendar', pathMatch: 'full' },
  { path: '**', redirectTo: '/calendar', pathMatch: 'full'},
  // { path: 'userlist', component: UserListComponent, canActivate: [AuthGuardService] },
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: false }
    )
  ],
  exports: [
    RouterModule
  ],
  declarations: []
})
export class AppRoutingModule { }
