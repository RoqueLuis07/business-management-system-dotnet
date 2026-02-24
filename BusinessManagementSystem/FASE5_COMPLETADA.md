# ?? PRÓXIMOS PASOS - FASE 5 COMPLETADA

**Infrastructure Layer está lista. ¡Ahora qué sigue!**

---

## ? LO QUE SE COMPLETÓ

```
? DbContext completado (ApplicationDbContext.cs)
? 5 Repositorios concretos implementados
? 6 DbSets configurados
? Owned types mapeados
? Índices de base de datos
? DI Configuration lista
? README.md con instrucciones
? Compilación 100% exitosa
```

---

## ?? PASOS INMEDIATOS (Semana 1 - Parte 2)

### PASO 1: Crear la Primera Migración

```powershell
cd "src/Infrastructure/BusinessManagementSystem.Infrastructure"

# Crear migration
dotnet ef migrations add InitialCreate --context ApplicationDbContext

# Resultado: Se crea carpeta Migrations/ con archivos
```

### PASO 2: Aplicar Migración a la Base de Datos

Primero, asegúrate de que PostgreSQL esté corriendo:

```powershell
# Verificar que PostgreSQL está activo
psql -U postgres -c "SELECT 1"

# Si funciona, aplicar migraciones
dotnet ef database update --context ApplicationDbContext

# Resultado: Tablas creadas en PostgreSQL
```

### PASO 3: Verificar que Funcionó

Conéctate a pgAdmin:

```
http://localhost:5050
Usuario: postgres
Contraseña: (la tuya)
```

Deberías ver:

```
ayr_servicio
??? Schemas > public > Tables
    ??? "Clients"
    ??? "Equipment"
    ??? "Users"
    ??? "WorkOrders"
    ??? "WorkOrderAccessories"
    ??? "WorkOrderParts"
    ??? "WorkOrderDiagnosis"
    ??? "WorkOrderQuote"
    ??? "WorkOrderServiceReport"
    ??? "PartCatalogItems"
    ??? "WarrantyClaims"
    ??? "__EFMigrationsHistory"
```

---

## ?? SEMANA 2: CREAR API REST

Una vez que la BD esté lista:

### Crear API Project

```powershell
cd "src"
dotnet new web -n BusinessManagementSystem.API

cd BusinessManagementSystem.API

# Agregar referencias
dotnet add reference "..\..\BusinessManagementSystem\BusinessManagementSystem.Domain"
dotnet add reference "..\Application\BusinessManagementSystem.Application"
dotnet add reference "..\Infrastructure\BusinessManagementSystem.Infrastructure"

# Agregar paquetes necesarios
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package System.IdentityModel.Tokens.Jwt
```

### Crear Controllers

```csharp
// src/API/BusinessManagementSystem.API/Controllers/ClientsController.cs

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientRepository _repository;

    [HttpPost]
    public async Task<IActionResult> CreateClient(CreateClient.Command cmd)
    {
        await CreateClient.HandleAsync(_repository, cmd, CancellationToken.None);
        return Ok(new { success = true });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(Guid id)
    {
        var client = await _repository.GetByIdAsync(id, CancellationToken.None);
        if (client is null)
            return NotFound();
        return Ok(client);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        var result = await GetAllClients.HandleAsync(_repository, CancellationToken.None);
        return Ok(result);
    }
}
```

---

## ?? REFERENCIA RÁPIDA

### Archivo de Configuración (appsettings.json)

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

### Program.cs Setup

```csharp
using BusinessManagementSystem.Infrastructure.Extensions;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

// Aplicar migraciones
await app.Services.ApplyMigrationsAsync();

// Verificar conexión
var connected = await app.Services.VerifyDatabaseConnectionAsync();
if (!connected)
    throw new InvalidOperationException("No se puede conectar a BD");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

---

## ?? TESTING INFRASTRUCTURE

### Crear Integration Tests

```powershell
mkdir tests\BusinessManagementSystem.Infrastructure.Tests
cd tests\BusinessManagementSystem.Infrastructure.Tests

dotnet new xunit
dotnet add package xunit
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory

dotnet add reference "..\..\src\Infrastructure\BusinessManagementSystem.Infrastructure"
```

### Test Example

```csharp
[Fact]
public async Task ClientRepository_CreatesAndRetrievesClient()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    using var context = new ApplicationDbContext(options);
    var repository = new ClientRepository(context);
    var client = new Client("Juan", "0972123456", "Calle");

    // Act
    await repository.AddAsync(client, CancellationToken.None);
    var retrieved = await repository.GetByIdAsync(client.Id, CancellationToken.None);

    // Assert
    retrieved.Should().NotBeNull();
    retrieved.FullName.Should().Be("Juan");
}
```

---

## ?? TROUBLESHOOTING

### Error: "Connection refused"

```powershell
# Verificar que PostgreSQL está corriendo
pg_isready -h localhost -p 5432

# Si no está, iniciar:
# Windows: Services > PostgreSQL > Start
# Linux: sudo systemctl start postgresql
```

### Error: "No migrations"

```powershell
# Asegúrate que estés en la carpeta correcta
cd "src/Infrastructure/BusinessManagementSystem.Infrastructure"

# Crear migration
dotnet ef migrations add InitialCreate
```

### Error: "Schema does not exist"

```powershell
# La base de datos debe existir
# Crearla con pgAdmin o:
psql -U postgres -c "CREATE DATABASE ayr_servicio"
```

---

## ?? CHECKLIST FASE 5

- [ ] Infrastructure project creado
- [ ] DbContext implementado
- [ ] 5 Repositorios creados
- [ ] Compilación exitosa ?
- [ ] PostgreSQL instalado
- [ ] Base de datos creada
- [ ] Migration creada
- [ ] Migration aplicada a BD
- [ ] Tablas verificadas en pgAdmin
- [ ] appsettings.json configurado
- [ ] DI setup verificado
- [ ] Repositorios testeados

---

## ?? PRÓXIMOS PASOS

### Semana 2: API REST
- [ ] Crear API project
- [ ] Crear Controllers
- [ ] Swagger setup
- [ ] JWT Authentication

### Semana 3: Testing
- [ ] Unit tests completados
- [ ] Integration tests completados
- [ ] 85%+ coverage

### Semana 4: Frontend Web
- [ ] Blazor project
- [ ] Layouts responsivos
- [ ] CRUD básico

---

## ?? TIPS

1. **Usar In-Memory DB para tests**
   ```csharp
   .UseInMemoryDatabase("test-db")
   ```

2. **Logging en desarrollo**
   ```csharp
   options.LogTo(Console.WriteLine)
   ```

3. **Retry automático**
   ```csharp
   npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3)
   ```

4. **Async/Await siempre**
   ```csharp
   public async Task<T> GetAsync(...) { ... }
   ```

---

## ?? CONTACTO Y RECURSOS

- EF Core Docs: https://docs.microsoft.com/ef
- PostgreSQL: https://www.postgresql.org
- Migration Guide: https://docs.microsoft.com/ef/core/managing-schemas/migrations

---

**¡Fase 5 COMPLETADA! ??**

**Ahora puedes:**
- ? Crear órdenes en BD
- ? Recuperar datos con repositorios
- ? Usar lógica de Domain layer
- ? Construir API REST encima

**Próximo objetivo: API REST funcional (Semana 2)**
