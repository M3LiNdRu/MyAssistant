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
  list: any[] = [];

  constructor(private summariesService: SummariesService,
    private expensesService: ExpensesService) 
  {
    this.myType = ChartType.PieChart;
  }

  ngOnInit(): void {
    this.getSummary();
    this.getMonthlyExpenses();
  }

  getMonthlyExpenses(): void {
    this.expensesService.getMonthlyExpensesByYearAndMonth(2022, 9)
    .subscribe(expenses => this.expenses = expenses);
  }

  getSummary(): void {
    this.summariesService.getMonthlySummary(2022, 9)
    .subscribe(summary => {
      this.summary = summary;
      this.list = Object.entries(summary.spentByCategory).map(([key, value]) => ({key: key, value: value})).sort((n1,n2) => n2.value - n1.value);
      this.populateChart();
    });
  }

  getTotalCost(): number {
    return Math.round(this.expenses.map(t => t.amount).reduce((acc, value) => acc + value, 0));
  }

  populateChart(): void {
    if (Object.keys(this.summary.progressBar).length > 0) {
      this.columns = Object.keys(this.summary.progressBar);
      this.myData = Object.entries(this.summary.progressBar);
    }
  }


  table_columns = [
    {
      columnDef: 'category',
      header: 'Category',
      cell: (element: any) => `${element.key}`,
    },
    {
      columnDef: 'value',
      header: 'Value',
      cell: (element: any) => `${element.value} €`,
    },
  ];


  displayedColumns = this.table_columns.map(c => c.columnDef);
}
