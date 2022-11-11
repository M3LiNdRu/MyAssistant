import { Component, OnInit, Output, EventEmitter  } from '@angular/core';
import { Router } from '@angular/router'

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {
  public currentMonth: Date;
  private expensesList = false;
  @Output() public monthChangedEvent = new EventEmitter<Date>();
  @Output() public showExpensesListEvent = new EventEmitter<boolean>();

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
    this.showExpensesListEvent.emit(this.expensesList);
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
