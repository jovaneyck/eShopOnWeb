# Essential Development Commands

## Project Setup
```bash
# Restore dependencies
dotnet restore

# Install EF Core tools globally
dotnet tool update --global dotnet-ef

# Database setup (from Web folder)
cd src/Web
dotnet ef database update -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
dotnet ef database update -c appidentitydbcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
```

## Build Commands
```bash
# Build entire solution
dotnet build ./eShopOnWeb.sln --configuration Debug

# Build for release
dotnet build ./eShopOnWeb.sln --configuration Release
```

## Testing Commands
```bash
# Run all tests with coverage
dotnet test ./eShopOnWeb.sln --no-restore --verbosity normal --collect:"XPlat Code Coverage" --logger trx --results-directory coverage

# Run specific test projects
dotnet test tests/UnitTests/UnitTests.csproj
dotnet test tests/IntegrationTests/IntegrationTests.csproj
dotnet test tests/FunctionalTests/FunctionalTests.csproj
dotnet test tests/PublicApiIntegrationTests/PublicApiIntegrationTests.csproj
```

## Running Applications
```bash
# Web Application (MVC) - https://localhost:5001
cd src/Web && dotnet run --launch-profile https

# Public API - https://localhost:5200
cd src/PublicApi && dotnet run

# Blazor Admin - https://localhost:5002
cd src/BlazorAdmin && dotnet run

# .NET Aspire Orchestration
cd src/eShopWeb.AppHost && dotnet run
```

## Database Operations
```bash
# Create migration (from Web folder)
dotnet ef migrations add MigrationName --context catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Data/Migrations

# Update database
dotnet ef database update -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
```

## Docker Commands
```bash
# Build and run with Docker Compose
docker-compose build
docker-compose up

# Access URLs:
# Web: http://localhost:5106  
# API: http://localhost:5200
```