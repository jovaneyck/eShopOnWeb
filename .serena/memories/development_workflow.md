# Development Workflow & Guidelines

## XP Practices & TDD Approach
- **Test-Driven Development**: Write failing test first, make it pass, then refactor
- **Four Rules of Simple Design**: 
  1. Code passes tests
  2. Reveals intent (clear naming, structure)
  3. Minimizes duplication
  4. Has minimal set of elements
- **Tiny Steps**: Each step results in building, passing tests
- **Refactoring**: Continuous improvement of design

## Adding New Features Workflow

### 1. Domain Layer (ApplicationCore)
- Define entities in `ApplicationCore/Entities/`
- Create specifications in `ApplicationCore/Specifications/`
- Add service interfaces in `ApplicationCore/Interfaces/`
- Implement domain services in `ApplicationCore/Services/`

### 2. Data Layer (Infrastructure)
- Implement repository interfaces in `Infrastructure/Data/`
- Add EF configurations in `Infrastructure/Data/Config/`
- Create migrations if schema changes needed

### 3. API Layer (PublicApi)
- Create endpoints using FastEndpoints pattern
- Implement request/response models
- Add validation rules

### 4. UI Layer (Web/BlazorAdmin) 
- Implement MVC controllers/actions in Web
- Create Blazor components in BlazorAdmin
- Add view models and mapping logic

## Testing Strategy
- **Unit Tests**: Domain logic, services, specifications
- **Integration Tests**: Data layer with real database
- **Functional Tests**: End-to-end scenarios
- **API Tests**: Endpoint behavior validation

## Reference Lookup Guidelines
- Always search for existing implementations before creating new ones
- Check for references when modifying public/protected APIs
- Use existing patterns and libraries already in the solution
- Follow Clean Architecture dependency rules