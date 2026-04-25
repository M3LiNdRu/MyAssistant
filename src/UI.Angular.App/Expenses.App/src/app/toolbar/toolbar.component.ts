import { Component, OnInit, Input, Output, EventEmitter  } from '@angular/core';
import { Router } from '@angular/router'

@Component({
    selector: 'app-toolbar',
    templateUrl: './toolbar.component.html',
    styleUrls: ['./toolbar.component.scss'],
    standalone: false
})
export class ToolbarComponent implements OnInit {
  @Input() showMonthBar = true;
  public currentMonth: Date;
  @Output() public monthChangedEvent = new EventEmitter<Date>();
  @Output() public showExpensesListEvent = new EventEmitter<boolean>();
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
    this.showExpensesListEvent.emit(true);
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
