import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarSubComponent } from './calendar-sub.component';

describe('CalendarSubComponent', () => {
  let component: CalendarSubComponent;
  let fixture: ComponentFixture<CalendarSubComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarSubComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarSubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
