import { Component, OnChanges, Input } from '@angular/core';

import { Expense } from '../expense';
import { ExpensesService } from '../expenses.service';

@Component({
  selector: 'app-expenses-list',
  templateUrl: './expenses-list.component.html',
  styleUrls: ['./expenses-list.component.scss']
})
export class ExpensesListComponent implements OnChanges {

  @Input() public date = new Date();
  expenses: Expense[] = [];

  constructor(private expensesService: ExpensesService) { }

  ngOnChanges(): void {
    this.getMonthlyExpenses(this.date);
  }

  getMonthlyExpenses(date: Date): void {
    this.expensesService.getMonthlyExpensesByYearAndMonth(this.date.getFullYear(), this.date.getMonth()+1)
    .subscribe(expenses => this.expenses = expenses);
  }

  getTotalCost(): number {
    return Math.round(this.expenses.map(t => t.amount).reduce((acc, value) => acc + value, 0));
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
