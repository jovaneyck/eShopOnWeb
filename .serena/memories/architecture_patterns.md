# Architecture & Design Patterns

## Clean Architecture Layers
- **ApplicationCore**: Domain entities, business logic, interfaces (innermost layer)
- **Infrastructure**: Data access, external services, EF Core implementations
- **Web/PublicApi/BlazorAdmin**: UI and presentation layers (outermost)

## Key Design Patterns

### Repository Pattern
- `IRepository<T>` and `IReadRepository<T>` interfaces
- EF Core-based implementations
- Generic repository with entity-specific logic

### Specification Pattern
- `Ardalis.Specification` for complex queries
- Encapsulates query logic in reusable specifications
- Examples: `CatalogFilterPaginatedSpecification`, `BasketWithItemsSpecification`

### Domain-Driven Design (DDD)
- **Aggregates**: Basket, Order, Buyer, Catalog domains
- **Aggregate Roots**: Marked with `IAggregateRoot` interface
- **Value Objects**: Address, CatalogItemOrdered
- **Domain Services**: BasketService, OrderService

### CQRS with MediatR
- Command/Query separation in Web layer
- Handlers for business operations
- Clean separation of read/write operations

### Decorator Pattern
- Caching decorators for performance (e.g., `CachedCatalogViewModelService`)
- Cross-cutting concerns implementation

## Entity Structure
- **BaseEntity**: Common base class with Id property
- **Domain Aggregates**: 
  - BasketAggregate (Basket, BasketItem)
  - OrderAggregate (Order, OrderItem, Address)
  - BuyerAggregate (Buyer, PaymentMethod)
- **Catalog Entities**: CatalogItem, CatalogBrand, CatalogType