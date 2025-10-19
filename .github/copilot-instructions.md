# Copilot Instructions for eShopOnWeb

## Project Overview
- **eShopOnWeb** is a monolithic ASP.NET Core 9.0 reference app demonstrating Clean Architecture.
- Major layers: `ApplicationCore` (domain/business), `Infrastructure` (data/external), `Web` (MVC), `PublicApi` (FastEndpoints), `BlazorAdmin` (Blazor WASM), `BlazorShared` (shared models).
- Follows DDD, Repository, Specification, CQRS (MediatR), and Decorator patterns.

## Key Workflows
- **Restore/build:** `dotnet restore` then `dotnet build ./eShopOnWeb.sln`
- **Run Web app:** `cd src/Web && dotnet run --launch-profile https` (https://localhost:5001, admin: /admin)
- **Run PublicApi:** `cd src/PublicApi && dotnet run` (https://localhost:5200)
- **Run all with Docker:** `docker-compose build && docker-compose up`
- **Database setup:**
  - `cd src/Web`
  - `dotnet ef database update -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj`
  - `dotnet ef database update -c appidentitydbcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj`
- **Tests:**
  - All: `dotnet test ./eShopOnWeb.sln --collect:"XPlat Code Coverage"`
  - Unit: `tdd-guard-dotnet-test tests/UnitTests/UnitTests.csproj`
  - Integration: `tdd-guard-dotnet-test tests/IntegrationTests/IntegrationTests.csproj`
  - Functional: `tdd-guard-dotnet-test tests/FunctionalTests/FunctionalTests.csproj`

## Architecture & Patterns
- **Domain:** `ApplicationCore/Entities/`, aggregates: Basket, Order, Buyer, CatalogItem
- **Repositories:** `IRepository<T>` and specifications in `ApplicationCore`, implemented in `Infrastructure`
- **API:** `PublicApi/` uses FastEndpoints (see `README.md` there)
- **UI:** MVC in `Web/`, Blazor in `BlazorAdmin/`
- **Auth:** ASP.NET Core Identity, JWT, GitHub SSO, role-based
- **Config:** `appsettings.json` (env-specific overrides), set `UseOnlyInMemoryDatabase` for in-memory mode

## Conventions & Tips
- Use Windows-style paths
- Default login: `demouser@microsoft.com` (seeded)
- Health checks: `/health` endpoint
- Caching via decorator pattern (see `CachedCatalogViewModelService`)
- Use specifications for complex queries, not raw LINQ
- For new features: add entities/specs in `ApplicationCore`, repos in `Infrastructure`, endpoints in `PublicApi`, UI in `Web`/`BlazorAdmin`
- For DB changes: create migrations from `src/Web` using `dotnet ef migrations add ...`

## Troubleshooting
- If DB errors: ensure SQL Server is running or use in-memory DB
- For HTTPS dev: `dotnet dev-certs https --trust`
- Port issues: check `launchSettings.json`
- Missing deps: `dotnet restore` and `dotnet tool restore`

## References
- Main docs: `README.md`, `CLAUDE.md`, `src/PublicApi/README.md`
- eBook: https://aka.ms/webappebook
- Example: to add a new product, update `CatalogItem` in `ApplicationCore/Entities/`, add a migration, and expose via `PublicApi` endpoint.

### ⚠️ CRITICAL: Always Verify Before Returning Control
**Before completing any task and returning control to the user, you MUST:**
1. Ensure the code compiles without errors
2. Run `dotnet test .\tests\UnitTests\UnitTests.csproj --no-restore` to verify all unit tests pass
3. Fix any compilation errors or test failures before finishing

This is non-negotiable. Never leave the user with broken code or failing tests.

# Communication style

NEVER summarize what you did. Be very concise and to the point. No need to explain your actions unless the user specifically asks for it.

# Visual markers

Every type of instruction should use a visual marker that you include in all your responses. The default visual marker for general instructions in this project is: 🛒