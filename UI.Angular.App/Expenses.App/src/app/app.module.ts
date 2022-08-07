import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatToolbarModule } from '@angular/material/toolbar'; 
import { MatIconModule } from '@angular/material/icon'; 
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card'; 
import { MatFormFieldModule } from '@angular/material/form-field'; 
import { MatSelectModule } from '@angular/material/select'; 
import { MatInputModule } from '@angular/material/input'; 
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatChipsModule } from '@angular/material/chips'; 
import { MatAutocompleteModule } from '@angular/material/autocomplete'; 


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { ExpensesListComponent } from './expenses-list/expenses-list.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { FooterComponent } from './footer/footer.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { FloatButtonComponent } from './float-button/float-button.component';
import { DisplayComponent } from './display/display.component';
import { ExpensesFormComponent } from './expenses-form/expenses-form.component';
import { MainPageComponent } from './main-page/main-page.component';
import { MonthSummaryComponent } from './month-summary/month-summary.component';



@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ExpensesListComponent,
    FooterComponent,
    ToolbarComponent,
    FloatButtonComponent,
    DisplayComponent,
    ExpensesFormComponent,
    MainPageComponent,
    MonthSummaryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NoopAnimationsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatDatepickerModule,
    MatChipsModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
