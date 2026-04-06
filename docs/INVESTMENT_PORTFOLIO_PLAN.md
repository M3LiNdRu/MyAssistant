# Investment Portfolio Tool - Implementation Plan

## Problem Statement

Add a new investment portfolio management tool to MyAssistant alongside the existing expense manager. The tool will track various investment types (cryptocurrencies, ETFs, gold, deposits, etc.) with portfolio grouping, performance metrics, and daily market price synchronization.

## Approach Overview

Follow the existing MyAssistant architecture pattern (expense manager) to build a parallel investment portfolio domain:

- **Backend**: Add a new Investments domain to the .NET API with support for multiple portfolio groups, asset types, and daily price syncing
- **Frontend**: Add an Investments module to the Angular app with portfolio dashboard and management UI
- **Data**: Store portfolios, investments, and market price history in MongoDB
- **Integration**: Reuse existing auth (Google JWT), DI patterns, and build/deployment pipeline

## Feature Scope (MVP)

### Backend (.NET API)

1. **Portfolio Management**: CRUD for portfolio groups (Retirement, Short-term, Emergency Fund, etc.)
2. **Investment Ledger**: CRUD for individual investments (asset type, quantity, purchase price, purchase date, current price)
3. **Asset Types**: Support for crypto, ETFs, gold, deposits, stocks, bonds, etc.
4. **Market Data Integration**: Daily batch job to sync current market prices from external APIs
5. **Portfolio Summary**: Endpoint to calculate total portfolio value, gain/loss %, asset allocation

### Frontend (Angular)

1. **Portfolio Navigation**: Switch between portfolio groups
2. **Investment List**: View all investments in selected portfolio with columns for asset, quantity, cost, current value, gain/loss
3. **Portfolio Dashboard**: Summary stats (total value, total gain/loss %, allocation pie chart)
4. **Investment Form**: Add/edit/delete individual investments
5. **Navigation**: Integrate investment module into main nav alongside expenses

### Data Model (MongoDB)

- **Portfolios**: User ID, portfolio name (group), description, created/updated dates
- **Investments**: Portfolio ID, asset type, symbol/ID, quantity, purchase price, purchase date, current price, created/updated
- **Price History**: Asset symbol, date, price (for historical tracking and daily syncs)
- **Asset Types**: Catalog of supported asset types (crypto, ETF, gold, deposit, etc.)

## Implementation Roadmap

### Phase 1: Backend Infrastructure

- [ ] Create Investments folder structure in `Api/src/Resources/` following Categories pattern
- [ ] Define MongoDB document models (Portfolio, Investment, PriceHistory, AssetType)
- [ ] Implement MongoDB repositories for CRUD operations
- [ ] Build Portfolio service with business logic
- [ ] Create REST endpoints (GET/POST/PUT/DELETE portfolios and investments)
- [ ] Create IServiceCollectionExtensions for DI registration

### Phase 2: Backend Market Data Integration

- [ ] Implement external API client (choose provider: CoinGecko for crypto, Alpha Vantage for stocks, etc.)
- [ ] Build service to fetch and store daily prices
- [ ] Implement storage for historical prices
- [ ] Create endpoint that calculates aggregated performance metrics

### Phase 3: Frontend Setup & Navigation

- [ ] Create investments folder structure mirroring expenses module
- [ ] Create Angular service for API calls to investments endpoints
- [ ] Add Investments link to toolbar/main navigation
- [ ] Verify Google auth token is passed to investment endpoints

### Phase 4: Frontend UI Components

- [ ] Component to switch between portfolio groups
- [ ] Display investments with columns (asset, qty, cost, current value, gain/loss %)
- [ ] Summary dashboard with total value, total gain/loss, allocation chart
- [ ] Form for adding/editing investments (asset type dropdown, quantity, purchase price, date)

### Phase 5: Integration & Testing

- [ ] Register investment services in Startup.ConfigureServices
- [ ] Ensure Swagger/OpenAPI documentation is generated for investment endpoints
- [ ] Manual test portfolio CRUD, market sync, dashboard
- [ ] Verify deployment to Azure with both expense and investment modules

## Key Architectural Decisions

1. **Domain Separation**: Investments as a separate resource domain alongside Expenses (not mixed)
2. **Portfolio Grouping**: User can create multiple portfolio groups; each investment belongs to one portfolio
3. **Market Data**: Daily batch job (no real-time updates) to keep costs low and avoid API rate limits
4. **Price History**: Store prices over time to enable historical trending in future enhancements
5. **UI Integration**: Single Angular app with expense and investment modules (tabs or sidebar nav)
6. **Authentication**: Reuse existing Google JWT auth; ensure investments are scoped to authenticated user

## Dependencies

- None between investment and expense modules (loosely coupled)
- Investment feature depends on MongoDB connectivity (existing)
- External market data API (TBD which provider)
- Angular Material for UI components (already in project)

## Recommended Implementation Order

1. **Start with Backend Infrastructure** (Phase 1): Get data models and basic CRUD working
2. **Add Market Data Layer** (Phase 2): Enable price syncing while backend is fresh
3. **Build Frontend Structure** (Phase 3): Module setup and navigation
4. **Implement UI Components** (Phase 4): Feature-complete UI
5. **E2E Test & Deploy** (Phase 5): Validation and production deployment

## Out of Scope (Future Enhancements)

- Real-time market data streaming
- Forecasting/projections
- Tax reporting/cost basis tracking
- Rebalancing recommendations
- Alert/notification system
- Mobile app
- Monthly or quarterly summaries
- YoY (year-over-year) comparisons

## Technology Stack

- **Backend**: ASP.NET Core 10, MongoDB C# driver, Entity Framework (optional)
- **Frontend**: Angular (latest from project), Angular Material, angular-google-charts
- **External APIs**: Market data provider (CoinGecko for crypto preferred)
- **Infrastructure**: Azure App Service, GitHub Actions CI/CD
- **Authentication**: Google JWT (existing pattern)

## Notes

- Follow existing code patterns from expense manager for consistency
- Use MongoDB transactions if portfolios are deleted (cascade to investments)
- Consider caching market prices in memory to avoid N+1 queries on portfolio summary
- Plan for Swagger doc generation for investment endpoints
- All new features should pass unit tests before integration
- Frontend and backend work can proceed in parallel after Phase 1 is complete
