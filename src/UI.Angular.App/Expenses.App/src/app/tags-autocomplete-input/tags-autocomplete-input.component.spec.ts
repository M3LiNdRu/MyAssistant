import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatChipsModule } from '@angular/material/chips';

import { TagsAutocompleteInputComponent } from './tags-autocomplete-input.component';

describe('TagsAutocompleteInputComponent', () => {
  let component: TagsAutocompleteInputComponent;
  let fixture: ComponentFixture<TagsAutocompleteInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule,
        HttpClientTestingModule,
        MatAutocompleteModule,
        MatInputModule,
        MatFormFieldModule,
        MatChipsModule,
      ],
      declarations: [ TagsAutocompleteInputComponent ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(TagsAutocompleteInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
