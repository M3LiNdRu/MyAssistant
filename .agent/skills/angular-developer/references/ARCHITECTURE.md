# Angular App Architecture Reference

## Project Structure

```
src/UI.Angular.App/Expenses.App/
├── src/
│   ├── app/
│   │   ├── categories-form/           # Modal dialog for adding categories
│   │   ├── display/                   # Summary amount display component
│   │   ├── expenses-form/             # Form to add new expenses
│   │   ├── expenses-list/             # MatTable of monthly expenses
│   │   ├── float-button/             # Floating action button (FAB)
│   │   ├── footer/                    # Footer component
│   │   ├── http-interceptors/         # Auth interceptor + barrel export
│   │   ├── login/                     # Google OAuth login component
│   │   ├── main-page/                 # Dashboard with PieChart summary
│   │   ├── month-summary/            # Monthly summary with Pie + Line charts
│   │   ├── not-authorized/           # 403 error page
│   │   ├── page-not-found/           # 404 error page
│   │   ├── tags-autocomplete-input/  # Reusable tags chip input
│   │   ├── toolbar/                   # Nav toolbar with month controls
│   │   ├── app.component.ts           # Root component (state manager)
│   │   ├── app.module.ts             # NgModule declarations & imports
│   │   ├── app-routing.module.ts     # Routing (minimal)
│   │   ├── categories.service.ts     # Categories API service
│   │   ├── category.ts               # Category interface
│   │   ├── currentSummary.ts         # CurrentSummary interface
│   │   ├── expenses.service.ts       # Expenses API service
│   │   ├── expense.ts                # Expense interface
│   │   ├── historigram.service.ts    # Historical chart data service
│   │   ├── historigram.ts            # Historigram + HistorigramDot interfaces
│   │   ├── monthlySummary.ts         # MonthlySummary interface
│   │   ├── summaries.service.ts      # Summary API service
│   │   ├── tag.ts                    # Tags interface
│   │   └── tags.service.ts           # Tags API service
│   ├── environments/
│   │   ├── environment.ts            # Dev config (apiUrl, GoogleClientId)
│   │   └── environment.prod.ts       # Prod config
│   ├── assets/
│   ├── index.html
│   ├── main.ts                       # Bootstrap entry point
│   ├── polyfills.ts
│   ├── styles.scss                   # Global styles
│   └── test.ts                       # Karma test entry
├── angular.json                      # Angular CLI configuration
├── karma.conf.js                     # Karma test runner config
├── package.json
├── tsconfig.json
├── tsconfig.app.json
└── tsconfig.spec.json
```

## Data Models

### Expense
```typescript
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

### Category
```typescript
export interface Category {
  id: number;
  name: string;
  description: string;
}
```

### Tags
```typescript
export interface Tags {
  tags: string[];
}
```

### CurrentSummary
```typescript
export interface CurrentSummary {
  month: string;
  year: string;
  totalAmount: number;
  progressBar: Dictionary<number>;  // category name → percentage
}
```

### MonthlySummary
```typescript
export interface MonthlySummary {
  month: string;
  year: string;
  start: number;
  saved: number;
  progressBar: Dictionary<number>;
  spentByCategory: Dictionary<number>;
}
```

### Historigram
```typescript
export interface Historigram {
  totalSavings: number;
  totalEarned: number;
  totalSpent: number;
  progressLine: Dictionary<HistorigramDot>;
}

export interface HistorigramDot {
  saved: number;
  spent: number;
  earned: number;
}
```

## Component Hierarchy

```
AppComponent (root — manages state flags & selected month)
├── LoginComponent (Google OAuth)
├── ToolbarComponent (navigation, month selector)
├── MainPageComponent (dashboard PieChart)
├── DisplayComponent (summary amount)
├── ExpensesListComponent (MatTable, receives date via @Input)
├── ExpensesFormComponent (add expense form)
│   └── TagsAutocompleteInputComponent (reusable tags chip input)
├── FloatButtonComponent (FAB to toggle form)
├── MonthSummaryComponent (monthly PieChart + LineChart)
├── FooterComponent
├── NotAuthorizedComponent (403)
└── PageNotFoundComponent (404)
```

## Environment Configuration

```typescript
// src/environments/environment.ts
export const environment = {
  production: false,
  apiUrl: "https://my-assistant-expenses-api.azurewebsites.net/",
  GoogleClientId: "1012495556001-1arrtrlhjcpnac4ql6g9783odd8j0h78.apps.googleusercontent.com"
};
```

## Build Configuration

- **Output**: `dist/expenses.app/browser/`
- **Budget**: 2MB warning / 5MB error for initial bundle
- **Dev server**: `npm run start` with source maps and no optimization
- **Production**: Output hashing, optimization, environment file replacement
