import { Component, OnInit } from '@angular/core';
import { MonthlySummary } from '../monthlySummary';
import { ChartType, Column } from 'angular-google-charts';
import { SummariesService } from '../summaries.service';
import { ExpensesService } from '../expenses.service';
import { Expense } from '../expense';

@Component({
  selector: 'app-month-summary',
  templateUrl: './month-summary.component.html',
  styleUrls: ['./month-summary.component.scss']
})
export class MonthSummaryComponent implements OnInit {

  summary: MonthlySummary = {
    month: "",
    year: "",
    saved: 0,
    start: 0,
    progressBar: {},
    spentByCategory: {}
  };

  expenses: Expense[] = [];
  columns: Column[] = ['Categoria', '%'];
  myData: any[][] = [['Buit', 100]];
  myType: ChartType;

  constructor(private summariesService: SummariesService,
    private expensesService: ExpensesService) 
  {
    this.myType = ChartType.PieChart;
  }

  ngOnInit(): void {
    this.getSummary();
    this.populateChart();
    this.getMonthlyExpenses();
  }

  getMonthlyExpenses(): void {
    this.expensesService.getMonthlyExpensesByYearAndMonth(2022, 9)
    .subscribe(expenses => this.expenses = expenses);
  }

  getSummary(): void {
    this.summariesService.getMonthlySummary(2022, 9)
    .subscribe(summary => this.summary = summary);
  }

  populateChart(): void {
    if (Object.keys(this.summary.spentByCategory).length > 0) {
      this.columns = Object.keys(this.summary.spentByCategory);
      this.myData = Object.entries(this.summary.spentByCategory);
    }
  }

  table_columns = [
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


  displayedColumns = this.table_columns.map(c => c.columnDef);
}
