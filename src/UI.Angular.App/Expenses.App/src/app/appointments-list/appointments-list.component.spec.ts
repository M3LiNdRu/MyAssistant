import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { of } from 'rxjs';

import { AppointmentsListComponent } from './appointments-list.component';
import { AppointmentsService } from '../appointments.service';
import { Appointment } from '../appointment';

describe('AppointmentsListComponent', () => {
  let component: AppointmentsListComponent;
  let fixture: ComponentFixture<AppointmentsListComponent>;
  let mockAppointmentsService: jasmine.SpyObj<AppointmentsService>;

  const appointments: Appointment[] = [
    { id: '1', title: 'Dentist', description: 'Checkup', dateTime: new Date(2026, 0, 1) },
    { id: '2', title: 'Doctor', description: '', dateTime: new Date(2026, 0, 2) }
  ];

  beforeEach(async () => {
    mockAppointmentsService = jasmine.createSpyObj('AppointmentsService', ['getAppointments']);
    mockAppointmentsService.getAppointments.and.returnValue(of(appointments));

    await TestBed.configureTestingModule({
      declarations: [AppointmentsListComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: AppointmentsService, useValue: mockAppointmentsService },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load the upcoming appointments on init', () => {
    expect(mockAppointmentsService.getAppointments).toHaveBeenCalled();
    expect(component.appointments).toEqual(appointments);
  });

  it('showForm should reveal the appointment form', () => {
    expect(component.isFormVisible).toBeFalse();

    component.showForm();

    expect(component.isFormVisible).toBeTrue();
  });

  it('onAppointmentSaved should hide the form and refresh the list', () => {
    component.isFormVisible = true;
    mockAppointmentsService.getAppointments.calls.reset();

    component.onAppointmentSaved();

    expect(component.isFormVisible).toBeFalse();
    expect(mockAppointmentsService.getAppointments).toHaveBeenCalled();
  });
});
