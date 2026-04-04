# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MyAssistant is a personal assistant tool for managing expenses. It consists of a .NET 8 backend API and an Angular frontend SPA, deployed together to Azure App Service.

## Architecture

- **Backend**: `src/expenses-api/` — ASP.NET Core 8 Web API with MongoDB storage
  - `Expenses.Api/src/` — Main API project (assembly: `MyAssistant.Apis.Expenses.Api`)
  - `Library.MongoDb/` — Shared MongoDB data access library
  - Solution file: `src/expenses-api/MyAssitant.Apis.sln` (note the typo "Assitant")
  - Authentication: Google JWT (configured via `Authentication:Google:Authority` and `Authentication:Google:ClientId`)
  - Each resource domain (Categories, Expenses, Tags, Summary, Historigrams) follows a consistent pattern: Controller, Service, Repositories, Requests/Responses DTOs, and an `IServiceCollectionExtensions` for DI registration
- **Frontend**: `src/UI.Angular.App/Expenses.App/` — Angular app with Angular Material UI
  - Uses Google Social Login (`@abacritt/angularx-social-login`)
  - Uses `angular-google-charts` for chart visualizations
  - Styles: SCSS
- **Deployment**: The SPA builds into `dist/expenses.app/browser/` and is served as static files from the API's `wwwroot/`

## Build Commands

### Backend (.NET API)
```bash
cd src/expenses-api
dotnet build
dotnet build --configuration Release
dotnet publish -c Release -o <output-dir>
```

### Frontend (Angular SPA)
```bash
cd src/UI.Angular.App/Expenses.App
npm i --legacy-peer-deps    # --legacy-peer-deps is required
npm run build
npm run start               # dev server
npm run test                # karma/jasmine tests
```

## CI/CD

GitHub Actions workflow (`.github/workflows/master_my-assistant-expenses-api.yml`) triggers on push to `master`. It builds the API, then the SPA, then deploys both to Azure.

## Key Patterns

- Resource modules register their services via extension methods (e.g., `services.RegisterCategoriesFeatures()`) called from `Startup.ConfigureServices`
- MongoDB configuration is done via `services.ConfigureMongoDb(Configuration)` from the `Library.MongoDb` project
- Swagger UI is available at `/swagger`
