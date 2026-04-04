import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { of } from 'rxjs';

import { ExpensesFormComponent } from './expenses-form.component';
import { TagsAutocompleteInputComponent } from '../tags-autocomplete-input/tags-autocomplete-input.component';
import { CategoriesService } from '../categories.service';
import { ExpensesService } from '../expenses.service';
import { TagsService } from '../tags.service';

describe('ExpensesFormComponent', () => {
  let component: ExpensesFormComponent;
  let fixture: ComponentFixture<ExpensesFormComponent>;
  let mockCategoriesService: jasmine.SpyObj<CategoriesService>;
  let mockExpensesService: jasmine.SpyObj<ExpensesService>;
  let mockTagsService: jasmine.SpyObj<TagsService>;

  beforeEach(async () => {
    mockCategoriesService = jasmine.createSpyObj('CategoriesService', ['getCategories', 'addCategory']);
    mockExpensesService = jasmine.createSpyObj('ExpensesService', ['addExpense']);
    mockTagsService = jasmine.createSpyObj('TagsService', ['getTags']);

    mockCategoriesService.getCategories.and.returnValue(of([]));
    mockExpensesService.addExpense.and.returnValue(of(true));
    mockTagsService.getTags.and.returnValue(of({ tags: [] }));

    await TestBed.configureTestingModule({
      declarations: [ExpensesFormComponent, TagsAutocompleteInputComponent],
      imports: [
        FormsModule,
        ReactiveFormsModule,
        NoopAnimationsModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatCardModule,
        MatIconModule,
        MatChipsModule,
        MatAutocompleteModule,
      ],
      providers: [
        { provide: CategoriesService, useValue: mockCategoriesService },
        { provide: ExpensesService, useValue: mockExpensesService },
        { provide: TagsService, useValue: mockTagsService },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ExpensesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set timestamp hours to noon before submitting to prevent UTC date shift', () => {
    // Simulate a datepicker selection: midnight local time
    const selectedDate = new Date(2026, 3, 4, 0, 0, 0, 0); // April 4, 2026 00:00:00
    component.expense.timestamp = selectedDate;
    component.expense.category = 'Ingressos';
    component.expense.amount = 100;

    component.add();

    const submitted = mockExpensesService.addExpense.calls.mostRecent().args[0];
    expect(submitted.timestamp.getFullYear()).toBe(2026);
    expect(submitted.timestamp.getMonth()).toBe(3); // April (0-indexed)
    expect(submitted.timestamp.getDate()).toBe(4);
    expect(submitted.timestamp.getHours()).toBe(12);
    expect(submitted.timestamp.getMinutes()).toBe(0);
    expect(submitted.timestamp.getSeconds()).toBe(0);

    // Verify the date survives UTC conversion (the actual bug scenario)
    const isoString = submitted.timestamp.toISOString();
    expect(isoString).toContain('2026-04-04');
  });
});
