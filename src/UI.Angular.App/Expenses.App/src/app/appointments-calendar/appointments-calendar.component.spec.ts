import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';

import { AppointmentsCalendarComponent } from './appointments-calendar.component';
import { AppointmentsService } from '../appointments.service';
import { Appointment } from '../appointment';
import { AppointmentDetailDialogComponent } from '../appointment-detail-dialog/appointment-detail-dialog.component';

describe('AppointmentsCalendarComponent', () => {
  let component: AppointmentsCalendarComponent;
  let fixture: ComponentFixture<AppointmentsCalendarComponent>;
  let mockAppointmentsService: jasmine.SpyObj<AppointmentsService>;
  let mockDialog: jasmine.SpyObj<MatDialog>;

  const appointments: Appointment[] = [
    { id: '1', title: 'Dentist', description: 'Checkup', dateTime: new Date(2026, 2, 15, 9, 0) },
    { id: '2', title: 'Doctor', description: '', dateTime: new Date(2026, 2, 15, 11, 0) }
  ];

  beforeEach(async () => {
    mockAppointmentsService = jasmine.createSpyObj('AppointmentsService', ['getMonthlyAppointmentsByYearAndMonth']);
    mockAppointmentsService.getMonthlyAppointmentsByYearAndMonth.and.returnValue(of(appointments));
    mockDialog = jasmine.createSpyObj('MatDialog', ['open']);

    await TestBed.configureTestingModule({
      declarations: [AppointmentsCalendarComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: AppointmentsService, useValue: mockAppointmentsService },
        { provide: MatDialog, useValue: mockDialog },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentsCalendarComponent);
    component = fixture.componentInstance;
    component.date = new Date(2026, 2, 1);
    component.ngOnChanges();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch appointments for the year/month derived from the date input', () => {
    expect(mockAppointmentsService.getMonthlyAppointmentsByYearAndMonth).toHaveBeenCalledWith(2026, 3);
  });

  it('should build a 6-week grid covering the full month', () => {
    expect(component.weeks.length).toBe(6);
    expect(component.weeks.every(week => week.length === 7)).toBeTrue();
  });

  it('should mark days outside the current month as leading/trailing days without appointments', () => {
    const otherMonthDays = component.weeks.flat().filter(day => !day.inCurrentMonth);
    expect(otherMonthDays.length).toBeGreaterThan(0);
    expect(otherMonthDays.every(day => day.appointments.length === 0)).toBeTrue();
  });

  it('should bucket appointments under their day of the month', () => {
    const day15 = component.weeks.flat().find(day => day.inCurrentMonth && day.date.getDate() === 15);
    expect(day15?.appointments.length).toBe(2);

    const day16 = component.weeks.flat().find(day => day.inCurrentMonth && day.date.getDate() === 16);
    expect(day16?.appointments.length).toBe(0);
  });

  it('should assign the same color to the same appointment across calls', () => {
    const first = component.colorFor(appointments[0]);
    const second = component.colorFor(appointments[0]);
    expect(first).toBe(second);
  });

  it('should highlight today only when the displayed month is the current month', () => {
    const today = new Date();
    component.date = new Date(today.getFullYear(), today.getMonth(), 1);
    component.ngOnChanges();

    const todayCell = component.weeks.flat().find(day => day.isToday);
    expect(todayCell).toBeTruthy();
    expect(todayCell?.date.getDate()).toBe(today.getDate());
  });

  it('should re-fetch appointments when the date input changes (month navigation)', () => {
    mockAppointmentsService.getMonthlyAppointmentsByYearAndMonth.calls.reset();

    component.date = new Date(2026, 3, 1);
    component.ngOnChanges();

    expect(mockAppointmentsService.getMonthlyAppointmentsByYearAndMonth).toHaveBeenCalledWith(2026, 4);
  });

  it('should open the appointment detail dialog when an appointment is clicked', () => {
    component.openAppointment(appointments[0]);

    expect(mockDialog.open).toHaveBeenCalledWith(AppointmentDetailDialogComponent, { data: appointments[0] });
  });

  it('showForm should reveal the appointment form', () => {
    expect(component.isFormVisible).toBeFalse();

    component.showForm();

    expect(component.isFormVisible).toBeTrue();
  });

  it('onAppointmentSaved should hide the form and refresh the calendar', () => {
    component.isFormVisible = true;
    mockAppointmentsService.getMonthlyAppointmentsByYearAndMonth.calls.reset();

    component.onAppointmentSaved();

    expect(component.isFormVisible).toBeFalse();
    expect(mockAppointmentsService.getMonthlyAppointmentsByYearAndMonth).toHaveBeenCalled();
  });
});
