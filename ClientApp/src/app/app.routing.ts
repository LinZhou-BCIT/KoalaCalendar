import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CalendarCreateComponent } from './components/calendar-create/calendar-create.component'
import { CalendarListComponent } from './components/calendar-list/calendar-list.component'
import { CalendarSubComponent } from './components/calendar-sub/calendar-sub.component' 
import { EventCreateComponent } from './components/event-create/event-create.component'
import { AboutComponent } from './components/about/about.component'

const appRoutes: Routes = [
    { path: 'calendar', component: CalendarListComponent },
    { path: 'calendar/create', component: CalendarCreateComponent },
    { path: 'calendar/list', component: CalendarListComponent },
    { path: 'calendar/sub', component: CalendarSubComponent },
    { path: 'event/create', component: EventCreateComponent },
    { path: 'about', component: AboutComponent },
    { path: '', redirectTo: '/calendar', pathMatch: 'full' },
    { path: '**', redirectTo: '/calendar', pathMatch: 'full'}
];

export const AppRouter: ModuleWithProviders = RouterModule.forRoot(appRoutes);
