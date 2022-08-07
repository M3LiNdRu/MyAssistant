import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TagsAutocompleteInputComponent } from './tags-autocomplete-input.component';

describe('TagsAutocompleteInputComponent', () => {
  let component: TagsAutocompleteInputComponent;
  let fixture: ComponentFixture<TagsAutocompleteInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TagsAutocompleteInputComponent ]
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
