# ??? Infrastructure Layer - A Y R Servicio Técnico

**Capa de infraestructura con Entity Framework Core y PostgreSQL**

---

## ?? Contenido

```
src/Infrastructure/BusinessManagementSystem.Infrastructure/
?
??? Data/
?   ??? ApplicationDbContext.cs       ? DbContext principal
?
??? Repositories/
?   ??? ClientRepository.cs           ? Implementación del repositorio de clientes
?   ??? WorkOrderRepository.cs        ? Implementación del repositorio de órdenes
?   ??? UserRepository.cs             ? Implementación del repositorio de usuarios
?   ??? PartCatalogRepository.cs      ? Implementación del repositorio de catálogo
?   ??? WarrantyClaimRepository.cs    ? Implementación del repositorio de garantías
?
??? Extensions/
?   ??? ServiceCollectionExtensions.cs ? Configuración de DI
?
??? Migrations/                        ? Migraciones de base de datos (generadas por EF Core)
    ??? [timestamp]_InitialCreate.cs
    ??? ApplicationDbContextModelSnapshot.cs
```

---

## ?? Cómo Usar

### 1. Crear la Primera Migración

```powershell
# Posicionarse en la carpeta Infrastructure
cd "src/Infrastructure/BusinessManagementSystem.Infrastructure"

# Crear migration inicial
dotnet ef migrations add InitialCreate --project . --context ApplicationDbContext

# Resultado: Se crea carpeta Migrations/ con archivos de migración
```

### 2. Aplicar Migraciones a la Base de Datos

```powershell
# Desde la carpeta Infrastructure
dotnet ef database update --context ApplicationDbContext

# O desde la raíz
dotnet ef database update --project "src/Infrastructure/BusinessManagementSystem.Infrastructure" --context ApplicationDbContext
```

### 3. Usar en Program.cs (API)

```csharp
// En Program.cs del API
using BusinessManagementSystem.Infrastructure.Extensions;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Agregar infraestructura
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

// Aplicar migraciones al iniciar
await app.Services.ApplyMigrationsAsync();

// Verificar conexión
var connected = await app.Services.VerifyDatabaseConnectionAsync();
if (!connected)
    throw new InvalidOperationException("No se puede conectar a la base de datos");

app.Run();
```

### 4. Configurar appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ayr_servicio;Username=postgres;Password=PostgresPass123;Application Name=AYR.Servicio.Tecnico;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

---

## ?? DbContext: ApplicationDbContext

### Características Principales

```csharp
? 6 DbSets (Clients, Equipment, Users, WorkOrders, PartCatalogItems, WarrantyClaims)
? Configuración completa de relaciones
? Índices para performance
? Tipos de propiedad personalizados (Owned Types)
? Constraints y validaciones
? Conversión de Enums a string
? Timestamps automáticos (UTC)
```

### Owned Types Configurados

```csharp
// WorkOrder contiene estos owned types:
- Accessories (colección)
- Parts (colección)
- Diagnosis (opcional)
- Quote (opcional)
- ServiceReport (opcional)
```

---

## ?? Repositorios Implementados

### 1. ClientRepository

```csharp
public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Client?> GetByPhoneAsync(string phone, CancellationToken ct);
    Task<IEnumerable<Client>> GetAllAsync(CancellationToken ct);
    Task AddAsync(Client client, CancellationToken ct);
    Task UpdateAsync(Client client, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
```

**Características:**
- Búsqueda por ID y teléfono
- Listado de todos
- CRUD completo

---

### 2. WorkOrderRepository

```csharp
public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<WorkOrder?> GetByNumberAsync(string workOrderNumber, CancellationToken ct);
    Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken ct);
    Task<IEnumerable<WorkOrder>> GetByClientAsync(Guid clientId, CancellationToken ct);
    Task<IEnumerable<WorkOrder>> GetByMechanicAsync(Guid mechanicUserId, CancellationToken ct);
    Task<IEnumerable<WorkOrder>> GetUnderWarrantyAsync(DateTime nowLocal, CancellationToken ct);
    Task AddAsync(WorkOrder workOrder, CancellationToken ct);
    Task UpdateAsync(WorkOrder workOrder, CancellationToken ct);
}
```

**Características:**
- Búsqueda múltiple (ID, número, estado, cliente, mecánico)
- Filtrado por garantía
- Eager loading de relaciones

---

### 3. UserRepository

```csharp
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<User>> GetByRoleAsync(UserRole role, CancellationToken ct);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
```

**Características:**
- Búsqueda por email
- Filtrado por rol
- Filtrado por estado activo

---

### 4. PartCatalogRepository

```csharp
public interface IPartCatalogRepository
{
    Task<PartCatalogItem?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<PartCatalogItem?> GetByNameAsync(string name, CancellationToken ct);
    Task<IEnumerable<PartCatalogItem>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<PartCatalogItem>> GetActiveAsync(CancellationToken ct);
    Task AddAsync(PartCatalogItem item, CancellationToken ct);
    Task UpdateAsync(PartCatalogItem item, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
```

**Características:**
- Búsqueda por nombre único
- Filtrado por estado activo
- CRUD completo

---

### 5. WarrantyClaimRepository

```csharp
public interface IWarrantyClaimRepository
{
    Task<WarrantyClaim?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<WarrantyClaim>> GetByOriginalWorkOrderAsync(Guid originalWorkOrderId, CancellationToken ct);
    Task<IEnumerable<WarrantyClaim>> GetByClaimWorkOrderAsync(Guid claimWorkOrderId, CancellationToken ct);
    Task<IEnumerable<WarrantyClaim>> GetAllAsync(CancellationToken ct);
    Task AddAsync(WarrantyClaim claim, CancellationToken ct);
    Task UpdateAsync(WarrantyClaim claim, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
```

**Características:**
- Búsqueda por órdenes de trabajo
- Historial de reclamaciones

---

## ?? Configuración de Entity Framework Core

### DbContext Setup

```csharp
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
{
}
```

### Configuración en Program.cs

```csharp
services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(30);
        npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
    });
});
```

### Características Activadas

```
? Retry automático en conexión fallida
? Timeout de 30 segundos
? Logging en desarrollo
? Sensitive data logging en desarrollo
? Migrations automáticas
```

---

## ?? Diseño de Base de Datos

### Tablas Principales

```
???????????????
?  Clients    ?
???????????????
? Id (PK)     ?
? FullName    ?
? Phone (UNQ) ?
? Email       ?
? Address     ?
? CreatedAtUtc?
???????????????

???????????????
?  Equipment  ?
???????????????
? Id (PK)     ?
? Type        ?
? Brand       ?
? Model       ?
? SerialNumber?
? CreatedAtUtc?
???????????????

???????????????
?   Users     ?
???????????????
? Id (PK)     ?
? FullName    ?
? Email (UNQ) ?
? PasswordHash?
? Role (Enum) ?
? IsActive    ?
? CreatedAtUtc?
???????????????

????????????????????
?  WorkOrders      ?
????????????????????
? Id (PK)          ?
? WorkOrderNumber  ?
? ClientId (FK)    ?
? EquipmentId (FK) ?
? Status (Enum)    ?
? CreatedAtUtc     ?
? ... más campos   ?
????????????????????

???????????????????
? PartCatalogItem ?
???????????????????
? Id (PK)         ?
? Name (UNQ)      ?
? Description     ?
? DefaultUnitPrice?
? IsActive        ?
? CreatedAtUtc    ?
???????????????????

??????????????????
?  WarrantyClaim ?
??????????????????
? Id (PK)        ?
? OriginalWOId   ?
? ClaimWOId      ?
? Reason         ?
? CreatedAtUtc   ?
??????????????????
```

### Tablas de Owned Types

```
????????????????????
? WorkOrderAccessories
????????????????????
? Id (PK)          ?
? WorkOrderId (FK) ?
? Name             ?
? IsPresent        ?
? Condition        ?
????????????????????

????????????????????
? WorkOrderParts   ?
????????????????????
? Id (PK)          ?
? WorkOrderId (FK) ?
? Name             ?
? Quantity         ?
? UnitPrice        ?
????????????????????

????????????????????
? WorkOrderDiagnosis
????????????????????
? WorkOrderId (FK) ?
? Findings         ?
? RecommendedWork  ?
? Notes            ?
? CreatedAtUtc     ?
????????????????????

????????????????????
? WorkOrderQuote   ?
????????????????????
? WorkOrderId (FK) ?
? LaborCost        ?
? PartsTotal       ?
? Notes            ?
? CreatedAtUtc     ?
????????????????????

????????????????????????
? WorkOrderServiceReport
????????????????????????
? WorkOrderId (FK)     ?
? WorkPerformed        ?
? Recommendations      ?
? Notes                ?
? CreatedAtUtc         ?
????????????????????????
```

---

## ?? Índices Creados

Para optimizar performance:

```sql
-- Clientes
CREATE UNIQUE INDEX idx_client_phone ON "Clients"("Phone");
CREATE INDEX idx_client_email ON "Clients"("Email");
CREATE INDEX idx_client_fullname ON "Clients"("FullName");

-- Equipos
CREATE INDEX idx_equipment_type ON "Equipment"("Type");
CREATE INDEX idx_equipment_serial ON "Equipment"("SerialNumber");

-- Usuarios
CREATE UNIQUE INDEX idx_user_email ON "Users"("Email");
CREATE INDEX idx_user_role ON "Users"("Role");
CREATE INDEX idx_user_active ON "Users"("IsActive");

-- Órdenes
CREATE UNIQUE INDEX idx_workorder_number ON "WorkOrders"("WorkOrderNumber");
CREATE INDEX idx_workorder_status ON "WorkOrders"("Status");
CREATE INDEX idx_workorder_createdat ON "WorkOrders"("CreatedAtUtc");
CREATE INDEX idx_workorder_clientid ON "WorkOrders"("ClientId");
CREATE INDEX idx_workorder_mechanicid ON "WorkOrders"("AssignedMechanicUserId");

-- Repuestos
CREATE UNIQUE INDEX idx_partcatalog_name ON "PartCatalogItems"("Name");
CREATE INDEX idx_partcatalog_active ON "PartCatalogItems"("IsActive");

-- Accesorios
CREATE INDEX idx_accessory_workorderid ON "WorkOrderAccessories"("WorkOrderId");

-- Repuestos de OT
CREATE INDEX idx_part_workorderid ON "WorkOrderParts"("WorkOrderId");

-- Garantías
CREATE INDEX idx_warrantyclaim_original ON "WarrantyClaims"("OriginalWorkOrderId");
CREATE INDEX idx_warrantyclaim_claim ON "WarrantyClaims"("ClaimWorkOrderId");
```

---

## ? Checklist de Configuración

- [ ] Crear migration inicial
- [ ] Aplicar migraciones a BD
- [ ] Registrar repositorios en DI
- [ ] Verificar conexión a BD
- [ ] Probar CRUD operations
- [ ] Verificar índices en BD
- [ ] Probar relaciones entre entidades
- [ ] Documentación actualizada

---

## ?? Próximos Pasos

1. **Crear API REST** ? Controllers para cada repositorio
2. **Implementar Autenticación** ? JWT tokens
3. **Crear Tests de Integración** ? Testing de repositorios
4. **Agregar Logging** ? Serilog o similar

---

**¡Infrastructure lista! ??**
