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
  styleUrls: ['./month-summary.component.scss']
})
export class MonthSummaryComponent implements OnChanges {

  @Input() public date = new Date();
  
  summary: MonthlySummary;
  historigram: Historigram;
  expenses: Expense[] = [];
  columns: Column[] = ['Categoria', '%'];
  myHistorigramColumns = [
    'Element',
    'Density',
    { type: 'number', role: 'interval' },
    { type: 'number', role: 'interval' },
    { type: 'string', role: 'annotation' },
    { type: 'string', role: 'annotationText' }
  ];
  myHistorigramData : Row[] = [
    ['April', 1000, 900, 1100, 'A', 'Stolen data'],
    ['May', 1170, 1000, 1200, 'B', 'Coffee spill'],
    ['June', 660, 550, 800, 'C', 'Wumpus attack'],
    ['July', 1030, null, null, null, null]
  ];
  myHistorigramType: ChartType;

  myData: any[][] = [['Buit', 100]];
  myType: ChartType;
  
  list: any[] = [];

  constructor(private summariesService: SummariesService,
    private expensesService: ExpensesService,
    private historigramService: HistorigramService) 
  {
    this.myType = ChartType.PieChart;
    this.myHistorigramType = ChartType.LineChart;
    
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
    //this.getHistorigram();
  }

  getHistorigram(): void {
    this.historigramService.getHistorigram()
    .subscribe(historigram => this.historigram = historigram);
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
      this.resetChart();
      this.populateCharts();
    });
  }
  
  resetChart(): void {
    this.myData = [['Buit', 100]];
    this.myHistorigramData = [[]];
  }

  getTotalCost(): number {
    return Math.round(this.expenses.filter(e => e.amount < 0).map(t => t.amount).reduce((acc, value) => acc + value, 0));
  }

  populateCharts(): void {
    if (Object.keys(this.summary.progressBar).length > 0) {
      this.columns = Object.keys(this.summary.progressBar);
      this.myData = Object.entries(this.summary.progressBar);
    }

    if (Object.keys(this.historigram.progressLine).length > 0) {
      //this.historigramColumns = Object.keys(this.historigram.progressLine);
      //this.myHistorigramData = Object.entries(this.historigram.progressLine);
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
