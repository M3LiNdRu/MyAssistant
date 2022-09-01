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

  getTotalCost(): number {
    return this.expenses.map(t => t.amount).reduce((acc, value) => acc + value, 0);
  }

  columns = [
    {
      columnDef: 'name',
      header: 'Name',
      cell: (element: Expense) => `${element.name}`,
    },
    {
      columnDef: 'category',
      header: 'Category',
      cell: (element: Expense) => `${element.category}`,
    },
    {
      columnDef: 'value',
      header: 'Value',
      cell: (element: Expense) => `${element.amount} ${element.currency}`,
    },
  ];


  displayedColumns = this.columns.map(c => c.columnDef);

}
