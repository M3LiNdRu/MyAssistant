import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { AppointmentsFormComponent } from './appointments-form.component';
import { AppointmentsService } from '../appointments.service';

describe('AppointmentsFormComponent', () => {
  let component: AppointmentsFormComponent;
  let fixture: ComponentFixture<AppointmentsFormComponent>;
  let mockAppointmentsService: jasmine.SpyObj<AppointmentsService>;

  beforeEach(async () => {
    mockAppointmentsService = jasmine.createSpyObj('AppointmentsService', ['addAppointment']);
    mockAppointmentsService.addAppointment.and.returnValue(of(true));

    await TestBed.configureTestingModule({
      declarations: [AppointmentsFormComponent],
      providers: [
        { provide: AppointmentsService, useValue: mockAppointmentsService },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('add should submit the appointment and emit appointmentSavedEvent when title is set', () => {
    spyOn(component.appointmentSavedEvent, 'emit');
    component.appointment.title = 'Dentist';
    component.appointment.description = 'Checkup';

    component.add();

    expect(mockAppointmentsService.addAppointment).toHaveBeenCalledWith(component.appointment);
    expect(component.appointmentSavedEvent.emit).toHaveBeenCalledWith(true);
  });

  it('add should do nothing when title is empty', () => {
    spyOn(component.appointmentSavedEvent, 'emit');
    component.appointment.title = '';

    component.add();

    expect(mockAppointmentsService.addAppointment).not.toHaveBeenCalled();
    expect(component.appointmentSavedEvent.emit).not.toHaveBeenCalled();
  });

  it('dateTimeLocal getter should format the appointment date as a datetime-local string', () => {
    component.appointment.dateTime = new Date(2026, 3, 4, 9, 30);

    expect(component.dateTimeLocal).toBe('2026-04-04T09:30');
  });

  it('dateTimeLocal setter should parse a datetime-local string into a Date, preserving date and time', () => {
    component.dateTimeLocal = '2026-04-04T14:45';

    expect(component.appointment.dateTime.getFullYear()).toBe(2026);
    expect(component.appointment.dateTime.getMonth()).toBe(3);
    expect(component.appointment.dateTime.getDate()).toBe(4);
    expect(component.appointment.dateTime.getHours()).toBe(14);
    expect(component.appointment.dateTime.getMinutes()).toBe(45);
  });
});
