import { Component, OnInit } from '@angular/core';

import { Expense } from '../expense';
import { ExpensesService } from '../expenses.service';

@Component({
  selector: 'app-expenses-list',
  templateUrl: './expenses-list.component.html',
  styleUrls: ['./expenses-list.component.scss']
})
export class ExpensesListComponent implements OnInit {

  expenses: Expense[] = [];

  constructor(private expensesService: ExpensesService) { }

  ngOnInit(): void {
    this.getExpenses();
  }

  getExpenses(): void {
    this.expensesService.getExpenses()
    .subscribe(expenses => this.expenses = expenses);
  }

}
