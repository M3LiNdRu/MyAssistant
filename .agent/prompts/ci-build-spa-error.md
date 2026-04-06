The failing job is your Angular unit-test step in `.github/workflows/master_my-assistant-expenses-api.yml` at commit `37bbc0459129dce0c82a299fc0d503366b71e2df` (workflow runs `npm run test -- --watch=false --browsers=ChromeHeadless`). Tests fail due to missing providers/modules in the Angular TestBed, not because of the runner or Azure deploy.

## Root causes from the logs
1. **Missing Social Login config provider**
   - Error: `No provider found for InjectionToken SocialAuthServiceConfig`
   - This happens when `@abacritt/angularx-social-login`’s `SocialAuthService` is injected but the `SocialAuthServiceConfig` token isn’t provided in the test module.

2. **Missing HttpClient provider**
   - Error: `No provider found for _HttpClient`
   - Tests that create components/services that inject `HttpClient` need `HttpClientTestingModule` (or `provideHttpClient()` in newer Angular).

3. **Angular Material components not imported**
   - Errors like: `'mat-icon' is not a known element`, `'mat-card' is not a known element`, `'mat-toolbar' is not a known element`
   - These come from specs that declare the component but don’t import the corresponding Material modules (or don’t stub schemas).

## Fix (recommended): Update the failing specs to include the required testing imports/providers

### 1) Fix `LoginComponent` test
File: `src/UI.Angular.App/Expenses.App/src/app/login/login.component.spec.ts` (ref `37bbc045...`)

Replace the TestBed setup with something like:

```ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { LoginComponent } from './login.component';

import {
  SocialAuthServiceConfig,
  SocialLoginModule,
} from '@abacritt/angularx-social-login';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        SocialLoginModule, // ensures module-level pieces are present
      ],
      declarations: [LoginComponent],
      providers: [
        {
          provide: 'SocialAuthServiceConfig',
          useValue: {
            autoLogin: false,
            providers: [],
          } as SocialAuthServiceConfig,
        },
      ],
      // If the template contains Material tags you aren't testing here, this avoids "not known element"
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
```

Notes:
- The key part is providing the config token. Some versions use the string token `'SocialAuthServiceConfig'` as above; if your code imports the token as an `InjectionToken`, use that exact token instead.

### 2) Fix components/services that need `HttpClient`
Any spec failing with `_HttpClient` should import the HttpClient testing module.

Example pattern:

```ts
import { HttpClientTestingModule } from '@angular/common/http/testing';

await TestBed.configureTestingModule({
  imports: [HttpClientTestingModule],
  declarations: [YourComponent],
}).compileComponents();
```

Apply this to the specs mentioned in logs (e.g., `MainPageComponent` spec you shared at `src/UI.Angular.App/Expenses.App/src/app/main-page/main-page.component.spec.ts`) if the component (or something it injects) uses `HttpClient`.

### 3) Fix Material “not a known element” errors
Best practice is to import the Material modules your template uses, e.g.:

```ts
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';

await TestBed.configureTestingModule({
  imports: [
    MatIconModule,
    MatCardModule,
    MatToolbarModule,
  ],
  declarations: [YourComponent],
}).compileComponents();
```

If you don’t care about template rendering in these unit tests and only want “should create”, a faster alternative is adding:

```ts
schemas: [NO_ERRORS_SCHEMA]
```

This suppresses unknown element errors (good for shallow tests, but don’t use it if you want template-level confidence).

## Why this will unblock the job
Your workflow is running unit tests headlessly in CI; the Angular TestBed currently doesn’t mirror the runtime module configuration (providers + imports), so component creation fails immediately. Adding the missing providers (`SocialAuthServiceConfig`), testing imports (`HttpClientTestingModule`), and either Material modules or `NO_ERRORS_SCHEMA` will bring the TestBed up to parity and stop the “Executed 22 of 22 (14 FAILED)” cascade.