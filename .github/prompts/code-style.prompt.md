# Reference implementation

Always follow the structure and coding style of the reference implementation. Read every file to view the latest coding style.

* Service: [BasketService](..\..\src\ApplicationCore\Services\BasketService.cs) (covers lookup and orchestration, delegate all functionality to the aggregates)
* Aggregate: [Basket](..\..\src\ApplicationCore\Entities\BasketAggregate\Basket.cs) (covers rich domain logic)
* Repository: [IRepository](..\..\src\ApplicationCore\Interfaces\IRepository.cs) and [EfRepository](..\..\src\Infrastructure\Data\EfRepository.cs) (covers data access and persistence)
* Unit tests: [AddItemToBasket.cs](..\..\tests\UnitTests\ApplicationCore\Services\BasketServiceTests/AddItemToBasket.cs) Write ONLY unit tests in the style of this file. Unit tests cover both the service and aggregate functionality and use an in memory repository and fluent test data builders for maximum speed and clarity.

---

## Design Summary

This codebase follows **Clean Architecture** with **Domain-Driven Design (DDD)** principles. Business logic is distributed across three layers: Services orchestrate workflows, Aggregates encapsulate domain rules, and Repositories abstract data access. This separation ensures testability, maintainability, and clear boundaries between concerns.

## Service Layer

**Purpose:** Orchestrate use cases by coordinating between repositories and aggregates. Services contain no business logic themselves—they delegate all domain operations to aggregates.

**Code Style:**
- Constructor injection of dependencies (`IRepository<T>`, loggers)
- Async/await for all I/O operations
- Use Guard clauses for null checks (`Guard.Against.Null`)
- Return `Result<T>` for operations that may fail (e.g., `NotFound()`)
- Keep methods focused on orchestration: load aggregate via specification, call aggregate method, persist
- Example pattern: `var basket = await _basketRepository.FirstOrDefaultAsync(spec); basket.AddItem(...); await _basketRepository.UpdateAsync(basket);`

## Aggregate Layer

**Purpose:** Encapsulate business rules and invariants. Aggregates are the heart of domain logic and maintain consistency boundaries.

**Code Style:**
- Private setters on all properties to enforce encapsulation
- Private backing fields for collections (`_items`), exposed as `IReadOnlyCollection<T>`
- Constructor-based initialization (e.g., `new Basket(buyerId)`)
- Public methods with clear intent (e.g., `AddItem`, `RemoveEmptyItems`)
- No direct property manipulation—always use methods to maintain invariants
- Implement `IAggregateRoot` marker interface to identify aggregate roots
- Example: `Basket` prevents duplicate items by checking existence before adding, ensuring consistency

## Repository Layer

**Purpose:** Abstract data access and persistence. Repositories provide collection-like interfaces for aggregates without exposing database details.

**Code Style:**
- Generic interface `IRepository<T>` inherits from Ardalis.Specification's `IRepositoryBase<T>`
- Constrained to aggregate roots: `where T : class, IAggregateRoot`
- Use Specification pattern for queries (e.g., `BasketWithItemsSpecification`)
- EfRepository is a thin wrapper around Entity Framework Core with Ardalis.Specification
- Never expose `IQueryable`—always use specifications for filtering
- Unit tests use in-memory implementations (e.g., `InMemoryBasketRepository`) for speed

## Unit Tests

**Purpose:** Verify both service orchestration and aggregate business logic through single tests. By default only test all aggregate logic indirectly through service methods.

**Code Style:**
- One test class per use case (e.g., `AddItemToBasket` class for `AddItemToBasket` use case)
- Use **Fluent Test Data Builders** (e.g., `BasketBuilder`) to construct test entities
- In-memory repositories (no database, no mocks for repos)
- NSubstitute for mocking non-repo dependencies (e.g., loggers)
- Arrange-Act-Assert structure with clear sections
- Descriptive test names: `AddsNewItemToEmptyBasket`, `IncreasesQuantityWhenAddingExistingItem`
- Assert on both returned results AND persisted state
- Example pattern: `var basket = new BasketBuilder().WithBuyerId(id).Build(); await _repo.AddAsync(basket);`
- Builders use fluent chaining: `.WithBuyerId("test").WithItem(1, 1.50m).Build()`