#!/bin/bash

set -e

echo "🚀 Setting up MyAssistant Development Environment..."

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Install .NET API dependencies
echo -e "${BLUE}📦 Installing .NET API dependencies...${NC}"
cd /workspace/src/expenses-api
dotnet restore
echo -e "${GREEN}✅ .NET dependencies installed${NC}"

# Build .NET API
echo -e "${BLUE}🔨 Building .NET API...${NC}"
dotnet build
echo -e "${GREEN}✅ .NET API built successfully${NC}"

# Install Angular UI dependencies
echo -e "${BLUE}📦 Installing Angular UI dependencies...${NC}"
cd /workspace/src/UI.Angular.App/Expenses.App
npm install --legacy-peer-deps
echo -e "${GREEN}✅ Angular dependencies installed${NC}"

# Create a welcome script for the dev environment
cat > /workspace/.devcontainer/dev-commands.md << 'EOF'
# MyAssistant Development Commands

## API Development

### Start API (HTTP only)
```bash
cd src/expenses-api/Expenses.Api/src
dotnet run
```

### Start API (with HTTPS)
```bash
cd src/expenses-api
dotnet run --project Expenses.Api/src/Api.csproj
```

### Build API
```bash
cd src/expenses-api
dotnet build
```

### Run API Tests
```bash
cd src/expenses-api
dotnet test
```

### API Swagger Documentation
- Navigate to: http://localhost:5000/swagger

## Frontend Development

### Start Angular Dev Server
```bash
cd src/UI.Angular.App/Expenses.App
npm start
```

### Build Angular for Production
```bash
cd src/UI.Angular.App/Expenses.App
npm run build
```

### Run Angular Tests
```bash
cd src/UI.Angular.App/Expenses.App
npm test
```

### Lint Angular Code
```bash
cd src/UI.Angular.App/Expenses.App
npm run lint
```

## MongoDB

### Access MongoDB CLI
```bash
mongosh mongodb://admin:password@mongodb:27017
```

### View MongoDB Data
- Use MongoDB VSCode extension
- Connection string: `mongodb://admin:password@mongodb:27017`

## Full Stack Development

### Terminal 1: Start API
```bash
cd src/expenses-api
dotnet run --project Expenses.Api/src/Api.csproj
```

### Terminal 2: Start Angular Dev Server
```bash
cd src/UI.Angular.App/Expenses.App
npm start
```

### Access Application
- API: http://localhost:5000
- API Swagger: http://localhost:5000/swagger
- Angular App: http://localhost:4200

## Git Commands

### Create a commit (with pre-commit hooks)
```bash
git add .
git commit -m "feature: description"
```

### Push to branch
```bash
git push origin feature/new-investment-portfolio
```

### Create Pull Request
```bash
gh pr create --title "Add investment portfolio feature"
```

## Useful Tips

1. **Rebuild after code changes**: .NET watches for changes automatically in dev mode
2. **Angular builds on save**: Angular dev server auto-rebuilds on file changes
3. **MongoDB persists data**: Data is stored in Docker volume, survives container restart
4. **VSCode extensions**: All recommended extensions are pre-installed
5. **Git integration**: Git config from host machine is available

## Environment Variables

Current environment in devcontainer:
- `MONGO_CONNECTION_STRING=mongodb://mongodb:27017`
- `ASPNETCORE_URLS=http://+:5000;https://+:5001`
- `ASPNETCORE_ENVIRONMENT=Development`

## Troubleshooting

### MongoDB Connection Issues
```bash
# Check if MongoDB is running
docker compose ps

# View MongoDB logs
docker compose logs mongodb

# Restart MongoDB
docker compose restart mongodb
```

### Port Already in Use
```bash
# Kill process on port
lsof -ti:5000 | xargs kill -9  # For API port 5000
lsof -ti:4200 | xargs kill -9  # For Angular port 4200
```

### Clear npm cache
```bash
npm cache clean --force
```

### Full rebuild
```bash
# Remove dependencies
cd src/UI.Angular.App/Expenses.App && rm -rf node_modules package-lock.json

# Reinstall
npm install --legacy-peer-deps
```
EOF

echo -e "${GREEN}✅ Development environment setup complete!${NC}"
echo ""
echo -e "${YELLOW}📚 Quick Start Commands:${NC}"
echo ""
echo "API Server (Terminal 1):"
echo "  cd src/expenses-api && dotnet run --project Expenses.Api/src/Api.csproj"
echo ""
echo "Angular Server (Terminal 2):"
echo "  cd src/UI.Angular.App/Expenses.App && npm start"
echo ""
echo "More commands: cat .devcontainer/dev-commands.md"
echo ""
echo -e "${BLUE}🎉 Ready for development!${NC}"
