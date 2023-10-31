import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatToolbarModule } from '@angular/material/toolbar'; 
import { MatIconModule } from '@angular/material/icon'; 
import { MatLegacyButtonModule as MatButtonModule } from '@angular/material/legacy-button';
import { MatLegacyCardModule as MatCardModule } from '@angular/material/legacy-card'; 
import { MatLegacyFormFieldModule as MatFormFieldModule } from '@angular/material/legacy-form-field'; 
import { MatLegacySelectModule as MatSelectModule } from '@angular/material/legacy-select'; 
import { MatLegacyInputModule as MatInputModule } from '@angular/material/legacy-input'; 
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatLegacyChipsModule as MatChipsModule } from '@angular/material/legacy-chips'; 
import { MatLegacyAutocompleteModule as MatAutocompleteModule } from '@angular/material/legacy-autocomplete'; 
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatLegacyDialogModule as MatDialogModule } from '@angular/material/legacy-dialog';
import { MatDividerModule } from '@angular/material/divider'; 
import { MatLegacyTableModule as MatTableModule } from '@angular/material/legacy-table'; 
import { MatLegacyTabsModule as MatTabsModule } from '@angular/material/legacy-tabs';

import { GoogleChartsModule } from 'angular-google-charts';

import { httpInterceptorProviders } from './http-interceptors'

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
import { TagsAutocompleteInputComponent } from './tags-autocomplete-input/tags-autocomplete-input.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { CategoriesFormComponent } from './categories-form/categories-form.component';
import { NotAuthorizedComponent } from './not-authorized/not-authorized.component';


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
    MonthSummaryComponent,
    TagsAutocompleteInputComponent,
    PageNotFoundComponent,
    CategoriesFormComponent,
    NotAuthorizedComponent,
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
    MatNativeDateModule,
    MatChipsModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatDialogModule,
    MatDividerModule,
    GoogleChartsModule,
    MatTableModule,
    MatTabsModule
  ],
  providers: [
    MatDatepickerModule,
    { provide: MAT_DATE_LOCALE, useValue: 'ca-ES' },
    httpInterceptorProviders
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
