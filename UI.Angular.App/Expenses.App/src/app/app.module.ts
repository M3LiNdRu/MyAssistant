import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
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
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider'; 
import { MatTableModule } from '@angular/material/table'; 
import { MatTabsModule } from '@angular/material/tabs';
import { MatMenuModule } from '@angular/material/menu';

import { SocialLoginModule, SocialAuthServiceConfig, GoogleLoginProvider, GoogleSigninButtonModule  } from '@abacritt/angularx-social-login';

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


@NgModule({ declarations: [
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
    bootstrap: [AppComponent], imports: [BrowserModule,
        AppRoutingModule,
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
        MatTabsModule,
        SocialLoginModule,
        GoogleSigninButtonModule,
        MatMenuModule], providers: [
        MatDatepickerModule,
        { provide: MAT_DATE_LOCALE, useValue: 'ca-ES' },
        httpInterceptorProviders,
        {
            provide: 'SocialAuthServiceConfig',
            useValue: {
                autoLogin: true,
                providers: [
                    {
                        id: GoogleLoginProvider.PROVIDER_ID,
                        provider: new GoogleLoginProvider('1012495556001-1arrtrlhjcpnac4ql6g9783odd8j0h78.apps.googleusercontent.com')
                    }
                ],
                onError: (err) => {
                    console.error(err);
                }
            } as SocialAuthServiceConfig,
        },
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule { }
