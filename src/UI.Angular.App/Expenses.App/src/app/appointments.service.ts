import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { environment } from '../environments/environment';

import { Appointment } from './appointment'

@Injectable({
  providedIn: 'root'
})
export class AppointmentsService {
  private appointmentUrl = environment.apiUrl + 'api/v1/appointment';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  /** GET upcoming appointments from the server, ordered by dateTime ascending */
  getAppointments(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(environment.apiUrl + 'api/v1/appointments');
  }

  addAppointment(appointment: Appointment): Observable<boolean> {
    return this.http.post<Appointment>(this.appointmentUrl, appointment, this.httpOptions).pipe(
      tap(_ => console.log(`added appointment "${appointment.title}"`)),
      catchError(this.handleError<any>('addAppointment'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}
