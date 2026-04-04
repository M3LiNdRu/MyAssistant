import { Component, Inject } from '@angular/core';

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Category } from '../category';

@Component({
    selector: 'app-categories-form',
    templateUrl: './categories-form.component.html',
    styleUrls: ['./categories-form.component.scss'],
    standalone: false
})
export class CategoriesFormComponent {

  constructor(
    public dialogRef: MatDialogRef<CategoriesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Category,
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
