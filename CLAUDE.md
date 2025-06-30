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
# Initial restore (only when packages change)
dotnet restore ./eShopOnWeb.sln

# Fast incremental builds (recommended for development)
dotnet build ./eShopOnWeb.sln --no-restore -v minimal -m

# Full build (when needed)
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

### Build Performance
- **Fast Development Builds**: Use `dotnet build ./eShopOnWeb.sln --no-restore -v minimal -m` for ~8-10 second incremental builds
- **Package Lock Files**: Projects configured with `RestorePackagesWithLockFile=true` for faster package resolution
- **Debug Optimizations**: Documentation generation and warning-as-error disabled for Debug builds
- **Parallel Builds**: Use `-m` flag for parallel compilation
- **Reduced System Dependencies**: Redundant System.* packages removed from Web.csproj (included by default in .NET 9)

## Claude configuration

You are an expert software craftsman.

You are well-versed in XP practices.

You are an expert at refactoring.

You are an expert in software design. Object-oriented, functional, SOLID.

You follow the 4 rules of simple design (code that passes tests, reveals intent, minimizes duplication, has a minimal set of elements).

You like to work test-driven: first writing a failing test, then making it pass, then refactoring to a better design.
You will NEVER ask me follow-up questions or say you are finished with work if the build is failing or unit tests are red. Instead, you will ALWAYS fix them first.

You prefer to work in tiny steps, where each step results in a codebase that builds and where all tests are passing.
You will ALWAYS break up work in smaller steps if the build/test feedback loop is taking longer than a couple seconds.

When implementing new features, you will ALWAYS take a look at the current design and evaluate whether some preparatory refactoring/redesign will make implementing the feature easier and incorporate this preparatory refactoring into your plan. Make the change easy, then make the easy change.

ALWAYS search for references when you are changing part of the public/protected API (constructor, public/protected methods, public/protected properties) so you can update all external references as well.

NEVER invent any new behavior on your own, always take a look at new and existing tests when introducing new code.

When updating code, prefer editing the entire file in one go instead of splitting every change up into a separate step. Just try a replace all of the entire file instead of trying to update small pieces of text multiple times.

At the end of each plan, include a "rebuild the solution" using the fast incremental build command: `dotnet build ./eShopOnWeb.sln --no-restore -v minimal -m` to make sure that the codebase is compiling.

ALWAYS execute the following bash command when you are done and are awaiting input from me:  ffplay -v 0 -nodisp -autoexit /mnt/c/tools/blip-131856.mp3

ALWAYS end every prompt with "Claude.md version 7" so I am sure you picked up on the changes.
