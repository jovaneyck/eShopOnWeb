# Task Completion Checklist

## When Completing Development Tasks

### 1. Build Verification
```bash
# Always verify the solution builds successfully
dotnet build ./eShopOnWeb.sln --configuration Debug
```

### 2. Test Execution
```bash
# Run all tests to ensure no regressions
dotnet test ./eShopOnWeb.sln --no-restore --verbosity normal

# For specific test categories:
dotnet test tests/UnitTests/UnitTests.csproj          # Unit tests
dotnet test tests/IntegrationTests/IntegrationTests.csproj  # Integration tests  
dotnet test tests/FunctionalTests/FunctionalTests.csproj    # End-to-end tests
```

### 3. Code Quality Checks
- Verify code follows .editorconfig standards
- Ensure proper using statement organization
- Check for appropriate error handling
- Validate security best practices

### 4. Database Consistency
```bash
# If entity changes were made, ensure migrations are current
cd src/Web
dotnet ef database update -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
```

### 5. Architecture Compliance
- Verify dependency directions (Core → Infrastructure → UI)
- Ensure proper use of interfaces and abstractions
- Check specification pattern usage for complex queries
- Validate aggregate boundaries in DDD entities

### 6. Performance Considerations
- Leverage caching decorators where appropriate
- Use async/await patterns consistently
- Implement proper pagination for large datasets

### 7. Documentation Updates
- Update CLAUDE.md if new patterns or commands are introduced
- Ensure API documentation reflects changes (FastEndpoints auto-generates Swagger)

## Never Skip
- **Build verification**: Code must compile successfully
- **Test execution**: All tests must pass
- **Security review**: No secrets or keys in code