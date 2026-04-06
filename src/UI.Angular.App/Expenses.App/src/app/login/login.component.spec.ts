import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { LoginComponent } from './login.component';
import { SocialAuthService } from '@abacritt/angularx-social-login';
import { of } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LoginComponent],
      providers: [
        {
          provide: SocialAuthService,
          useValue: {
            authState: of(null),
            signOut: jasmine.createSpy('signOut'),
            getAccessToken: jasmine.createSpy('getAccessToken').and.returnValue(Promise.resolve('')),
            refreshAccessToken: jasmine.createSpy('refreshAccessToken'),
          },
        },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
