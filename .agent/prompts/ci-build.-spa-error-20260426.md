The failing job is your Angular unit tests in `build-spa` (step “npm install and npm run test”). The logs show Angular template compilation errors and 2 failing assertions:

- `NG0304: 'mat-toolbar' / 'mat-toolbar-row' is not a known element` (coming from `ToolbarComponent` template)
- `NG0304: 'app-login' is not a known element` (used somewhere in `AppComponent` template)
- Test failures in `src/app/app.component.spec.ts`:
  - expects `app.title` to be `'Expenses.App'` but actual is `'MyAssistant'`
  - expects `.content span` to contain `'Please, login'` but the element/text is missing (`undefined`)

## Fix 1: Make the AppComponent tests match the current AppComponent
Right now `AppComponent` sets:

- `title = 'MyAssistant'` in [`src/UI.Angular.App/Expenses.App/src/app/app.component.ts` @ `b8c6d0c...`](https://github.com/M3LiNdRu/MyAssistant/blob/b8c6d0c9b2ba49c492908548cc151ec07eab7232/src/UI.Angular.App/Expenses.App/src/app/app.component.ts#L11)

But the spec still expects the old title:

- [`app.component.spec.ts` @ `b8c6d0c...`](https://github.com/M3LiNdRu/MyAssistant/blob/b8c6d0c9b2ba49c492908548cc151ec07eab7232/src/UI.Angular.App/Expenses.App/src/app/app.component.spec.ts#L37-L41)

### Option A (recommended): Update the spec to the current title
Change:

```ts
expect(app.title).toEqual('Expenses.App');
```

to:

```ts
expect(app.title).toEqual('MyAssistant');
```

### Option B: Change the app title back
If you really want the title to be `Expenses.App`, change in `app.component.ts`:

```ts
title = 'Expenses.App';
```

## Fix 2: Fix “not a known element” errors in the test module (Angular Material + app-login)
Your test bed declares `ToolbarComponent` and `FooterComponent`, but it does **not** import the Angular Material modules needed by `ToolbarComponent` (`mat-toolbar`, `mat-toolbar-row`). In unit tests, Angular still compiles component templates, so missing module imports will throw `NG0304`.

Also, `app-login` is referenced by the `AppComponent` template, but the `LoginComponent` (or whatever declares `selector: 'app-login'`) is not declared/imported in the test module.

### Update `app.component.spec.ts` to import Material and declare/mock Login
Edit [`src/UI.Angular.App/Expenses.App/src/app/app.component.spec.ts` @ `b8c6d0c...`](https://github.com/M3LiNdRu/MyAssistant/blob/b8c6d0c9b2ba49c492908548cc151ec07eab7232/src/UI.Angular.App/Expenses.App/src/app/app.component.spec.ts):

1) Import the Material module(s) used by `ToolbarComponent`:

```ts
import { MatToolbarModule } from '@angular/material/toolbar';
```

2) Provide a stub for `<app-login>` (quickest) **or** declare the real LoginComponent.

**Stub approach (quick and keeps this spec focused):**

```ts
import { Component } from '@angular/core';

@Component({ selector: 'app-login', template: '' })
class LoginStubComponent {}
```

3) Add the module + stub to the TestBed:

```ts
await TestBed.configureTestingModule({
  imports: [
    RouterTestingModule,
    MatToolbarModule,
  ],
  declarations: [
    AppComponent,
    ToolbarComponent,
    FooterComponent,
    LoginStubComponent,
  ],
  providers: [
    {
      provide: SocialAuthService,
      useValue: { authState: of(null) },
    },
  ],
}).compileComponents();
```

This should eliminate:
- `mat-toolbar` / `mat-toolbar-row` unknown element errors
- `app-login` unknown element errors

## Fix 3: Make the “Please, login” expectation align with the actual DOM
The failing assertion:

```ts
expect(compiled.querySelector('.content span')?.textContent).toContain('Please, login');
```

fails because the element is missing or the text isn’t rendered in that selector anymore.

You have two choices:

### Option A: Update the selector/text to whatever the template currently renders
Adjust the test to match the real UI. (This is the correct approach if the UI changed intentionally.)

### Option B: Restore the expected markup in the template
If you intend the app to show a “Please, login” message when `loggedIn === false`, ensure `app.component.html` renders something like:

```html
<div class="content" *ngIf="!loggedIn">
  <span>Please, login</span>
</div>
```

(Your spec already sets `authState: of(null)`, so `loggedIn` should remain false after `detectChanges()`.)

## Summary of the minimal patch that should unblock CI
- In `app.component.spec.ts`:
  - import `MatToolbarModule`
  - declare a stub `app-login` component (or declare the real Login component)
  - update the title expectation to `'MyAssistant'` (or change the app title back)
  - fix the login-message assertion to match the real template (or restore the template message)

Once those are applied, `npm run test -- --watch=false --browsers=ChromeHeadless` in the workflow (`.github/workflows/master_my-assistant-expenses-api.yml` @ `b8c6d0c...`) should stop failing.