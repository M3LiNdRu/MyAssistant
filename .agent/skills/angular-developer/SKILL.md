---
name: angular-developer
description: Expert Angular frontend developer for the MyAssistant Expenses SPA. Use when working with Angular components, templates, services, Material UI, SCSS styling, Google Charts, routing, authentication, RxJS observables, or any frontend task in the Expenses.App project.
---

# Senior Angular Developer Skill

## Workflow Instructions

**IMPORTANT**: When this skill is invoked for complex multi-file implementations, code generation, or architectural tasks:

1. **Use a subagent** to perform the actual implementation work to keep the main session context clean
2. **The subagent should**:
   - Read this entire SKILL.md file as the first action
   - Read `references/ARCHITECTURE.md` for project structure and conventions
   - Apply all principles, patterns, and best practices defined below
   - Implement the requested features following the guidelines
   - Return a concise summary of what was implemented

3. **For simple tasks** (single file edits, quick answers, code explanations), work directly without a subagent

**When to use subagent**:
- Creating new components or services
- Refactoring across multiple files
- Implementing new features with tests
- Complex template or styling work

**When to work directly**:
- Answering Angular questions
- Explaining code patterns
- Single file edits or quick bug fixes
- Code reviews

## Project Context

- **Location**: `src/UI.Angular.App/Expenses.App/`
- **Angular Version**: 20.x (NgModule-based, `standalone: false`)
- **UI Framework**: Angular Material 20.x with `indigo-pink` prebuilt theme
- **Charts**: `angular-google-charts` 16.x
- **Auth**: `@abacritt/angularx-social-login` 2.x (Google OAuth)
- **Styling**: SCSS (component-scoped + global `styles.scss`)
- **Testing**: Karma 6.x + Jasmine 4.x
- **Build Output**: `dist/expenses.app/browser/` (served as static files from the .NET API's `wwwroot/`)

## Build & Run Commands

```bash
cd src/UI.Angular.App/Expenses.App
npm i --legacy-peer-deps    # REQUIRED: --legacy-peer-deps flag
npm run build               # Production build
npm run start               # Dev server
npm run test                # Karma/Jasmine tests
```

## Core Principles

### 1. Follow Existing Architecture

This app uses **NgModule-based architecture** (not standalone components). All new components and services must:
- Be declared in `app.module.ts` (declarations array for components, providers for services)
- Use `standalone: false` (the default)
- Import required Angular Material modules in `app.module.ts`

### 2. Component Patterns

Components follow this structure:
```
src/app/<component-name>/
├── <component-name>.component.ts
├── <component-name>.component.html
├── <component-name>.component.scss
└── <component-name>.component.spec.ts
```

**Parent-child communication**:
- `@Input()` for passing data down (e.g., selected date)
- `@Output()` with `EventEmitter` for notifying parents
- No NgRx or state management library — state lives in `AppComponent` properties

**AppComponent state flags** control view visibility:
```typescript
display: boolean;      // show summary
displayForm: boolean;  // show expense form
displayList: boolean;  // show expenses list
loggedIn: boolean;     // auth state
current: Date;         // selected month
```

### 3. Service Patterns

All services use `providedIn: 'root'` and follow this pattern:

```typescript
@Injectable({ providedIn: 'root' })
export class ExampleService {
  private url = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getData(): Observable<T> {
    return this.http.get<T>(`${this.url}api/v1/endpoint`).pipe(
      catchError(this.handleError<T>('getData'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}
```

### 4. API Endpoints

All endpoints are relative to `environment.apiUrl`:

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/expenses/monthly/{year}/{month}` | Expenses by month |
| GET | `/api/v1/expenses/monthly` | Current month expenses |
| POST | `/api/v1/expense` | Create expense |
| GET | `/api/v1/categories` | List categories |
| POST | `/api/v1/category` | Create category |
| GET | `/api/v1/tags` | Get available tags |
| GET | `/api/v1/summary/current` | Current month summary |
| GET | `/api/v1/summary/monthly/{year}/{month}` | Monthly summary |
| GET | `/api/v1/historigram/savings` | Historical savings data |

### 5. Authentication

- Google OAuth via `@abacritt/angularx-social-login`
- Token stored in `localStorage` as `"id_token"`
- `AuthInterceptor` (at `http-interceptors/authInterceptor.ts`) attaches Bearer token to all HTTP requests
- Interceptors exported via barrel file at `http-interceptors/index.ts`
- Auto-login is enabled in the `SocialAuthServiceConfig`

### 6. Angular Material Usage

Import Material modules in `app.module.ts`. Currently used modules:
- MatToolbar, MatIcon, MatButton, MatCard
- MatFormField, MatSelect, MatInput
- MatDatepicker, MatNativeDateModule
- MatChips, MatAutocomplete
- MatDialog, MatDivider
- MatTable, MatTabs, MatMenu

**Date locale**: `MAT_DATE_LOCALE` set to `'ca-ES'` (Catalan)

### 7. Charts (Google Charts)

Use `angular-google-charts` with `GoogleChartsModule`:

```typescript
// PieChart example (from MainPageComponent)
chartType = ChartType.PieChart;
chartColumns = ['Categoria', '%'];
chartData = [['Food', 45], ['Transport', 30]];

// LineChart example (from MonthSummaryComponent)
chartType = ChartType.LineChart;
chartColumns = ['Month', 'saved', 'spent', 'earned'];
```

### 8. Styling Guidelines

- **Global styles**: `src/styles.scss`
- **Component styles**: SCSS files co-located with components
- **Theme**: Angular Material `indigo-pink` prebuilt theme
- **Typography**: Applied via `class="mat-typography"` on `<body>` in `index.html`
- Keep component styles scoped; use global styles sparingly

### 9. TypeScript Interfaces

Define interfaces in standalone `.ts` files in `src/app/`:

```typescript
// expense.ts
export interface Expense {
  id: string;
  name: string;
  category: string;
  timestamp: Date;
  amount: number;
  currency: string;
  tags: string[];
}
```

Existing interfaces: `expense.ts`, `category.ts`, `tag.ts`, `currentSummary.ts`, `monthlySummary.ts`, `historigram.ts`

### 10. Testing Strategy

- Tests live alongside components as `*.spec.ts` files
- Use `TestBed.configureTestingModule()` with required imports and providers
- Mock services with Jasmine spies
- Run with `npm run test`
- Coverage output: `coverage/expenses.app/`

```typescript
describe('ExampleComponent', () => {
  let component: ExampleComponent;
  let fixture: ComponentFixture<ExampleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ExampleComponent],
      imports: [/* required Material modules */],
      providers: [/* mock services */]
    }).compileComponents();

    fixture = TestBed.createComponent(ExampleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
```

## Domain-Specific Notes

- **"Ingressos"** (Income) category: amount sign is inverted when creating expenses
- **"Historigram"** is the project's spelling for histogram — keep it consistent
- **Tags input**: Uses `MatChips` + `MatAutocomplete` with `ENTER` and `COMMA` separators (from `@angular/cdk/keycodes`)
- **Navigation**: Component visibility toggling in AppComponent rather than route-based navigation
- **Locale**: Catalan/Spanish date formatting (`ca-ES`)

## Common Anti-Patterns to Avoid

- Do not use standalone components — this project uses NgModule architecture
- Do not add NgRx or other state management — use the existing service + EventEmitter pattern
- Do not change `--legacy-peer-deps` — it is required for dependency resolution
- Do not hardcode API URLs — always use `environment.apiUrl`
- Do not create route-based navigation without discussing with the user first (app uses component toggling)
- Do not remove or refactor the `handleError` pattern in services — it is used consistently across all services

## Quick Checklist

Before completing any Angular task, verify:

- [ ] Component declared in `app.module.ts`
- [ ] Required Material modules imported in `app.module.ts`
- [ ] Services use `providedIn: 'root'`
- [ ] API calls use `environment.apiUrl` prefix
- [ ] Error handling follows the `handleError` pattern
- [ ] SCSS used for component styles (not CSS)
- [ ] Spec file created alongside component
- [ ] `@Input`/`@Output` used for component communication
- [ ] No standalone components introduced
- [ ] `--legacy-peer-deps` used when installing packages
