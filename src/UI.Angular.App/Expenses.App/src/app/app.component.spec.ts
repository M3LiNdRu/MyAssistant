import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AppComponent } from './app.component';
import { SocialAuthService } from '@abacritt/angularx-social-login';
import { of } from 'rxjs';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { FooterComponent } from './footer/footer.component';

@Component({ selector: 'app-login', template: '', standalone: false })
class LoginStubComponent {}

@Component({ selector: 'app-footer', template: '', standalone: false })
class FooterStubComponent {}

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MatToolbarModule,
        MatIconModule,
        MatButtonModule,
      ],
      declarations: [
        AppComponent,
        ToolbarComponent,
        LoginStubComponent,
        FooterStubComponent,
      ],
      providers: [
        {
          provide: SocialAuthService,
          useValue: { authState: of(null) },
        },
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'MyAssistant'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('MyAssistant');
  });

  it('should not be logged in by default', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const app = fixture.componentInstance;
    expect(app.loggedIn).toBeFalse();
  });
});
