# Coding Standards & Conventions

## Code Style (.editorconfig)
- **Indentation**: 4 spaces for C# code, 2 spaces for XML/config files
- **Encoding**: UTF-8 with BOM for C# files
- **Line Endings**: Insert final newline
- **Organizing Usings**: System directives first

## .NET Coding Conventions
- **this. preferences**: Avoid `this.` qualification (silent)
- **Type preferences**: Use language keywords over BCL types
- **Parentheses**: Always use for clarity in arithmetic/relational operators
- **Access modifiers**: Always specify explicitly

## Naming Conventions
- **Classes**: PascalCase (e.g., `BasketService`, `CatalogItem`)
- **Interfaces**: IPascalCase (e.g., `IRepository`, `IBasketService`)
- **Methods**: PascalCase (e.g., `GetBasketAsync`, `AddItemToBasket`)
- **Properties**: PascalCase
- **Fields**: camelCase with underscore prefix for private fields
- **Constants**: PascalCase (e.g., `AuthorizationConstants`)

## Architecture Conventions
- **Services**: End with "Service" (e.g., `BasketService`, `OrderService`)
- **Specifications**: End with "Specification" or "Spec"
- **Exceptions**: End with "Exception" (e.g., `BasketNotFoundException`)
- **Interfaces**: Mirror implementation names with "I" prefix

## File Organization
- **Entities**: Organized by aggregates in separate folders
- **Specifications**: Dedicated Specifications folder
- **Services**: Dedicated Services folder  
- **Interfaces**: Dedicated Interfaces folder
- **Extensions**: Dedicated Extensions folder

## Package Management
- **Central Package Management**: Uses `Directory.Packages.props`
- **Version Control**: Centralized package version management