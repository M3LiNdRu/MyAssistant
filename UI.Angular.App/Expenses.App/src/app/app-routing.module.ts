import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExpensesFormComponent } from './expenses-form/expenses-form.component';
import { ExpensesListComponent } from './expenses-list/expenses-list.component';
import { MainPageComponent } from './main-page/main-page.component';
import { MonthSummaryComponent } from './month-summary/month-summary.component';
import { NotAuthorizedComponent } from './not-authorized/not-authorized.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

const routes: Routes = [
  { path: 'summary/last', component: MonthSummaryComponent },
  { path: 'summary', component: MainPageComponent },
  { path: 'expense/add', component: ExpensesFormComponent },
  { path: 'expenses/list', component: ExpensesListComponent },
  { path: 'not-authorized', component: NotAuthorizedComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
