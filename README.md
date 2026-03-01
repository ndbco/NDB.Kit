# NDB.Kit

> Productivity toolkit for modern .NET applications.
> Provides EF Core helpers, AutoMapper conventions, Excel export, guards, parsing utilities, and common development extensions.

---

## Overview

`NDB.Kit` is a practical utility library designed to reduce boilerplate and enforce safer defaults in enterprise .NET systems.

It complements `NDB.Abstraction` by providing implementation helpers for:

* EF Core query composition (filter, sort, search, paging)
* EF Core audit integration
* AutoMapper auto-registration
* Excel export via OpenXML
* Guard clauses
* Enum & primitive parsing
* Result flow helpers
* String normalization
* Base64 utilities
* Identifier generation

This library focuses on **developer productivity and consistency**, not business logic.

---

# Package Structure

```id="m0n8r1"
NDB.Kit
 ├── Base64
 ├── Collections
 ├── Ef
 ├── Enums
 ├── Excel
 ├── Guards
 ├── Identifiers
 ├── Mapping
 ├── Primitives
 ├── Results
 └── Text
```

---

# EF Core Helpers

## 1. Safer Query Entry

```csharp
context.Query<TEntity>();
```

Applies `AsNoTracking()` by default for read-only operations.

---

## 2. Filtering

```csharp
query.ApplyFilters(filters, allowedFields);
```

* Uses expression trees
* Ignores unknown fields
* Supports:

  * Equals
  * Contains
  * StartsWith
  * EndsWith
  * GreaterThan
  * LessThan

Designed to integrate with `FilterRequest` from `NDB.Abstraction`.

---

## 3. Search

```csharp
query.ApplySearch(keyword, searchableFields);
```

* Case-insensitive
* Works on string properties only
* Builds dynamic OR expressions

---

## 4. Sorting

```csharp
query.ApplySorts(sorts, allowedFields);
```

* Supports multi-column sort
* Dynamically builds OrderBy / ThenBy
* Ignores unknown fields

---

## 5. Paging Integration

### With AutoMapper

```csharp
await query.ToPagedResultAsync<TEntity, TDto>(
    paging,
    mapper,
    ct);
```

### With manual selector

```csharp
await query.ToPagedResultAsync<TEntity, TDto>(
    paging,
    entity => new TDto(...),
    ct);
```

Returns `PagedResult<T>` from `NDB.Abstraction`.

---

## 6. List Result

```csharp
await query.ToListResultAsync<TEntity, TDto>(
    mapper,
    ct);
```

---

## 7. Audit Integration

### Save with audit

```csharp
await context.SaveWithAuditAsync();
```

### Save with audit result

```csharp
var result = await context.SaveWithAuditResultAsync();
```

Returns:

```csharp
AuditSaveResult<AuditEntry>
```

Requires `IAuditService` registration (from NDB.Audit.EF).

---

## 8. Tracking Guards

```csharp
context.EnsureDetached(entity);
context.EnsureTracked(entity);
```

Helps control EF Core tracking behavior explicitly.

---

# AutoMapper Conventions

`AutoMapping.Apply()` automatically scans assemblies and registers mapping profiles based on interfaces:

* `IMapFrom<TModel, TEntity>`
* `IMapTo<TEntity, TSource>`
* `IMapObject<TSource, TDestination>`

Example:

```csharp
public class UserDto : IMapFrom<UserDto, User>
{
    public void Mapping(IMappingExpression<User, UserDto> map)
    {
        map.ForMember(...);
    }
}
```

Then register:

```csharp
AutoMapping.Apply(cfg, typeof(Startup).Assembly);
```

Removes the need for manual profile classes.

---

# Excel Export (OpenXML)

Attribute-based export system.

## Step 1: Annotate DTO

```csharp
public class ReportDto
{
    [ExcelColumn("Name", Order = 1)]
    public string Name { get; set; }

    [ExcelColumn("Amount", Order = 2)]
    public decimal Amount { get; set; }
}
```

## Step 2: Export

```csharp
var bytes = exporter.Export(data, "Report");
```

## Features

* Header styling
* Body styling
* Numeric support
* DateTime OADate support
* Order validation
* Duplicate order detection

### Service registration

```csharp
services.AddNdbExcel();
```

---

# Guard Clauses

```csharp
Guard.AgainstNull(value, nameof(value));
Guard.AgainstEmpty(name, nameof(name));
Guard.AgainstDefault(id, nameof(id));
Guard.AgainstNegative(number, nameof(number));
```

Helps enforce early validation in application services.

---

# Result Flow Helpers

```csharp
ResultGuard.NotFoundIfNull(entity, "User not found");

ResultGuard.FailIf(condition,
    ResultStatus.BadRequest,
    "Invalid state");
```

Integrates with `NDB.Abstraction.Result`.

---

# Primitive Parsing Helpers

```csharp
Parse.Int("123");
Parse.Decimal("100.50");
Parse.Guid("...");
Parse.Bool("true");
Parse.DateTime("2024-01-01");
```

Returns nullable types instead of throwing exceptions.

---

# Enum Helpers

```csharp
EnumHelper.TryParse<MyEnum>(value, out var result);

EnumHelper.ParseOrDefault(value, MyEnum.Default);

EnumHelper.ParseOrThrow(value);
```

---

# String Normalization

```csharp
StringNormalize.Normalize(input,
    removeWhitespace: true,
    upper: true);
```

Removes control characters and trims input safely.

---

# Base64 Helpers

```csharp
Base64Helper.ToBytes(base64String);

Base64Helper.FromBytes(bytes, "image/png");
```

Supports Data URI format automatically.

---

# Identifier Generator

```csharp
var id = IdGenerator.NewId();
```

Returns uppercase, no-dash GUID string.

---

# Localization Utilities (Indonesian Focus)

Includes helpers for:

* Rupiah formatting
* Thousand formatting
* Terbilang (number to Indonesian words)
* Indonesian month names

Useful for government, finance, and reporting systems.

---

# Intended Usage

```id="r9f2od"
Application Layer
    ↓
NDB.Abstraction (Contracts)
    ↓
NDB.Kit (Implementation Helpers)
    ↓
Infrastructure
```

---

# Non-Goals

This library does not:

* Replace EF Core
* Replace AutoMapper
* Replace FluentValidation
* Contain domain business logic
* Manage dependency injection automatically

It provides structured utilities only.

---

# Versioning Policy

* MAJOR → Breaking behavior changes
* MINOR → New utilities / extensions
* PATCH → Fixes & internal improvements

---

# Dependencies

Depending on feature usage:

* Microsoft.EntityFrameworkCore
* AutoMapper
* DocumentFormat.OpenXml
* NDB.Abstraction
* NDB.Audit.EF (optional for audit integration)

---

# License

Choose your preferred license (MIT recommended).

---

# Maintained By

Navigate Digital Boundaries (NDB)
