# MyAssistant DevContainer Setup

This directory contains the DevContainer configuration for MyAssistant development environment.

## Overview

The DevContainer provides a complete, isolated development environment with:
- **.NET 10 SDK** - For the ASP.NET Core API
- **Node.js + npm** - For Angular frontend development
- **MongoDB 7.0** - For data persistence
- **Docker** - For containerized services
- **Development tools** - ESLint, Prettier, Git tools, etc.

## Prerequisites

1. **Docker Desktop** installed and running
2. **VSCode** with Remote Containers extension (`ms-vscode-remote.remote-containers`)
3. **Git** configured on your host machine

## Quick Start

### 1. Open in DevContainer

**Option A: Using VSCode**
1. Open the project folder in VSCode
2. Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
3. Search for "Dev Containers: Reopen in Container"
4. Select and wait for the container to build (3-5 minutes first time)

**Option B: Using Command Line**
```bash
# Build and start the container
docker compose -f .devcontainer/docker-compose.yml up -d

# Open a shell in the container
docker compose -f .devcontainer/docker-compose.yml exec devcontainer bash
```

### 2. Run the Application

**Terminal 1: Start the API**
```bash
cd src/expenses-api
dotnet run --project Expenses.Api/src/Api.csproj
```

**Terminal 2: Start Angular Dev Server**
```bash
cd src/UI.Angular.App/Expenses.App
npm start
```

### 3. Access Services

- **Angular App**: http://localhost:4200
- **API Server**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **MongoDB**: mongodb://admin:password@localhost:27017

## Services

### Development Container
- **Image**: Custom build from `.devcontainer/Dockerfile`
- **Includes**: .NET 10, Node.js, Angular CLI, development tools
- **Ports**: 5000, 5001, 4200, 9090
- **Volume**: Your entire workspace is mounted at `/workspace`

### MongoDB
- **Image**: mongo:7.0
- **Credentials**: 
  - Username: `admin`
  - Password: `password`
- **Port**: 27017
- **Data**: Persisted in Docker volume `mongodb_data`

## Configuration Files

### `devcontainer.json`
Main DevContainer configuration:
- Service definition
- Port forwarding
- VSCode extensions and settings
- Environment variables
- Mount points for git config

### `docker-compose.yml`
Orchestrates the devcontainer and MongoDB:
- Service definitions
- Volume management
- Network configuration
- Health checks

### `Dockerfile`
Custom development image:
- Base: .NET 10 (mcr.microsoft.com/devcontainers/dotnet:1-10)
- Adds: Node.js, Angular CLI, development tools
- Installs: dotnet tools (ef, format, reportgenerator)

### `post-create.sh`
Runs after container initialization:
- Restores .NET dependencies
- Builds the API
- Installs npm packages
- Generates development command reference

## Included VSCode Extensions

### Backend Development
- **C# Dev Kit** - C# language support
- **.NET Runtime** - .NET runtime management
- **Makefile Tools** - Build system support
- **NuGet Tools** - Package management

### Frontend Development
- **Angular Language Service** - Angular template support
- **ESLint** - JavaScript linting
- **Prettier** - Code formatting

### Database & Tools
- **MongoDB for VSCode** - MongoDB integration
- **REST Client** - API testing

### Utilities
- **GitLens** - Git integration
- **Git Graph** - Git visualization
- **Code Spell Checker** - Spell checking

## Environment Variables

Available in the container:
```
MONGO_CONNECTION_STRING=mongodb://mongodb:27017
ASPNETCORE_URLS=http://+:5000;https://+:5001
ASPNETCORE_ENVIRONMENT=Development
```

## Development Workflow

### Running Tests

**API Tests**
```bash
cd src/expenses-api
dotnet test
```

**Angular Tests**
```bash
cd src/UI.Angular.App/Expenses.App
npm test
```

### Code Formatting

**C# Code**
```bash
cd src/expenses-api
dotnet format
```

**TypeScript/HTML/SCSS**
```bash
cd src/UI.Angular.App/Expenses.App
npm run lint
npm run prettier
```

### Building for Production

**API**
```bash
cd src/expenses-api
dotnet build --configuration Release
```

**Angular**
```bash
cd src/UI.Angular.App/Expenses.App
npm run build
```

## Troubleshooting

### Container Won't Start
```bash
# Check docker compose logs
docker compose -f .devcontainer/docker-compose.yml logs devcontainer

# Rebuild container (clean)
docker compose -f .devcontainer/docker-compose.yml down
docker compose -f .devcontainer/docker-compose.yml up --build
```

### MongoDB Connection Issues
```bash
# Verify MongoDB is running
docker compose -f .devcontainer/docker-compose.yml ps

# Check MongoDB logs
docker compose -f .devcontainer/docker-compose.yml logs mongodb

# Restart MongoDB
docker compose -f .devcontainer/docker-compose.yml restart mongodb
```

### Port Already in Use
```bash
# Find and kill process on port
lsof -ti:5000 | xargs kill -9     # API port
lsof -ti:4200 | xargs kill -9     # Angular port
lsof -ti:27017 | xargs kill -9    # MongoDB port
```

### NPM Dependency Issues
```bash
cd src/UI.Angular.App/Expenses.App
rm -rf node_modules package-lock.json
npm install --legacy-peer-deps
```

### .NET Dependency Issues
```bash
cd src/expenses-api
rm -rf obj bin
dotnet restore
dotnet build
```

## Working with Git

Your git configuration from the host machine is available:
- SSH keys are mounted from `~/.ssh`
- Git config is mounted from `~/.gitconfig`

```bash
# Pull/push normally
git pull origin feature/new-investment-portfolio
git push origin feature/new-investment-portfolio

# Create commit (pre-commit hooks work)
git commit -m "feat: add investment portfolio"
```

## Data Persistence

### MongoDB Data
- **Location**: Docker volume `mongodb_data`
- **Persists**: Through container restarts
- **Clean**: `docker volume rm myassistant_mongodb_data` (⚠️ destructive)

### Workspace
- **Location**: Mounted from host directory
- **Persists**: Always (stored on host)

## Performance Tips

1. **Use WSL2 backend** (Windows) for better Docker performance
2. **Exclude large directories** from VSCode file watching
3. **Use `--legacy-peer-deps`** with npm for faster installs
4. **Cache node_modules** in Docker volume for faster rebuilds

## Rebuilding Container

```bash
# Rebuild from scratch
docker compose -f .devcontainer/docker-compose.yml down -v
docker compose -f .devcontainer/docker-compose.yml up --build
```

## Useful Commands

```bash
# View logs from all services
docker compose -f .devcontainer/docker-compose.yml logs -f

# Execute command in container
docker compose -f .devcontainer/docker-compose.yml exec devcontainer bash

# Remove all containers and volumes
docker compose -f .devcontainer/docker-compose.yml down -v

# Restart all services
docker compose -f .devcontainer/docker-compose.yml restart
```

## Additional Resources

- [VSCode Remote Development](https://code.visualstudio.com/docs/remote/remote-overview)
- [Dev Containers Specification](https://containers.dev/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [.NET Development Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Angular Development Guide](https://angular.io/guide/setup-local)

## Notes

- First build takes 3-5 minutes (downloading images, installing dependencies)
- Subsequent starts are faster (uses cached layers)
- Changes in `Dockerfile` or dependencies require rebuild
- VSCode extensions install automatically in container
- Git history is accessible inside the container
