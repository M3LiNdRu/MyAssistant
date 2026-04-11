import { Component, OnInit, Output, EventEmitter  } from '@angular/core';
import { Router } from '@angular/router'

@Component({
    selector: 'app-toolbar',
    templateUrl: './toolbar.component.html',
    styleUrls: ['./toolbar.component.scss'],
    standalone: false
})
export class ToolbarComponent implements OnInit {
  public currentMonth: Date;
  private expensesList = false;
  private investmentsList = false;
  @Output() public monthChangedEvent = new EventEmitter<Date>();
  @Output() public showExpensesListEvent = new EventEmitter<boolean>();
  @Output() public showInvestmentsEvent = new EventEmitter<boolean>();
  @Output() public showPortfoliosEvent = new EventEmitter<boolean>();

  constructor(public router: Router)
  {
    this.currentMonth = new Date();
  }

  ngOnInit(): void {
    this.setMonth(0);
    this.monthChangedEvent.emit(this.currentMonth);
  }

  showExpensesList(): void {
    this.expensesList = !this.expensesList;
    this.investmentsList = false;
    this.showExpensesListEvent.emit(this.expensesList);
  }

  showInvestments(): void {
    this.investmentsList = !this.investmentsList;
    this.expensesList = false;
    this.showInvestmentsEvent.emit(this.investmentsList);
  }

  showPortfolios(): void {
    this.showPortfoliosEvent.emit(true);
  }

  previous(): void {
    this.setMonth(-1);
    this.monthChangedEvent.emit(this.currentMonth);
  }

  next(): void {
    this.setMonth(1);
    this.monthChangedEvent.emit(this.currentMonth);
  }

  private setMonth(delta: number): void {
    this.currentMonth.setMonth(this.currentMonth.getMonth() + delta);
    this.currentMonth = new Date(
      this.currentMonth.getFullYear(),
      this.currentMonth.getMonth(),
      1);
  }

}
