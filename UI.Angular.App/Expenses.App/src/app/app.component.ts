import { Component, OnInit } from '@angular/core';
import { SocialAuthService } from "@abacritt/angularx-social-login";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Expenses.App';
  current = new Date();
  display = true;
  displayForm = false;
  displayList = false;
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
  }

  displayExpensesList(display: boolean) {
    this.display = !display; 
    this.displayForm = false;
    this.displayList = display;
  }

  displayExpensesForm(display: boolean) {
    this.display = !display;
    this.displayForm = display;
  }

}

