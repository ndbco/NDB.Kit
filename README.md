# NDB.Kit

**Application productivity kit for modern .NET applications.**

NDB.Kit is a lightweight helper library that sits **above infrastructure**
and **below business logic**, designed to reduce repetitive boilerplate
in clean and modular .NET architectures.

This library is intentionally opinionated but **framework-agnostic**.

---

## What does NDB.Kit provide?

NDB.Kit bundles several **small but powerful building blocks**:

- AutoMapper convention-based mapping
- EF Core guardrails and audit integration
- Enum and primitive parsing helpers
- Guard clauses for validation
- Collection and string utilities
- Result helpers integrated with `NDB.Abstraction`

All modules are **optional** and can be used independently.

---

## Design Principles

- No business logic
- No UI or HTTP concerns
- No DAL replacement
- Safe defaults (e.g. `AsNoTracking`)
- Explicit, readable, and testable
- Open-source friendly

---

## Installation

```bash
dotnet add package NDB.Kit
```
## Modules Overview
### Mapping (AutoMapper)

Define mappings by implementing simple marker interfaces.
```code
using NDB.Kit.Mapping;
using AutoMapper;

public class VehicleResponse : IMapFrom<Vehicle>
{
    public Guid Id { get; set; }
    public string EngineNumber { get; set; }

    public void Mapping(IMappingExpression<Vehicle, object> map)
    {
        map.ForMember(d => d.EngineNumber,
            opt => opt.MapFrom(s => s.EngineNumber.Trim()));
    }
}
```

Register once:
```code
services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new AutoMappingProfile());
}, Assembly.GetExecutingAssembly());
```

### EF Core Helpers
Default AsNoTracking queries
```
var vehicles = await db.Query<Vehicle>()
    .Where(x => x.Status == Status.Active)
    .ToListAsync();

Save with audit (if NDB.Audit.EF is installed)
await db.SaveWithAuditAsync();
```
Audit is executed automatically when IAuditService is registered.

### Guard Clauses
```code
Guard.AgainstNull(request);
Guard.AgainstEmpty(request.EngineNumber, nameof(request.EngineNumber));
Guard.AgainstDefault(request.CompanyId, nameof(request.CompanyId));
```

### Enum Parsing
```code
var status = EnumHelper.ParseOrDefault(
    request.Status,
    ProductionStatusEnum.Unknown);
```

### Primitive Parsing

```code
var lot = Parse.Int(request.Lot);
var amount = Parse.Decimal(request.Amount);
var id = Parse.Guid(request.Id);
```

Returns nullable values instead of throwing.

### Collection Helpers
```code
foreach (var (index, item) in items.WithIndex())
{
    Console.WriteLine($"{index}. {item}");
}
```

### String Normalization
```code
var code = StringNormalize.Normalize(
    input,
    removeWhitespace: true,
    upper: true);
```

### Result Helpers (NDB.Abstraction)
```code
return ResultGuard.NotFoundIfNull(vehicle, "Vehicle not found");

return ResultGuard.FailIf(
    quantity <= 0,
    "Quantity must be greater than zero");
```

## What NDB.Kit is NOT
- Not a DAL
- Not a Web framework
- Not a UI helper library
- Not a validation framework
- Not a replacement for EF Core, AutoMapper, or MediatR

## Related Libraries
- NDB.Abstraction
Request, result, validation, and common contracts.

- NDB.Audit.EF
EF Core audit trail with old/new values and actor tracking.

Each library is independent and can be used separately.

## Final Notes
If you are building a clean architecture, modular monolith, or
enterprise-grade .NET system, NDB.Kit helps you stay productive
without sacrificing clarity or control.
