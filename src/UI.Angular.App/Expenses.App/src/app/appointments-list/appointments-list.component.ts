import { Component, OnInit } from '@angular/core';

import { Appointment } from '../appointment';
import { AppointmentsService } from '../appointments.service';

@Component({
    selector: 'app-appointments-list',
    templateUrl: './appointments-list.component.html',
    styleUrls: ['./appointments-list.component.scss'],
    standalone: false
})
export class AppointmentsListComponent implements OnInit {

  appointments: Appointment[] = [];
  isFormVisible = false;

  constructor(private appointmentsService: AppointmentsService) { }

  ngOnInit(): void {
    this.getAppointments();
  }

  getAppointments(): void {
    // The API already returns only future appointments, ordered by dateTime ascending
    this.appointmentsService.getAppointments()
      .subscribe(appointments => this.appointments = appointments);
  }

  showForm(): void {
    this.isFormVisible = true;
  }

  onAppointmentSaved(): void {
    this.isFormVisible = false;
    this.getAppointments();
  }

  columns = [
    {
      columnDef: 'title',
      header: 'Title',
      cell: (element: Appointment) => `${element.title}`,
    },
    {
      columnDef: 'description',
      header: 'Description',
      cell: (element: Appointment) => `${element.description ?? ''}`,
    },
    {
      columnDef: 'dateTime',
      header: 'Date',
      cell: (element: Appointment) => `${new Date(element.dateTime).toLocaleString()}`,
    },
  ];

  displayedColumns = this.columns.map(c => c.columnDef);

}
