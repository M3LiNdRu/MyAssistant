import {Component, OnInit} from '@angular/core';

import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';

import { CategoriesService } from '../categories.service';
import { ExpensesService } from '../expenses.service';

import { Category } from '../category';
import { Expense } from '../expense';
import { CategoriesFormComponent } from '../categories-form/categories-form.component';

const INGRESSOS = "Ingressos"

@Component({
  selector: 'app-expenses-form',
  templateUrl: './expenses-form.component.html',
  styleUrls: ['./expenses-form.component.scss']
})
export class ExpensesFormComponent implements OnInit {
  
  expense: Expense = {
    id: "",
    category: "",
    amount: 0,
    currency: "EUR",
    name: "",
    tags: [],
    timestamp: new Date()
  }

  categories: Category[] = [];

  constructor(private categoriesService: CategoriesService,
    private expenseService: ExpensesService,
    public dialog: MatDialog) { 
  }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.categoriesService.getCategories()
    .subscribe(categories => this.categories = categories);
  }

  add(): void {
    if (this.expense) {
      if (this.expense.category != INGRESSOS) 
        this.expense.amount *= -1;
      
      this.expenseService.addExpense(this.expense)
      .subscribe(() => console.log("Expense Added"));
    }
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(CategoriesFormComponent, {
      width: '250px',
      data: { name: "", description: "" }
    });

    dialogRef.afterClosed().subscribe((category: Category) => {
      if (category) {
        this.categoriesService.addCategory(category)
        .subscribe(() => {
          console.log("Category Added");
          this.categories.push(category);
        });
      }
    });
  }
}
