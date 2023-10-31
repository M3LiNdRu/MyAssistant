import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatLegacyChipInputEvent as MatChipInputEvent } from '@angular/material/legacy-chips';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

import { TagsService } from '../tags.service';

@Component({
  selector: 'app-tags-autocomplete-input',
  templateUrl: './tags-autocomplete-input.component.html',
  styleUrls: ['./tags-autocomplete-input.component.scss']
})
export class TagsAutocompleteInputComponent implements OnInit {


  separatorKeysCodes: number[] = [ENTER, COMMA];
  tagCtrl = new FormControl('');
  filteredTags: Observable<string[]>;

  @Input()
  tags: string[] = [];

  allTags: string[] = [];

  @ViewChild('tagInput') fruitInput: ElementRef<HTMLInputElement> = {} as ElementRef;

  @Output()
  tagsChange = new EventEmitter<string[]>();

  constructor(private tagsService: TagsService ) { 
    this.filteredTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((fruit: string | null) => (fruit ? this._filter(fruit) : this.allTags.slice())),
    );
  }

  ngOnInit(): void {
    this.getAllTags();
  }

  getAllTags(): void {
    this.tagsService.getTags()
    .subscribe(allTags => this.allTags = allTags.tags);
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our fruit
    if (value) {
      this.tags.push(value);
    }

    // Clear the input value
    event.chipInput!.clear();

    this.tagCtrl.setValue(null);
    this.tagsChange.emit(this.tags);
  }

  remove(fruit: string): void {
    const index = this.tags.indexOf(fruit);

    if (index >= 0) {
      this.tags.splice(index, 1);
    }

    this.tagsChange.emit(this.tags);

  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.tags.push(event.option.viewValue);
    this.fruitInput.nativeElement.value = '';
    this.tagCtrl.setValue(null);
    this.tagsChange.emit(this.tags);
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.allTags.filter(tag => tag.toLowerCase().includes(filterValue));
  }


}
