import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { MonthlySummary } from '../monthlySummary';
import { ChartType, Column, Row } from 'angular-google-charts';
import { SummariesService } from '../summaries.service';
import { ExpensesService } from '../expenses.service';
import { Expense } from '../expense';
import { HistorigramService } from '../historigram.service';
import { Historigram } from '../historigram';

@Component({
    selector: 'app-month-summary',
    templateUrl: './month-summary.component.html',
    styleUrls: ['./month-summary.component.scss'],
    standalone: false
})
export class MonthSummaryComponent implements OnChanges {

  @Input() public date = new Date();
  
  summary: MonthlySummary;
  historigram: Historigram;
  expenses: Expense[] = [];
  summaryColumns: Column[] = ['Categoria', '%'];
  historigramColumns = ['Month', 'saved', 'spent', 'earned'];

  historigramData: any[][] = [];
  historigramType: ChartType;

  summaryData: any[][] = [['Buit', 100]];
  summaryType: ChartType;
  
  list: any[] = [];

  constructor(private summariesService: SummariesService,
    private expensesService: ExpensesService,
    private historigramService: HistorigramService) 
  {
    this.summaryType = ChartType.PieChart;
    this.historigramType = ChartType.LineChart;
    
    this.summary = {
      month: "",
      year: "",
      saved: 0,
      start: 0,
      progressBar: {},
      spentByCategory: {}
    };

    this.historigram = {
      totalEarned: 0,
      totalSavings: 0,
      totalSpent: 0,
      progressLine: {}
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log("ngOnChanges::Getting expenses from date: " + this.date);
    this.getSummary();
    this.getMonthlyExpenses();
    this.getHistorigram();
  }

  getHistorigram(): void {
    this.historigramService.getHistorigram()
    .subscribe(historigram => {
      this.historigram = historigram;
      this.resetHistorigramChart();
      this.populateHistorigramChart();
    });
  }

  getMonthlyExpenses(): void {
    this.expensesService.getMonthlyExpensesByYearAndMonth(this.date.getFullYear(), this.date.getMonth()+1)
    .subscribe(expenses => this.expenses = expenses);
  }

  getSummary(): void {
    this.summariesService.getMonthlySummary(this.date.getFullYear(), this.date.getMonth()+1)
    .subscribe(summary => {
      this.summary = summary;
      this.list = Object.entries(summary.spentByCategory).map(([key, value]) => ({key: key, value: value})).sort((n1,n2) => n2.value - n1.value);
      this.resetSummaryChart();
      this.populateSummaryChart();
    });
  }
  
  resetSummaryChart(): void {
    this.summaryData = [['Buit', 100]];
  }

  resetHistorigramChart(): void {
    this.historigramData = [[]];
  }

  getTotalCost(): number {
    return Math.round(this.expenses.filter(e => e.amount < 0).map(t => t.amount).reduce((acc, value) => acc + value, 0));
  }

  populateSummaryChart(): void {
    if (Object.keys(this.summary.progressBar).length > 0) {
      this.summaryColumns = Object.keys(this.summary.progressBar);
      this.summaryData = Object.entries(this.summary.progressBar);
    }
  }

  populateHistorigramChart(): void {
    if (Object.keys(this.historigram.progressLine).length > 0) {
      this.historigramData = Object.keys(this.historigram.progressLine)
                          .map((key) => ({key: key, value: this.historigram.progressLine[key]}))
                          .map((value) => ([value.key.substring(5,7), value.value.saved, value.value.spent, value.value.earned]));
      console.log(this.historigramData);
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
