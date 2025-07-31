# Codebase Structure

## Solution Structure
```
eShopOnWeb/
├── src/
│   ├── ApplicationCore/        # Domain layer (Clean Architecture)
│   ├── Infrastructure/         # Data access & external services
│   ├── Web/                   # ASP.NET Core MVC application
│   ├── PublicApi/             # FastEndpoints API
│   ├── BlazorAdmin/           # Blazor WebAssembly admin
│   ├── BlazorShared/          # Shared Blazor components
│   ├── eShopWeb.AppHost/      # .NET Aspire orchestration
│   └── eShopWeb.AspireServiceDefaults/
├── tests/
│   ├── UnitTests/             # Domain logic tests
│   ├── IntegrationTests/      # Data layer tests
│   ├── FunctionalTests/       # End-to-end web tests
│   └── PublicApiIntegrationTests/  # API endpoint tests
└── docs/                      # Documentation
```

## ApplicationCore Structure
- **Entities/**: Domain entities with aggregates (Basket, Order, Buyer, Catalog)
- **Interfaces/**: Service and repository contracts
- **Services/**: Domain services (BasketService, OrderService)
- **Specifications/**: Query specifications using Ardalis.Specification
- **Exceptions/**: Domain-specific exceptions
- **Constants/**: Application constants

## Key Aggregates
- **BasketAggregate/**: Shopping cart functionality
- **OrderAggregate/**: Order processing (Order, OrderItem, Address)
- **BuyerAggregate/**: Customer management (Buyer, PaymentMethod)
- **CatalogItem**: Product catalog management

## Infrastructure Layer
- **Data/**: EF Core contexts, configurations, repositories
- **Identity/**: ASP.NET Core Identity implementation  
- **Logging/**: Custom logging adapters
- **Services/**: External service implementations

## Configuration Files
- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development overrides
- `Directory.Packages.props`: Central package management
- `.editorconfig`: Code style rules
- `global.json`: .NET SDK version specification