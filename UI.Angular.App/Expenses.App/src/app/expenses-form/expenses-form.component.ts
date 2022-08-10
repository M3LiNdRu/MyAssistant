import {Component, OnInit} from '@angular/core';

import { CategoriesService } from '../categories.service';
import { ExpensesService } from '../expenses.service';

import { Category } from '../category';
import { Expense } from '../expense';


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
    private expenseService: ExpensesService) { 
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
      this.expenseService.addExpense(this.expense)
      .subscribe(() => console.log("Expense Added"));
    }
  }
}
