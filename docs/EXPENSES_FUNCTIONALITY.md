# MyAssistant - Expenses Manager Functionality

## Overview

The Expenses Manager is the core feature of MyAssistant, a personal assistant tool for tracking and managing personal expenses. It provides a complete expense tracking solution with categorization, tagging, and financial summaries.

## Current Features

### 1. Expense Management

#### Add Expense
- **Endpoint**: `POST /api/v1/expense`
- **Fields**:
  - `name` (string): Expense description/title
  - `amount` (decimal): Expense amount
  - `currency` (string): Currency code (default: EUR)
  - `category` (string): Expense category (must exist)
  - `timestamp` (datetime): Date of expense (default: current UTC time)
  - `tags` (array of strings): Custom tags for organization

#### Delete Expense
- **Endpoint**: `DELETE /api/v1/expense/{id}`
- **Parameters**:
  - `id` (string): MongoDB ObjectId of the expense to delete

#### Retrieve Expenses
- **Get All Expenses**: `GET /api/v1/expenses`
  - Returns all expenses for authenticated user
- **Get Current Month Expenses**: `GET /api/v1/expenses/monthly`
  - Returns expenses from the first day of current month to today
- **Get Monthly Expenses by Year/Month**: `GET /api/v1/expenses/monthly/{year}/{month}`
  - Returns expenses for a specific month (e.g., `/expenses/monthly/2025/3` for March 2025)

### 2. Category Management

#### Add Category
- **Endpoint**: `POST /api/v1/category`
- **Fields**:
  - `name` (string): Category name (unique)
  - `description` (string): Optional category description

#### List All Categories
- **Endpoint**: `GET /api/v1/categories`
- **Returns**: Array of all expense categories

#### Delete Category
- **Endpoint**: `DELETE /api/v1/category/{name}`
- **Parameters**:
  - `name` (string): Name of category to delete

### 3. Tags Management

#### List All Tags
- **Endpoint**: `GET /api/v1/tags`
- **Returns**: All tags used across expenses

#### Tag Auto-Creation
- Tags are automatically created when referenced in expense entries
- Avoids manual tag management while maintaining tag history

### 4. Financial Summaries

#### Current Month Summary
- **Endpoint**: `GET /api/v1/summary/current`
- **Returns** (`CurrentSummary`):
  - Current month total expenses
  - Breakdown by category
  - Currency information

#### Monthly Detailed Summary
- **Endpoint**: `GET /api/v1/summary/monthly/{year}/{month}`
- **Returns** (`CompleteSummary`):
  - Monthly expense total
  - Expenses by category with amounts
  - Additional analytics

### 5. Historical Data Visualization

#### Savings Historigram (Chart Data)
- **Endpoint**: `GET /api/v1/historigram/savings`
- **Returns** (`SavingsHistorigram`):
  - Historical savings/expense data
  - Time-series data for chart visualization
  - Supports trends and patterns analysis

## Data Model

### Expense Document
```json
{
  "_id": ObjectId,
  "timestamp": DateTime,
  "category": string,
  "name": string,
  "amount": decimal,
  "currency": string,
  "tags": [string]
}
```

### Category Document
```json
{
  "_id": ObjectId,
  "name": string,
  "description": string
}
```

### Tag Document
```json
{
  "_id": ObjectId,
  "name": string
}
```

## Frontend Components

### Main Application Structure

#### 1. **Login Component**
- Google OAuth authentication
- User identity verification
- Session management

#### 2. **Toolbar Component**
- Navigation between features
- User profile information
- Logout functionality

#### 3. **Main Page Component**
- Application shell
- Layout management
- Main navigation

#### 4. **Expenses Form Component**
- Add new expenses
- Edit existing expenses
- Category dropdown selection
- Tags input with autocomplete
- Currency selection
- Date picker for expense timestamp

#### 5. **Expenses List Component**
- Display expenses in table format
- View all or filtered expenses
- Delete expenses
- Sort capabilities
- Expense details display

#### 6. **Categories Form Component**
- Add new categories
- Delete categories
- Category management UI
- Modal-based dialog

#### 7. **Month Summary Component**
- Monthly expense overview
- Category breakdown
- Visual charts
- Expense trends

#### 8. **Display Component**
- Main content area
- Tab/view switching
- Layout management

#### 9. **Tags Autocomplete Component**
- Tag suggestion/autocomplete
- Tag input with filtering
- Reusable component for expense tagging

#### 10. **Float Button Component**
- Quick access button
- Add new expense shortcut
- Floating action button UI

#### 11. **Footer Component**
- Application footer
- Additional information

#### 12. **Not Authorized Component**
- Authentication error handling

#### 13. **Page Not Found Component**
- 404 error handling

### Frontend Services

#### Categories Service
- Fetch all categories
- Create new category
- Delete category
- Cache management

#### Expenses Service
- Create expense
- Delete expense
- Fetch all expenses
- Fetch monthly expenses
- Expense filtering

#### Summary Service
- Get current month summary
- Get historical summaries
- Calculate expense totals

#### Historigram Service
- Fetch savings historigram data
- Format data for chart visualization
- Support for angular-google-charts

### Frontend Features

#### Authentication
- Google Social Login (`@abacritt/angularx-social-login`)
- JWT token management via HTTP interceptors
- Automatic token refresh
- Protected routes with authorization guards

#### UI Framework
- Angular Material for Material Design components
- Google Charts (`angular-google-charts`) for data visualization
- SCSS for styling
- Responsive layout

#### User Workflows

1. **Add Expense**
   - User clicks "Add Expense" button
   - Opens expense form modal
   - Selects category from dropdown
   - Enters amount and description
   - Adds tags with autocomplete
   - Sets date/time
   - Submits to API

2. **View Expenses**
   - Browse expenses in list
   - Filter by date/category
   - View details
   - Delete if needed

3. **Manage Categories**
   - View all categories
   - Add new category
   - Remove unused categories

4. **View Monthly Summary**
   - See current month overview
   - View category breakdown
   - Analyze spending patterns
   - Compare with historical data

## Authentication & Security

- **Auth Method**: Google JWT (OpenID Connect)
- **Configuration**: Via `Authentication:Google:Authority` and `Authentication:Google:ClientId`
- **Authorization**: `[Authorize]` attribute on all controllers
- **Token Handling**: HTTP interceptors automatically attach JWT to requests
- **Scoped Data**: Each user's data is isolated and only accessible with valid JWT

## Backend Architecture

### Stack
- **Framework**: ASP.NET Core 8
- **Database**: MongoDB
- **API Patterns**: RESTful API with clear resource endpoints
- **Logging**: Integrated logging throughout

### Design Patterns
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic separation
- **Dependency Injection**: ASP.NET Core DI container
- **Attributes**: Model validation and controller behavior customization
- **Swagger/OpenAPI**: Automatic API documentation

### Folder Structure
```
src/expenses-api/Expenses.Api/src/
├── Resources/
│   ├── Categories/
│   │   ├── Category.cs
│   │   ├── Controller.cs
│   │   ├── Service.cs
│   │   ├── Repositories.cs
│   │   ├── Requests.cs
│   │   ├── Responses.cs
│   │   └── IServiceCollectionExtensions.cs
│   ├── Expenses/
│   ├── Tags/
│   ├── Summary/
│   ├── Historigrams/
│   └── Developer/ (health checks)
├── Authentication/
├── Attributes/
├── Swagger/
├── Startup.cs
└── Program.cs
```

## Frontend Architecture

### Stack
- **Framework**: Angular (latest stable)
- **Styling**: SCSS
- **UI Library**: Angular Material
- **Charting**: angular-google-charts
- **Testing**: Jasmine/Karma

### Folder Structure
```
src/UI.Angular.App/Expenses.App/src/app/
├── components/
│   ├── login/
│   ├── toolbar/
│   ├── main-page/
│   ├── expenses-form/
│   ├── expenses-list/
│   ├── categories-form/
│   ├── month-summary/
│   ├── display/
│   ├── tags-autocomplete-input/
│   ├── float-button/
│   ├── footer/
│   ├── not-authorized/
│   └── page-not-found/
├── services/
│   ├── categories.service.ts
│   ├── expenses.service.ts
│   ├── summaries.service.ts
│   └── historigram.service.ts
├── models/
│   ├── category.ts
│   ├── expense.ts
│   ├── summary.ts
│   └── more...
├── http-interceptors/
├── guards/ (auth guards)
└── app.component.ts
```

## Deployment

### Continuous Integration/Deployment
- **CI/CD Tool**: GitHub Actions
- **Trigger**: Commits to `master` branch
- **Build Steps**:
  1. Build .NET backend (`dotnet build`)
  2. Build Angular frontend (`npm run build`)
  3. Deploy to Azure App Service
- **Static Files**: Angular build output (`dist/expenses.app/browser/`) served from API's `wwwroot/`

### Hosting
- **Platform**: Azure App Service
- **Backend**: ASP.NET Core 8 hosted in Docker container
- **Frontend**: Static SPA files served from same host
- **Configuration**: Environment variables for auth endpoints, MongoDB connection, API keys

## Future Enhancement Opportunities

- [ ] Recurring expenses
- [ ] Budget planning and alerts
- [ ] Export expenses (CSV, PDF)
- [ ] Mobile app support
- [ ] Multi-user households/family sharing
- [ ] Expense splitting
- [ ] Receipt image attachments
- [ ] Advanced analytics and forecasting
- [ ] Multiple currency support with conversion
- [ ] Payment method tracking
- [ ] Custom date range reports

## API Documentation

All endpoints are documented in **Swagger UI** available at `/swagger` when the API is running.

## Known Limitations

- Tags are created automatically but cannot be deleted directly (they persist)
- No soft delete for expenses (permanent deletion)
- Category deletion may impact historical reports
- Single user per authentication context
- No bulk operations for expenses

## Related Features

This expense manager integrates with the broader MyAssistant platform, which will include:
- Investment portfolio tracking
- Task/appointment management
- Notes and journaling
- Other personal assistant tools
