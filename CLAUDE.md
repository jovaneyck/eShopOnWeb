# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

eShopOnWeb is a Microsoft ASP.NET Core reference application demonstrating Clean Architecture principles in a single-process (monolithic) e-commerce application. It runs on .NET 9.0 and showcases modern web development patterns including ASP.NET Core MVC, Blazor WebAssembly, and API development.

## Essential Commands

### Development Setup
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

### Build and Test
```bash
# Full build
dotnet build ./eShopOnWeb.sln --configuration Debug

# Run all tests with coverage
dotnet test ./eShopOnWeb.sln --no-restore --verbosity normal --collect:"XPlat Code Coverage" --logger trx --results-directory coverage

# Run specific test project
Unit tests: dotnet test tests/UnitTests/UnitTests.csproj
Integration tests: dotnet test tests/IntegrationTests/IntegrationTests.csproj
Functional tests: dotnet test tests/FunctionalTests/FunctionalTests.csproj
```

### Running Applications

#### Web Application (MVC)
```bash
cd src/Web
dotnet run --launch-profile https
# Access: https://localhost:5001
# Admin (Blazor): https://localhost:5001/admin
```

#### Public API
```bash
cd src/PublicApi
dotnet run
# Access: https://localhost:5200
```

#### Blazor Admin (Standalone)
```bash
cd src/BlazorAdmin
dotnet run
# Access: https://localhost:5002
```

#### Using Docker
```bash
# Build and run all services
docker-compose build
docker-compose up
# Web: http://localhost:5106
# API: http://localhost:5200
```

#### .NET Aspire (Orchestrated)
```bash
cd src/eShopWeb.AppHost
dotnet run
# Access Aspire dashboard for service management
```

## Architecture Overview

### Clean Architecture Layers
- **ApplicationCore**: Domain entities, business logic, interfaces (inner layer)
- **Infrastructure**: Data access, external services, EF Core implementations
- **Web**: ASP.NET Core MVC application with Razor Pages
- **PublicApi**: FastEndpoints-based API for external integrations
- **BlazorAdmin**: Blazor WebAssembly admin interface
- **BlazorShared**: Shared models and interfaces for Blazor components

### Key Patterns
- **Repository Pattern**: `IRepository<T>` with EF Core implementation
- **Specification Pattern**: Complex queries using Ardalis.Specification
- **Domain-Driven Design**: Aggregates with `IAggregateRoot` marker
- **CQRS with MediatR**: Command/Query separation in Web layer
- **Decorator Pattern**: Caching services (e.g., `CachedCatalogViewModelService`)

### Domain Aggregates
- **Basket**: Shopping cart management (`BasketAggregate/`)
- **Order**: Order processing and tracking (`OrderAggregate/`)
- **Buyer**: Customer information (`BuyerAggregate/`)
- **CatalogItem**: Product catalog management

### Data Access
- **CatalogContext**: Business data (products, orders, baskets)
- **AppIdentityDbContext**: Authentication and user management
- **Repository**: Generic repository pattern with specifications
- **Migrations**: EF Core migrations for database schema

### Authentication
- **ASP.NET Core Identity**: Primary authentication system
- **JWT Bearer**: API authentication
- **OAuth Integration**: GitHub SSO support
- **Role-based Authorization**: Built-in role management system

## Development Workflow

### Adding New Features
1. Define domain entities in `ApplicationCore/Entities/`
2. Create specifications in `ApplicationCore/Specifications/`
3. Implement repository interfaces in `Infrastructure/Data/`
4. Add EF configurations in `Infrastructure/Data/Config/`
5. Create API endpoints in `PublicApi/` using FastEndpoints
6. Implement UI in `Web/` (MVC) or `BlazorAdmin/` (Blazor)

### Database Changes
```bash
# Create new migration (from Web folder)
dotnet ef migrations add MigrationName --context catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Data/Migrations

# Update database
dotnet ef database update -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
```

### Testing Strategy
- **Unit Tests**: `tests/UnitTests/` - Domain logic and services
- **Integration Tests**: `tests/IntegrationTests/` - Data layer with real database
- **Functional Tests**: `tests/FunctionalTests/` - End-to-end web testing
- **API Tests**: `tests/PublicApiIntegrationTests/` - API endpoint testing

## Configuration

### Database Options
- **SQL Server**: Default for production (configured in `appsettings.json`)
- **In-Memory**: For development/testing (set `"UseOnlyInMemoryDatabase": true`)

### Environment-Specific Settings
- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development overrides
- `appsettings.Docker.json`: Docker-specific settings

### Key Configuration Sections
- **ConnectionStrings**: Database connections
- **BaseUrls**: Service endpoint configurations
- **Authentication**: OAuth and JWT settings
- **Logging**: Structured logging configuration

## Important Notes

- **Default Login**: `demouser@microsoft.com` (seeded in development)
- **Admin Access**: Use `/admin` route for Blazor admin interface
- **API Documentation**: FastEndpoints provides automatic Swagger documentation
- **Health Checks**: Available at `/health` endpoint
- **Caching**: Implemented via decorator pattern for performance
- **Docker Support**: Full containerization with docker-compose
- **.NET Aspire**: Modern cloud-native orchestration support

## Troubleshooting

### Common Issues
- **Database Connection**: Ensure SQL Server is running or use in-memory database
- **HTTPS Certificates**: Run `dotnet dev-certs https --trust` for development
- **Port Conflicts**: Check `launchSettings.json` for port configurations
- **Missing Dependencies**: Run `dotnet restore` and `dotnet tool restore`

### Performance Optimization
- Leverage caching decorators for frequently accessed data
- Use specifications for complex queries instead of raw LINQ
- Implement pagination for large datasets
- Consider using in-memory database for development/testing
