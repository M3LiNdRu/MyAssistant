import { Component, Output, EventEmitter } from '@angular/core';

import { AppointmentsService } from '../appointments.service';
import { Appointment } from '../appointment';

@Component({
    selector: 'app-appointments-form',
    templateUrl: './appointments-form.component.html',
    styleUrls: ['./appointments-form.component.scss'],
    standalone: false
})
export class AppointmentsFormComponent {

  @Output() public appointmentSavedEvent = new EventEmitter<boolean>();

  appointment: Appointment = {
    id: "",
    title: "",
    description: "",
    dateTime: new Date()
  }

  constructor(private appointmentsService: AppointmentsService) { }

  /** Bridges the native datetime-local input (which works with strings) and the Date-typed model */
  get dateTimeLocal(): string {
    return this.toDateTimeLocalString(this.appointment.dateTime);
  }

  set dateTimeLocal(value: string) {
    this.appointment.dateTime = value ? new Date(value) : new Date();
  }

  private toDateTimeLocalString(date: Date): string {
    const d = new Date(date);
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
  }

  add(): void {
    if (this.appointment && this.appointment.title) {
      this.appointmentsService.addAppointment(this.appointment)
        .subscribe(() => {
          console.log("Appointment Added");
          this.appointmentSavedEvent.emit(true);
        });
    }
  }
}
