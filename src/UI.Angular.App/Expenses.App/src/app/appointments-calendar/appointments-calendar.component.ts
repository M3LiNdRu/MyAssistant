import { Component, Input, OnChanges } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { Appointment } from '../appointment';
import { AppointmentsService } from '../appointments.service';
import { AppointmentDetailDialogComponent } from '../appointment-detail-dialog/appointment-detail-dialog.component';

export interface CalendarDay {
  date: Date;
  inCurrentMonth: boolean;
  isToday: boolean;
  appointments: Appointment[];
}

const DOT_COLORS = ['#e57373', '#64b5f6', '#81c784', '#ffb74d', '#ba68c8', '#4db6ac', '#f06292', '#a1887f'];

@Component({
    selector: 'app-appointments-calendar',
    templateUrl: './appointments-calendar.component.html',
    styleUrls: ['./appointments-calendar.component.scss'],
    standalone: false
})
export class AppointmentsCalendarComponent implements OnChanges {

  @Input() public date = new Date();

  appointments: Appointment[] = [];
  isFormVisible = false;
  weeks: CalendarDay[][] = [];
  readonly weekdayLabels = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

  constructor(private appointmentsService: AppointmentsService, private dialog: MatDialog) { }

  ngOnChanges(): void {
    this.getMonthlyAppointments();
  }

  getMonthlyAppointments(): void {
    this.appointmentsService.getMonthlyAppointmentsByYearAndMonth(this.date.getFullYear(), this.date.getMonth() + 1)
      .subscribe(appointments => {
        this.appointments = appointments;
        this.weeks = this.buildCalendarWeeks(this.date, appointments);
      });
  }

  showForm(): void {
    this.isFormVisible = true;
  }

  onAppointmentSaved(): void {
    this.isFormVisible = false;
    this.getMonthlyAppointments();
  }

  openAppointment(appointment: Appointment): void {
    this.dialog.open(AppointmentDetailDialogComponent, { data: appointment });
  }

  colorFor(appointment: Appointment): string {
    const hash = (appointment.id ?? appointment.title)
      .split('')
      .reduce((acc, char) => acc + char.charCodeAt(0), 0);
    return DOT_COLORS[hash % DOT_COLORS.length];
  }

  private buildCalendarWeeks(date: Date, appointments: Appointment[]): CalendarDay[][] {
    const year = date.getFullYear();
    const month = date.getMonth();
    const today = new Date();
    const isSameDay = (a: Date, b: Date) =>
      a.getFullYear() === b.getFullYear() && a.getMonth() === b.getMonth() && a.getDate() === b.getDate();

    const appointmentsByDay = new Map<number, Appointment[]>();
    for (const appointment of appointments) {
      const appointmentDate = new Date(appointment.dateTime);
      if (appointmentDate.getFullYear() === year && appointmentDate.getMonth() === month) {
        const day = appointmentDate.getDate();
        const bucket = appointmentsByDay.get(day) ?? [];
        bucket.push(appointment);
        appointmentsByDay.set(day, bucket);
      }
    }

    const firstOfMonth = new Date(year, month, 1);
    // getDay(): 0=Sunday..6=Saturday; shift so the grid starts on Monday.
    const leadingDays = (firstOfMonth.getDay() + 6) % 7;
    const gridStart = new Date(year, month, 1 - leadingDays);

    const days: CalendarDay[] = [];
    for (let i = 0; i < 42; i++) {
      const cellDate = new Date(gridStart.getFullYear(), gridStart.getMonth(), gridStart.getDate() + i);
      const inCurrentMonth = cellDate.getMonth() === month && cellDate.getFullYear() === year;
      days.push({
        date: cellDate,
        inCurrentMonth,
        isToday: isSameDay(cellDate, today),
        appointments: inCurrentMonth ? (appointmentsByDay.get(cellDate.getDate()) ?? []) : [],
      });
    }

    const weeks: CalendarDay[][] = [];
    for (let i = 0; i < days.length; i += 7) {
      weeks.push(days.slice(i, i + 7));
    }
    return weeks;
  }
}
