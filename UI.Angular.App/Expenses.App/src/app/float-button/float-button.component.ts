import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-float-button',
  templateUrl: './float-button.component.html',
  styleUrls: ['./float-button.component.scss']
})
export class FloatButtonComponent implements OnInit {
  @Output() public displayExpenseFormEvent = new EventEmitter<boolean>();
  
  constructor(public router: Router) { }

  ngOnInit(): void {
  }

  displayExpensesForm(): void {
    this.displayExpenseFormEvent.emit(true);
  }

}
