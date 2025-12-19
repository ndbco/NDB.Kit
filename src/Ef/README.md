# NDB.Kit.Ef – Entity Framework Utilities Guide

`NDB.Kit.Ef` provides **safe, opinionated utilities** on top of **Entity Framework Core**.

Its purpose is to:

* Reduce boilerplate
* Enforce safe defaults
* Keep application handlers thin
* Bridge infrastructure concerns (EF, Audit) with application needs

This package does **NOT** contain business logic.

---

## DESIGN GOALS

`NDB.Kit.Ef` is designed to be:

* ✅ Infrastructure-friendly
* ✅ Application-agnostic
* ✅ Safe by default
* ✅ Explicit, not magical
* ❌ Not a repository pattern replacement
* ❌ Not a UnitOfWork abstraction

---

## CORE PRINCIPLES

1. **EF Core semantics are preserved**

   * Exceptions are NOT swallowed
   * SaveChanges remains exception-based

2. **Query helpers are explicit**

   * No dynamic LINQ strings
   * No reflection without guard

3. **Audit is observability**

   * Audit does not determine success or failure
   * Audit is optional

4. **Handlers stay thin**

   * Query mechanics live here
   * Business logic stays in application layer

---

## QUERY ENTRY POINT

### `Query<TEntity>()`

Default entry point for **read-only queries**.

```csharp
var query = _db.Roles.Query();
```

### Behavior

* Applies `AsNoTracking()`
* Prevents accidental tracking
* Encourages explicit write intent

---

## AUDIT-AWARE SAVE OPERATIONS

### `SaveWithAuditAsync`

Save changes and write audit logs if audit service is registered.

```csharp
await _db.SaveWithAuditAsync(ct);
```

### Characteristics

* Uses `IAuditService` if available
* Throws exception on failure
* Does NOT return audit entries

---

### `SaveWithAuditResultAsync`

Save changes and **return audit entries** in a safe wrapper.

```csharp
var result = await _db.SaveWithAuditResultAsync(ct);

if (!result.Success)
{
    // result.Exception is available
}

var auditEntries = result.AuditEntries;
```

### `AuditSaveResult<T>`

```csharp
public sealed record AuditSaveResult<T>(
    bool Success,
    string Message,
    Exception? Exception,
    IReadOnlyList<T> AuditEntries);
```

### Important Rules

* `Success == true` → Save completed without exception
* `AuditEntries` describe **what changed**, not success
* Failure is always exception-based internally

---

## PAGING HELPERS

### `ToPagedResultAsync`

Convert an `IQueryable<TEntity>` into a `PagedResult<TDto>`.

```csharp
return await query.ToPagedResultAsync<Role, RoleResponse>(
    request.Paging,
    _mapper,
    ct);
```

### Behavior

* Executes `CountAsync`
* Applies `Skip` / `Take`
* Uses AutoMapper `ProjectTo`
* Returns immutable `PagedResult<T>`

---

### Selector-based overload

```csharp
return await query.ToPagedResultAsync(
    request.Paging,
    entity => new RoleResponse { ... },
    ct);
```

Use when AutoMapper is not desired.

---

## LIST HELPERS

### `ToListResultAsync`

```csharp
return await _db.Roles
    .Query()
    .ToListResultAsync<Role, RoleResponse>(_mapper, ct);
```

Returns `ListResult<T>` with:

* Items
* TotalCount

---

## FILTERING HELPERS

### `ApplyFilters`

```csharp
query = query.ApplyFilters(
    request.Filters,
    RoleQueryMap.AllowedFilters);
```

### Characteristics

* Uses expression trees
* Field names must be whitelisted
* Invalid filters are ignored
* No dynamic LINQ execution

Supported operators:

* Equals
* Contains
* StartsWith
* EndsWith
* GreaterThan
* LessThan

---

## SORTING HELPERS

### `ApplySorts`

```csharp
query = query.ApplySorts(
    request.Sorts,
    RoleQueryMap.AllowedSorts);
```

### Characteristics

* Supports multiple sort rules
* Uses `OrderBy / ThenBy`
* Field whitelist enforced
* Invalid sorts are ignored

---

## GLOBAL SEARCH

### `ApplySearch`

```csharp
query = query.ApplySearch(
    request.Search,
    RoleQueryMap.SearchableFields);
```

### Characteristics

* Applies keyword search across whitelisted fields
* String-only fields
* Case-insensitive (implementation dependent)
* Skipped if keyword is null or empty

---

## TRACKING GUARDS

### `EnsureDetached`

```csharp
_db.EnsureDetached(entity);
```

Ensures entity is not tracked by the DbContext.

---

### `EnsureTracked`

```csharp
_db.EnsureTracked(entity);
```

Ensures entity is attached and tracked.

---

## TYPICAL QUERY PIPELINE

```csharp
return await _db.Roles
    .Query()
    .ApplySearch(request.Search, RoleQueryMap.SearchableFields)
    .ApplyFilters(request.Filters, RoleQueryMap.AllowedFilters)
    .ApplySorts(request.Sorts, RoleQueryMap.AllowedSorts)
    .ToPagedResultAsync<Role, RoleResponse>(
        request.Paging,
        _mapper,
        ct);
```

---

## ERROR HANDLING STRATEGY

* EF Core exceptions are **not swallowed**
* Application layer handles exception-to-result mapping
* `SaveWithAuditResultAsync` provides optional wrapper only

---

## ANTI-PATTERNS (DO NOT USE)

* ❌ Repository pattern on top of DbContext
* ❌ UnitOfWork abstraction
* ❌ Dynamic string-based LINQ
* ❌ Swallowing exceptions in infrastructure
* ❌ AutoMapper mapping Result objects

---

## PACKAGE RESPONSIBILITIES

### This package DOES:

* Provide safe EF helpers
* Reduce boilerplate
* Enforce query consistency
* Integrate audit safely

### This package DOES NOT:

* Contain business rules
* Perform authorization
* Define HTTP behavior
* Replace EF Core concepts

---

## SUMMARY

`NDB.Kit.Ef` is the **official EF utility layer** for the NDB ecosystem.

It ensures:

* Safe querying
* Clean handlers
* Explicit audit integration
* Consistent paging and filtering
* Long-term maintainability

---

If you are using:

* **NDB.Abstraction.Requests** → input contracts
* **NDB.Abstraction.Results** → output contracts
* **NDB.Audit.EF** → observability

Then **NDB.Kit.Ef** is the **glue layer** that makes everything clean and predictable.
