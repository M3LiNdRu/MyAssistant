import { Component, OnInit } from '@angular/core';
import { SocialAuthService } from "@abacritt/angularx-social-login";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent implements OnInit {
  title = 'MyAssistant';
  current = new Date();
  display = true;
  displayForm = false;
  displayList = false;
  displayTransactionsList = false;
  displayTransactionsForm = false;
  displayPortfolioManagement = false;
  loggedIn = false;

  constructor(private authService: SocialAuthService) {

  }

  ngOnInit(): void {
    this.authService.authState.subscribe((user) => { this.loggedIn = (user != null); });
  }

  useMonth(month: Date) {
    console.log("Get expenses for month: " + month);
    this.current = month;
  }

  displaySummary(display: boolean) {
    console.log("display summary: " + display);
    this.display = display;
    this.displayForm = false;
    this.displayTransactionsList = false;
    this.displayTransactionsForm = false;
    this.displayPortfolioManagement = false;
  }

  displayExpensesSummary(display: boolean) {
    this.display = display;
    this.displayForm = false;
    this.displayList = false;
    this.displayTransactionsList = false;
    this.displayTransactionsForm = false;
    this.displayPortfolioManagement = false;
  }

  displayExpensesList(_display: boolean) {
    const showList = !this.displayList;
    this.display = !showList;
    this.displayForm = false;
    this.displayList = showList;
    this.displayTransactionsList = false;
    this.displayTransactionsForm = false;
    this.displayPortfolioManagement = false;
  }

  displayExpensesForm(display: boolean) {
    this.display = !display;
    this.displayForm = display;
    this.displayTransactionsList = false;
    this.displayTransactionsForm = false;
    this.displayPortfolioManagement = false;
  }

displayTransactionsFormEvent(display: boolean) {
    this.display = !display;
    this.displayForm = false;
    this.displayList = false;
    this.displayTransactionsList = false;
    this.displayTransactionsForm = display;
    this.displayPortfolioManagement = false;
  }

  displayPortfolios(display: boolean) {
    this.display = !display;
    this.displayForm = false;
    this.displayList = false;
    this.displayTransactionsList = false;
    this.displayTransactionsForm = false;
    this.displayPortfolioManagement = display;
  }

}

