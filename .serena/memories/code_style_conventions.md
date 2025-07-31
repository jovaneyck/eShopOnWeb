# Code Style and Conventions

## EditorConfig Rules
- **Indentation**: Spaces (4 spaces for C# files, 2 for XML/config files)
- **Charset**: UTF-8 with BOM for C# files
- **Final newline**: Required
- **Organize usings**: System directives first
- **this. preferences**: Not required (disabled)

## C# Coding Standards
- Follow standard .NET naming conventions
- Use dependency injection through constructor parameters
- Implement interfaces for services
- Use async/await for I/O operations
- Include logging in service operations

## Service Layer Patterns
```csharp
public class ExampleService : IExampleService
{
    private readonly IRepository<Entity> _repository;
    private readonly IAppLogger<ExampleService> _logger;

    public ExampleService(IRepository<Entity> repository,
        IAppLogger<ExampleService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
```

## Architecture Patterns
- **Repository Pattern**: Use `IRepository<T>` for data access
- **Specification Pattern**: Use Ardalis.Specification for complex queries
- **Domain-Driven Design**: Aggregates with `IAggregateRoot` marker interface
- **CQRS**: Command/Query separation using MediatR
- **Decorator Pattern**: For caching services

## Package Management
- Central package management via `Directory.Packages.props`
- Target framework: `net9.0`
- Key packages: Ardalis.Specification, AutoMapper, MediatR, FastEndpoints

## Testing Conventions
- **Unit Tests**: Domain logic and services
- **Integration Tests**: Data layer with real database
- **Functional Tests**: End-to-end web testing
- **API Tests**: FastEndpoints testing
- Use xUnit framework