The job is failing in the Angular unit tests (`npm run test`) because the `AppComponent` template contains custom components (`<app-toolbar>` and `<app-footer>`) that are **not declared/provided in the TestBed**, so Angular throws:

- `NG0304: 'app-toolbar' is not a known element`
- `NG0304: 'app-footer' is not a known element`

After that, one spec also fails because it expects the default Angular starter text, but your template now renders **“Please, login”** when not logged in.

## Fix (recommended): Update `AppComponent` unit test to include the missing components + correct expectation

### 1) Declare the components used by the template (Toolbar/Footer)
Edit: `src/UI.Angular.App/Expenses.App/src/app/app.component.spec.ts` at ref `5e571eb1c6e4bb3c04763c2df3af74911ce9c13c`

Add imports for the components and include them in `declarations`:

```ts
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';

import { SocialAuthService } from '@abacritt/angularx-social-login';
import { of } from 'rxjs';

// Add these:
import { ToolbarComponent } from './toolbar/toolbar.component';
import { FooterComponent } from './footer/footer.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [
        AppComponent,
        ToolbarComponent,
        FooterComponent,
      ],
      providers: [
        {
          provide: SocialAuthService,
          useValue: { authState: of(null) }, // not logged in
        },
      ],
    }).compileComponents();
  });

  // ... keep the other tests
});
```

This resolves the `NG0304` “not a known element” errors for toolbar/footer.

> Note: if `ToolbarComponent`/`FooterComponent` have their own dependencies (Material modules, etc.) and tests still fail, see the “Alternative” section below (stub components), which avoids pulling those dependencies into this unit test.

### 2) Fix the failing expectation in “should render title”
Your current template (`app.component.html`) shows:

- When `loggedIn` is false: `<span>Please, login</span>`

But the test currently expects: `Expenses.App app is running!`

Update that test accordingly:

```ts
it('should render login message when not logged in', () => {
  const fixture = TestBed.createComponent(AppComponent);
  fixture.detectChanges();
  const compiled = fixture.nativeElement as HTMLElement;
  expect(compiled.querySelector('.content span')?.textContent).toContain('Please, login');
});
```

That matches the real behavior driven by `authState: of(null)`.

## Alternative (often best for AppComponent tests): Stub the child components
If importing real `ToolbarComponent` / `FooterComponent` causes additional template/module errors, stub them instead:

```ts
import { Component } from '@angular/core';

@Component({ selector: 'app-toolbar', template: '' })
class ToolbarStubComponent {}

@Component({ selector: 'app-footer', template: '' })
class FooterStubComponent {}
```

…and then:

```ts
declarations: [AppComponent, ToolbarStubComponent, FooterStubComponent],
```

This keeps the `AppComponent` unit test focused and eliminates dependency churn.

## Why this fixes the pipeline
The failing GitHub Actions job runs `npm run test -- --watch=false --browsers=ChromeHeadless` in `src/UI.Angular.App/Expenses.App/`. Fixing the TestBed configuration and aligning the assertion with the current template removes both:
- the Angular runtime template errors (`NG0304`)
- the single failing Jasmine assertion in `app.component.spec.ts` (line referenced in logs)

After these changes, `npm run test` should return exit code 0, letting the workflow proceed to `npm run build` and deployment.