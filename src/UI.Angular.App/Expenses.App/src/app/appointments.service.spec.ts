import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { AppointmentsService } from './appointments.service';
import { Appointment } from './appointment';
import { environment } from '../environments/environment';

describe('AppointmentsService', () => {
  let service: AppointmentsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(AppointmentsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getAppointments should GET the upcoming appointments endpoint', () => {
    const appointments: Appointment[] = [
      { id: '1', title: 'Dentist', description: '', dateTime: new Date() }
    ];

    service.getAppointments().subscribe(result => {
      expect(result).toEqual(appointments);
    });

    const req = httpMock.expectOne(environment.apiUrl + 'api/v1/appointments');
    expect(req.request.method).toBe('GET');
    req.flush(appointments);
  });

  it('addAppointment should POST to the appointment endpoint', () => {
    const appointment: Appointment = { id: '', title: 'Dentist', description: 'Checkup', dateTime: new Date() };

    service.addAppointment(appointment).subscribe(result => {
      expect(result).toBeTruthy();
    });

    const req = httpMock.expectOne(environment.apiUrl + 'api/v1/appointment');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(appointment);
    req.flush(appointment);
  });

  it('addAppointment should swallow errors and return undefined', () => {
    const appointment: Appointment = { id: '', title: 'Dentist', description: '', dateTime: new Date() };

    service.addAppointment(appointment).subscribe(result => {
      expect(result).toBeUndefined();
    });

    const req = httpMock.expectOne(environment.apiUrl + 'api/v1/appointment');
    req.error(new ProgressEvent('error'));
  });
});
