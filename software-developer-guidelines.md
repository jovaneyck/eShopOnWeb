# Software Developer Guidelines

This document outlines the coding standards, patterns, and best practices for the eShopOnWeb project based on established patterns in the codebase.

## Table of Contents

1. [Service Layer Architecture](#service-layer-architecture)
2. [Domain-Driven Design (DDD) Patterns](#domain-driven-design-ddd-patterns)
3. [Repository Pattern Implementation](#repository-pattern-implementation)
4. [Testing Strategy](#testing-strategy)
5. [Code Quality Standards](#code-quality-standards)

## Service Layer Architecture

### Service Pattern Guidelines

Services in this codebase follow a specific pattern as exemplified by `BasketService`. Follow these guidelines:

#### 1. Service Structure

```csharp
public class BasketService : IBasketService
{
    private readonly IRepository<Basket> _basketRepository;
    private readonly IAppLogger<BasketService> _logger;

    public BasketService(IRepository<Basket> basketRepository,
        IAppLogger<BasketService> logger)
    {
        _basketRepository = basketRepository;
        _logger = logger;
    }
}
```

**Key Principles:**
- Always inject dependencies through constructor
- Use repository pattern for data access
- Include logging for service operations
- Implement corresponding interface

#### 2. Service Method Patterns

**Async Operations:**
```csharp
public async Task<Basket> AddItemToBasket(string username, int catalogItemId, decimal price, int quantity = 1)
{
    var basketSpec = new BasketWithItemsSpecification(username);
    var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

    if (basket == null)
    {
        basket = new Basket(username);
        await _basketRepository.AddAsync(basket);
    }

    basket.AddItem(catalogItemId, price, quantity);
    await _basketRepository.UpdateAsync(basket);
    return basket;
}
```

**Result Pattern Usage:**
```csharp
public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
{
    var basketSpec = new BasketWithItemsSpecification(basketId);
    var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
    if (basket == null) return Result<Basket>.NotFound();
    
    // Business logic here
    await _basketRepository.UpdateAsync(basket);
    return basket;
}
```

#### 3. Service Best Practices

- **Use Specifications:** Always use specifications for complex queries
- **Guard Clauses:** Use `Guard.Against.Null()` for null checks
- **Business Logic in Domain:** Keep business logic in domain entities, not services
- **Logging:** Log important operations with meaningful context
- **Error Handling:** Use Result pattern for operations that can fail

## Domain-Driven Design (DDD) Patterns

### Aggregate Design

#### 1. Aggregate Root Structure

```csharp
public class Basket : BaseEntity, IAggregateRoot
{
    public string BuyerId { get; private set; }
    private readonly List<BasketItem> _items = new List<BasketItem>();
    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();
    
    public int TotalItems => _items.Sum(i => i.Quantity);
}
```

**Key Principles:**
- Inherit from `BaseEntity` and implement `IAggregateRoot`
- Use private setters for properties
- Encapsulate collections with private backing fields
- Expose collections as `IReadOnlyCollection<T>`
- Include computed properties for aggregate calculations

#### 2. Entity Design

```csharp
public class BasketItem : BaseEntity
{
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public int CatalogItemId { get; private set; }
    public int BasketId { get; private set; }

    public BasketItem(int catalogItemId, int quantity, decimal unitPrice)
    {
        CatalogItemId = catalogItemId;
        UnitPrice = unitPrice;
        SetQuantity(quantity);
    }
}
```

**Key Principles:**
- Private setters for all properties
- Constructor that enforces invariants
- Business methods for state changes
- Use Guard clauses for validation

#### 3. Business Logic in Aggregates

```csharp
public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
{
    if (!Items.Any(i => i.CatalogItemId == catalogItemId))
    {
        _items.Add(new BasketItem(catalogItemId, quantity, unitPrice));
        return;
    }
    var existingItem = Items.First(i => i.CatalogItemId == catalogItemId);
    existingItem.AddQuantity(quantity);
}

public void RemoveEmptyItems()
{
    _items.RemoveAll(i => i.Quantity == 0);
}
```

**Key Principles:**
- Encapsulate business rules within the aggregate
- Maintain consistency through business methods
- Use meaningful method names that express business intent
- Validate business rules at the aggregate level

### Specification Pattern

```csharp
public sealed class BasketWithItemsSpecification : Specification<Basket>
{
    public BasketWithItemsSpecification(int basketId)
    {
        Query
            .Where(b => b.Id == basketId)
            .Include(b => b.Items);
    }

    public BasketWithItemsSpecification(string buyerId)
    {
        Query
            .Where(b => b.BuyerId == buyerId)
            .Include(b => b.Items);
    }
}
```

**Key Principles:**
- Use `Specification<T>` base class from Ardalis.Specification
- Create overloaded constructors for different query scenarios
- Always include all child entities for aggregate consistency
- Use meaningful names that describe the query intent

## Repository Pattern Implementation

### Repository Interface

```csharp
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
```

### Repository Implementation

```csharp
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> 
    where T : class, IAggregateRoot
{
    public EfRepository(CatalogContext dbContext) : base(dbContext)
    {
    }
}
```

**Key Principles:**
- Repository works only with aggregate roots
- Inherit from Ardalis.Specification's `RepositoryBase<T>`
- Use Entity Framework Core for data access
- Implement both `IReadRepository<T>` and `IRepository<T>`

### Repository Usage Patterns

```csharp
// Create
var basket = new Basket(username);
await _basketRepository.AddAsync(basket);

// Read with specification
var basketSpec = new BasketWithItemsSpecification(username);
var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

// Update
basket.AddItem(catalogItemId, price, quantity);
await _basketRepository.UpdateAsync(basket);

// Delete
await _basketRepository.DeleteAsync(basket);
```

## Testing Strategy

### Integration Testing

Integration tests focus on testing the data access layer with a real database context.

#### Test Structure

```csharp
public class BasketRepositoryTests
{
    private (DbContext, EfRepository<Basket> repository) BuildFreshRepository()
    {
        var context = new CatalogContext(new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog")
            .Options);
        return (context, new EfRepository<Basket>(context));
    }

    [Fact]
    public async Task CanPersistAndReadAggregate()
    {
        var (writeContext, writeRepo) = BuildFreshRepository();
        var basket = new BasketBuilder().WithOneBasketItem().Build();
        await writeRepo.AddAsync(basket, TestContext.Current.CancellationToken);
        await writeContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        
        var (_, repo) = BuildFreshRepository();
        var retrieved = await repo.FirstOrDefaultAsync(
            new BasketWithItemsSpecification(basket.Id), 
            TestContext.Current.CancellationToken);
        
        Assert.NotNull(retrieved);
        Assert.Equal(basket.BuyerId, retrieved.BuyerId);
        Assert.Equal(basket.TotalItems, retrieved.TotalItems);
    }
}
```

**Key Principles:**
- Use In-Memory Database for isolation
- Test complete aggregate persistence and retrieval
- Verify all important properties are persisted correctly
- Use a fresh repository instance for each test and in each test use a fresh repository between Act and Assert phases to make sure ef is not up to any caching.
- Test with specifications to ensure proper query behavior

### Unit Testing

Unit tests focus on testing business logic in isolation using mocks and test doubles.

#### Test Structure

```csharp
public class AddItemToBasket
{
    private readonly string _buyerId = "Test buyerId";
    private readonly IRepository<Basket> _basketRepo = new InMemoryBasketRepository();
    private readonly IAppLogger<BasketService> _mockLogger = Substitute.For<IAppLogger<BasketService>>();

    [Fact]
    public async Task AddsNewItemToEmptyBasket()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);
        
        var result = await basketService.AddItemToBasket(_buyerId, 1, 1.50m);
        
        Assert.NotNull(result);
        var persistedBasket = await _basketRepo.FirstOrDefaultAsync(new BasketWithItemsSpecification(_buyerId), CancellationToken.None);
        Assert.NotNull(persistedBasket);
        Assert.Equal(_buyerId, persistedBasket.BuyerId);
        Assert.Single(persistedBasket.Items);
        Assert.Equal(1, persistedBasket.Items.First().CatalogItemId);
        Assert.Equal(1.50m, persistedBasket.Items.First().UnitPrice);
        Assert.Equal(1, persistedBasket.Items.First().Quantity);
    }
}
```

**Key Principles:**
- Use in-memory repository implementations for fast tests
- Build test data using test data builders
- Mock external dependencies (like logging)
- Test business logic flows
- Verify state changes and side effects
- Use descriptive test method names
- Test both happy path and error conditions

#### Test Data Builders

```csharp
public class BasketBuilder
{
    private string _buyerId = "testbuyerId@test.com";
    private int _basketId = 1;
    private readonly List<(int catalogItemId, decimal unitPrice, int quantity)> _items = new();

    public BasketBuilder WithBuyerId(string buyerId)
    {
        _buyerId = buyerId;
        return this;
    }

    public BasketBuilder WithItem(int catalogItemId, decimal unitPrice, int quantity = 1)
    {
        _items.Add((catalogItemId, unitPrice, quantity));
        return this;
    }

    public Basket Build()
    {
        var b = new Basket(_buyerId);
        SetId(b, _basketId);
        foreach (var i in _items)
        {
            b.AddItem(i.catalogItemId, i.unitPrice, i.quantity);
        }
        return b;
    }
}
```

**Key Principles:**
- Use fluent interface for readable test setup
- Provide reasonable defaults
- Allow customization of any property
- Keep builders simple and focused
- Use reflection EXCEPTIONALLY and only when necessary for setting internal state

#### In-Memory Repository Pattern

```csharp
public class InMemoryBasketRepository : IRepository<Basket>
{
    private readonly List<Basket> _baskets = new();
    private int _nextId = 1;

    public Task<Basket> AddAsync(Basket entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == 0)
        {
            var idProperty = typeof(Basket).BaseType!.GetProperty("Id");
            idProperty!.SetValue(entity, _nextId++);
        }
        _baskets.Add(entity);
        return Task.FromResult(entity);
    }

    // ... other repository methods
}
```

**Key Principles:**
- Implement full repository interface
- Use in-memory collections like lists and dictionaries for storage
- Handle ID generation for new entities
- Support specifications for querying

## Code Quality Standards

### General Guidelines

1. **Naming Conventions:**
   - Use PascalCase for classes, methods, and properties
   - Use camelCase for private fields with underscore prefix
   - Use meaningful, descriptive names

2. **Dependency Injection:**
   - Always inject dependencies through constructors
   - Use interfaces for dependencies
   - Register services in DI container

3. **Error Handling:**
   - Use Result pattern for operations that can fail
   - Use Guard clauses for argument validation
   - Log exceptions with meaningful context

4. **Async/Await:**
   - Use async/await for all I/O operations
   - Include CancellationToken parameters
   - Use ConfigureAwait(false) in library code

### Performance Considerations

1. **Entity Framework:**
   - Use specifications for complex queries
   - Avoid N+1 queries with proper includes
   - Use projected queries when only specific properties are needed

This document serves as a living guide for development practices. It should be updated as the codebase evolves and new patterns emerge.