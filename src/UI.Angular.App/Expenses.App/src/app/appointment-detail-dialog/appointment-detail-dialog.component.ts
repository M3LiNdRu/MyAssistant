import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { Appointment } from '../appointment';

@Component({
    selector: 'app-appointment-detail-dialog',
    templateUrl: './appointment-detail-dialog.component.html',
    styleUrls: ['./appointment-detail-dialog.component.scss'],
    standalone: false
})
export class AppointmentDetailDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<AppointmentDetailDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public appointment: Appointment
  ) { }

  close(): void {
    this.dialogRef.close();
  }
}
