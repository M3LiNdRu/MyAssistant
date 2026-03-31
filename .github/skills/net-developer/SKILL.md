---
name: net-developer
description: Expert .NET development guidance. Use when working with C#, ASP.NET Core, Entity Framework, .NET APIs, dependency injection, middleware, authentication, RESTful services, microservices, performance optimization, testing, LINQ, async/await, or any .NET framework task. Provides architectural patterns, best practices, and production-ready code.
---

# Senior .NET Developer Skill

## Workflow Instructions

**IMPORTANT**: When this skill is invoked for complex multi-file implementations, code generation, or architectural tasks:

1. **Use a subagent** to perform the actual implementation work to keep the main session context clean
2. **The subagent should**:
   - Read this entire SKILL.md file as the first action
   - Apply all principles, patterns, and best practices defined below
   - Implement the requested .NET features following the guidelines
   - Return a concise summary of what was implemented

3. **For simple tasks** (single file edits, quick answers, code explanations), work directly without a subagent

**When to use subagent**:
- Creating multiple new files (controllers, services, repositories)
- Refactoring across multiple files
- Implementing new features with tests
- Setting up project architecture
- Complex database migrations

**When to work directly**:
- Answering .NET questions
- Explaining code patterns
- Single file edits
- Quick bug fixes
- Code reviews

## Core Principles

### 1. Write Idiomatic C# Code
- Use **modern C# features** (records, pattern matching, nullable reference types, top-level statements where appropriate)
- Prefer **expression-bodied members** for simple properties and methods
- Use **var** when type is obvious from right side, explicit types when clarity is needed
- Follow **PascalCase** for public members, **camelCase** for private fields with `_` prefix
- Use **async/await** for all I/O operations; avoid `.Result` or `.Wait()`
- Leverage **LINQ** for collection operations instead of manual loops when readable

### 2. Dependency Injection & Configuration
- **Constructor injection only** for required dependencies
- Register services with appropriate lifetime:
  - `AddSingleton`: Stateless services, shared across all requests
  - `AddScoped`: Per-request services (DbContext, HTTP context-dependent)
  - `AddTransient`: Lightweight, stateless, created each time
- Use **IOptions<T>** pattern for configuration binding
- Never use Service Locator anti-pattern
- Keep constructors clean; avoid logic in constructors

### 3. API Development Best Practices

#### RESTful Design
- Use **conventional HTTP verbs**: GET (read), POST (create), PUT (update), DELETE (remove), PATCH (partial update)
- Return appropriate status codes:
  - `200 OK`: Successful GET/PUT/PATCH
  - `201 Created`: Successful POST with `Location` header
  - `204 No Content`: Successful DELETE
  - `400 Bad Request`: Validation errors
  - `401 Unauthorized`: Missing/invalid authentication
  - `403 Forbidden`: Authenticated but insufficient permissions
  - `404 Not Found`: Resource doesn't exist
  - `409 Conflict`: Business rule violation
  - `500 Internal Server Error`: Unhandled exceptions
- Use **plural nouns** for resource names: `/api/expenses`, `/api/categories`
- Version APIs: `/api/v1/expenses` or via headers

#### Request Validation
- Use **Data Annotations** for simple validation: `[Required]`, `[MaxLength]`, `[Range]`
- Use **FluentValidation** for complex validation rules
- Implement model validation in action filters or middleware
- Return **ProblemDetails** (RFC 7807) for error responses
- Validate at API boundary; don't trust input

#### Response Patterns
```csharp
// Success with data
return Ok(new { data = result, timestamp = DateTime.UtcNow });

// Created resource
return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

// Validation errors
return BadRequest(new ValidationProblemDetails(ModelState));

// Not found
return NotFound(new { message = $"Expense {id} not found" });
```

### 4. Async/Await Best Practices
- **Always use async all the way**: Don't mix sync and async code
- Use `ConfigureAwait(false)` in library code (not in ASP.NET Core)
- Return `Task<T>` not `Task<Task<T>>` - don't double-wrap
- Avoid `async void` except for event handlers
- Use `ValueTask<T>` for hot paths when result often available synchronously
- Use `IAsyncEnumerable<T>` for streaming large datasets

### 5. Error Handling & Logging

#### Exception Handling
```csharp
// Use exception filters for logging
[HttpPost]
public async Task<IActionResult> CreateExpense(ExpenseDto dto)
{
    try
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetExpense), new { id = result.Id }, result);
    }
    catch (ValidationException ex)
    {
        _logger.LogWarning(ex, "Validation failed for expense creation");
        return BadRequest(new { errors = ex.Errors });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error creating expense");
        return StatusCode(500, new { message = "An error occurred" });
    }
}
```

#### Structured Logging
```csharp
// Use structured logging with named parameters
_logger.LogInformation("Expense {ExpenseId} created by user {UserId}", expense.Id, userId);

// Log levels:
// Trace: Very detailed, development only
// Debug: Internal flow, development/staging
// Information: General flow, track requests
// Warning: Unusual but handled situations
// Error: Errors and exceptions
// Critical: Application/system failures
```

### 6. Performance Optimization

#### Database Access
- Use **projection** to select only needed columns
- Implement **pagination** for list endpoints (avoid returning all records)
- Use **AsNoTracking()** for read-only queries
- Batch operations when possible
- Use **compiled queries** for frequently executed queries
- Index foreign keys and frequently queried columns

#### Caching
```csharp
// In-memory cache for frequently accessed data
public async Task<Category> GetCategoryAsync(string id)
{
    var cacheKey = $"category_{id}";
    if (!_cache.TryGetValue(cacheKey, out Category category))
    {
        category = await _repository.GetByIdAsync(id);
        _cache.Set(cacheKey, category, TimeSpan.FromMinutes(10));
    }
    return category;
}
```

#### Response Compression
- Enable response compression middleware
- Configure for JSON, XML, text responses
- Use GZIP or Brotli

### 7. Security Best Practices

#### Authentication & Authorization
```csharp
// Use JWT bearer authentication
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

// Apply authorization policies
[Authorize(Policy = "RequireAdminRole")]
public async Task<IActionResult> DeleteExpense(string id)
```

#### Input Sanitization
- **Never trust user input**
- Use parameterized queries (prevents SQL injection)
- Validate and sanitize all inputs
- Use HTML encoding for output
- Implement rate limiting for public APIs
- Use CORS policies appropriately

#### Secrets Management
- **Never hardcode secrets** in source code
- Use User Secrets for development
- Use Azure Key Vault, AWS Secrets Manager, or similar for production
- Use environment variables for configuration, not appsettings.json

### 8. Testing Strategy

#### Unit Tests
```csharp
[Fact]
public async Task CreateExpense_ValidInput_ReturnsCreatedExpense()
{
    // Arrange
    var mockRepo = new Mock<IExpenseRepository>();
    var service = new ExpenseService(mockRepo.Object);
    var dto = new ExpenseDto { Amount = 100, Description = "Test" };
    
    // Act
    var result = await service.CreateAsync(dto);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(100, result.Amount);
    mockRepo.Verify(r => r.AddAsync(It.IsAny<Expense>()), Times.Once);
}
```

#### Integration Tests
- Use **WebApplicationFactory** for API testing
- Test against test database (not production)
- Use **TestContainers** for database integration tests
- Test authentication/authorization flows
- Verify HTTP status codes and response structure

#### Test Organization
- Arrange-Act-Assert pattern
- One assertion concept per test
- Use descriptive test names: `MethodName_Scenario_ExpectedBehavior`
- Mock external dependencies
- Use xUnit, NUnit, or MSTest consistently

### 9. Code Organization

#### Project Structure
```
Solution/
├── src/
│   ├── Api/                    # ASP.NET Core API
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── Filters/
│   │   └── Program.cs
│   ├── Application/            # Business logic, services
│   │   ├── Services/
│   │   ├── DTOs/
│   │   └── Interfaces/
│   ├── Domain/                 # Domain models, entities
│   │   ├── Entities/
│   │   └── ValueObjects/
│   └── Infrastructure/         # Data access, external services
│       ├── Repositories/
│       └── Data/
└── tests/
    ├── UnitTests/
    └── IntegrationTests/
```

#### Clean Architecture Principles
- **Separation of concerns**: Each layer has single responsibility
- **Dependency inversion**: Depend on abstractions, not concretions
- **Domain-driven design**: Rich domain models with behavior
- **Repository pattern**: Abstract data access
- **Service layer**: Orchestrate business operations

### 10. Modern .NET Patterns

#### Minimal APIs (Alternative to Controllers)
```csharp
app.MapGet("/api/expenses/{id}", async (string id, IExpenseService service) =>
{
    var expense = await service.GetByIdAsync(id);
    return expense is null ? Results.NotFound() : Results.Ok(expense);
});

app.MapPost("/api/expenses", async (ExpenseDto dto, IExpenseService service) =>
{
    var expense = await service.CreateAsync(dto);
    return Results.Created($"/api/expenses/{expense.Id}", expense);
});
```

#### Records for DTOs
```csharp
public record ExpenseDto(
    decimal Amount,
    string Description,
    string CategoryId,
    DateTime Date);

public record ExpenseResponse(
    string Id,
    decimal Amount,
    string Description,
    CategoryDto Category,
    DateTime CreatedAt);
```

#### Result Pattern (Instead of Exceptions for Business Logic)
```csharp
public record Result<T>(bool IsSuccess, T? Value, string? Error)
{
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
```

## Decision Making Framework

### When to use which pattern?

| Scenario | Recommended Pattern |
|----------|-------------------|
| Simple CRUD API | Controller with repository pattern |
| Complex business logic | CQRS with MediatR |
| High-performance APIs | Minimal APIs with Dapper |
| Real-time updates | SignalR with event-driven architecture |
| Long-running processes | Background services with Hosted Services |
| Microservices | Domain-driven design with message bus |
| File uploads | Stream processing with IFormFile |
| Bulk operations | Background jobs with Hangfire/Quartz |

### Database Access Patterns

| Use Case | Technology | When |
|----------|-----------|------|
| Relational data | Entity Framework Core | Complex relationships, rapid development |
| High-performance reads | Dapper | Read-heavy workloads, complex queries |
| Document storage | MongoDB with official driver | Flexible schema, nested documents |
| Caching | Redis/IMemoryCache | Frequently accessed data |
| Time-series data | InfluxDB/TimescaleDB | Metrics, logs, sensor data |

## Common Anti-Patterns to Avoid

❌ **Using `async void`** (except event handlers)
❌ **Blocking async code** with `.Result` or `.Wait()`
❌ **N+1 queries** (use eager loading or projection)
❌ **Fat controllers** (move logic to services)
❌ **Returning IQueryable from services** (expose implementation details)
❌ **Using `DateTime.Now`** (use `DateTime.UtcNow`)
❌ **Swallowing exceptions** without logging
❌ **Magic strings** (use constants or enums)
❌ **God objects** (violates Single Responsibility Principle)
❌ **Primitive obsession** (use value objects for domain concepts)

## Quick Checklist

Before completing any .NET task, verify:

- [ ] All I/O operations are async
- [ ] Appropriate HTTP status codes returned
- [ ] Input validation implemented
- [ ] Proper error handling and logging
- [ ] Dependencies injected via constructor
- [ ] No hardcoded configuration values
- [ ] Nullable reference types considered
- [ ] XML documentation on public APIs
- [ ] Unit tests written for business logic
- [ ] Follows SOLID principles
- [ ] Uses appropriate data structures
- [ ] No memory leaks (dispose IDisposable)
- [ ] Thread-safe when needed
- [ ] Performance considered (async, caching, pagination)
- [ ] Security vulnerabilities checked

## References

- [Microsoft .NET Documentation](https://docs.microsoft.com/dotnet/)
- [C# Coding Conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [ASP.NET Core Best Practices](https://docs.microsoft.com/aspnet/core/fundamentals/best-practices)
- [Clean Architecture in .NET](https://github.com/jasontaylordev/CleanArchitecture)