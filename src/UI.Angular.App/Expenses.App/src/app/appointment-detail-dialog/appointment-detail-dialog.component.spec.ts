import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

import { AppointmentDetailDialogComponent } from './appointment-detail-dialog.component';
import { Appointment } from '../appointment';

describe('AppointmentDetailDialogComponent', () => {
  let component: AppointmentDetailDialogComponent;
  let fixture: ComponentFixture<AppointmentDetailDialogComponent>;
  let mockDialogRef: jasmine.SpyObj<MatDialogRef<AppointmentDetailDialogComponent>>;

  const appointment: Appointment = {
    id: '1',
    title: 'Dentist',
    description: 'Checkup',
    dateTime: new Date(2026, 2, 15, 9, 0)
  };

  beforeEach(async () => {
    mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [MatDialogModule, MatButtonModule],
      declarations: [AppointmentDetailDialogComponent],
      providers: [
        { provide: MatDialogRef, useValue: mockDialogRef },
        { provide: MAT_DIALOG_DATA, useValue: appointment },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppointmentDetailDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render title, description and date/time from the injected appointment', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Dentist');
    expect(compiled.textContent).toContain('Checkup');
  });

  it('close should close the dialog', () => {
    component.close();

    expect(mockDialogRef.close).toHaveBeenCalled();
  });
});
