import { Component, OnInit } from '@angular/core';
import { SocialAuthService } from "@abacritt/angularx-social-login";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent implements OnInit {
  title = 'Expenses.App';
  current = new Date();
  display = true;
  displayForm = false;
  displayList = false;
  displayInvestmentsList = false;
  displayInvestmentsForm = false;
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
    this.displayInvestmentsList = false;
    this.displayInvestmentsForm = false;
    this.displayPortfolioManagement = false;
  }

  displayExpensesList(display: boolean) {
    this.display = !display;
    this.displayForm = false;
    this.displayInvestmentsList = false;
    this.displayInvestmentsForm = false;
    this.displayPortfolioManagement = false;
    this.displayList = display;
  }

  displayExpensesForm(display: boolean) {
    this.display = !display;
    this.displayForm = display;
    this.displayInvestmentsList = false;
    this.displayInvestmentsForm = false;
    this.displayPortfolioManagement = false;
  }

  displayInvestments(display: boolean) {
    this.display = !display;
    this.displayForm = false;
    this.displayList = false;
    this.displayInvestmentsList = display;
    this.displayInvestmentsForm = false;
    this.displayPortfolioManagement = false;
  }

  displayInvestmentsFormEvent(display: boolean) {
    this.display = !display;
    this.displayForm = false;
    this.displayList = false;
    this.displayInvestmentsList = false;
    this.displayInvestmentsForm = display;
    this.displayPortfolioManagement = false;
  }

  displayPortfolios(display: boolean) {
    this.display = !display;
    this.displayForm = false;
    this.displayList = false;
    this.displayInvestmentsList = false;
    this.displayInvestmentsForm = false;
    this.displayPortfolioManagement = display;
  }

}

