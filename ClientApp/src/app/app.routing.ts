import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CalendarCreateComponent } from './components/calendar-create/calendar-create.component';
import { CalendarListComponent } from './components/calendar-list/calendar-list.component';
import { CalendarSubComponent } from './components/calendar-sub/calendar-sub.component'; 
import { EventCreateComponent } from './components/event-create/event-create.component';
import { AboutComponent } from './components/about/about.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ForgetPasswordComponent } from './components/forget-password/forget-password.component';
import { ProfileComponent } from './components/profile/profile.component';

const appRoutes: Routes = [
    { path: 'calendar', component: CalendarListComponent },
    { path: 'calendar/create', component: CalendarCreateComponent },
    { path: 'calendar/list', component: CalendarListComponent },
    { path: 'calendar/sub', component: CalendarSubComponent },
    { path: 'event/create', component: EventCreateComponent },
    { path: 'about', component: AboutComponent },
    { path: 'account/login', component: LoginComponent },
    { path: 'account/register', component: RegisterComponent },
    { path: 'account/forgetPassword', component: ForgetPasswordComponent },
    { path: 'account/profile', component: ProfileComponent },
    { path: '', redirectTo: '/calendar', pathMatch: 'full' },
    { path: '**', redirectTo: '/calendar', pathMatch: 'full'}
];

export const AppRouter: ModuleWithProviders = RouterModule.forRoot(appRoutes);
