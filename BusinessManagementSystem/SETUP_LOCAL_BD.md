# ?? GUÍA DE SETUP LOCAL CON BASE DE DATOS - A Y R Servicio Técnico

**Cómo ejecutar el sistema completo localmente con PostgreSQL**

---

## ?? Tabla de Contenidos

- [Requisitos Previos](#requisitos-previos)
- [Paso 1: Instalar PostgreSQL](#paso-1-instalar-postgresql)
- [Paso 2: Crear Database](#paso-2-crear-database)
- [Paso 3: Configurar Project](#paso-3-configurar-project)
- [Paso 4: Instalar EF Core](#paso-4-instalar-ef-core)
- [Paso 5: Crear Migrations](#paso-5-crear-migrations)
- [Paso 6: Ejecutar Aplicación](#paso-6-ejecutar-aplicación)
- [Paso 7: Verificar & Testear](#paso-7-verificar--testear)
- [Troubleshooting](#troubleshooting)

---

## ? Requisitos Previos

Asegúrate de tener:

```powershell
# Verificar .NET 8
dotnet --version
# Resultado: 8.0.x

# Verificar Git
git --version
# Resultado: git version x.x.x

# Tu proyecto ya clonado
cd "C:\Users\roque\source\repos\business-management-system-dotnet"
```

---

## ?? Paso 1: Instalar PostgreSQL

### Descarga

1. Ve a https://www.postgresql.org/download/windows/
2. Descarga **PostgreSQL 15 o superior**
3. O haz clic directo: [PostgreSQL 15.5 Installer](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)

### Instalación Paso a Paso

**Ejecutar el instalador:**

```
1. Doble clic en postgresql-xx-windows-x64.exe
2. Next > Next > Next
3. En "Password" ingresa una contraseña segura (ej: PostgresPass123)
4. Anotar esta contraseña (la necesitarás después)
5. Port: 5432 (dejar por defecto)
6. Next > Install > Finish
```

### Verificar Instalación

```powershell
# En PowerShell, ejecuta:
psql --version
# Resultado: psql (PostgreSQL) 15.x

# Conectarse a PostgreSQL
psql -U postgres
# Te pide contraseña (la que anotaste)
# Si funciona, deberías ver: postgres=#

# Salir:
\q
```

Si no funciona, agrega PostgreSQL al PATH:

```powershell
# Buscar donde se instaló PostgreSQL
# Generalmente: C:\Program Files\PostgreSQL\15\bin

# Agregar al PATH (temporal, solo esta sesión):
$env:Path += ";C:\Program Files\PostgreSQL\15\bin"

# O hacerlo permanente (buscar "Edit environment variables")
```

---

## ??? Paso 2: Crear Database

### Opción A: Usando pgAdmin (GUI - Recomendado)

**Abrir pgAdmin:**

```
1. Abre navegador: http://localhost:5050
2. Login con usuario: postgres (contraseña que anotaste)
3. En el panel izquierdo: Servers > PostgreSQL 15
4. Right-click > Create > Database
5. Nombre: ayr_servicio
6. Owner: postgres
7. Click Create
```

### Opción B: Usando comando (Terminal)

```powershell
# Conectarse como admin
psql -U postgres

# Dentro de psql:
postgres=# CREATE DATABASE ayr_servicio;
postgres=# \l
# Deberías ver: ayr_servicio | postgres | UTF8 | ...

# Salir
postgres=# \q
```

### Verificar Creación

```powershell
# Ver todas las bases de datos
psql -U postgres -l

# Deberías ver "ayr_servicio" en la lista
```

---

## ?? Paso 3: Configurar Project

### Crear Infrastructure Project

Primero, crearemos el proyecto Infrastructure que conecta la lógica con la BD:

```powershell
cd "C:\Users\roque\source\repos\business-management-system-dotnet"

# Crear carpeta Infrastructure
mkdir "src\Infrastructure"
mkdir "src\Infrastructure\BusinessManagementSystem.Infrastructure"

# Crear proyecto
cd "src\Infrastructure\BusinessManagementSystem.Infrastructure"
dotnet new classlib -n BusinessManagementSystem.Infrastructure

# Volver al root
cd "..\..\.."
```

### Configurar Proyecto Infrastructure

**Abrir el archivo .csproj:**

```xml
<!-- src/Infrastructure/BusinessManagementSystem.Infrastructure/BusinessManagementSystem.Infrastructure.csproj -->

<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\Application\BusinessManagementSystem.Application\BusinessManagementSystem.Application.csproj" />
  </ItemGroup>

</Project>
```

### Restaurar Dependencias

```powershell
cd "src/Infrastructure/BusinessManagementSystem.Infrastructure"
dotnet restore
```

---

## ?? Paso 4: Instalar EF Core

### Instalar NuGet Packages

```powershell
cd "src/Infrastructure/BusinessManagementSystem.Infrastructure"

# Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0

# PostgreSQL Provider
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0

# EF Core Tools (para migrations)
dotnet tool install --global dotnet-ef
```

### Verificar Instalación

```powershell
# Ver si dotnet-ef está instalado
dotnet ef --version
# Resultado: Entity Framework Core .NET Command-line Tools 8.0.0
```

Si falta, instálalo así:

```powershell
dotnet tool update --global dotnet-ef
```

---

## ?? Paso 5: Crear DbContext y Migrations

### Crear DbContext

**Archivo: `src/Infrastructure/BusinessManagementSystem.Infrastructure/Data/ApplicationDbContext.cs`**

```csharp
using Microsoft.EntityFrameworkCore;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Entidades principales
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Equipment> Equipment { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<WorkOrder> WorkOrders { get; set; } = null!;
        public DbSet<PartCatalogItem> PartCatalogItems { get; set; } = null!;
        public DbSet<WarrantyClaim> WarrantyClaims { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Phone).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Address).HasMaxLength(500);
            });

            // Configuración de Equipment
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
            });

            // Configuración de User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
            });

            // Configuración de WorkOrder
            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WorkOrderNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.WorkOrderNumber).IsUnique();
                entity.Property(e => e.Status).HasConversion<string>();
                entity.HasOne<Client>().WithMany().HasForeignKey("ClientId");
                entity.HasOne<Equipment>().WithMany().HasForeignKey("EquipmentId");
            });

            // Configuración de PartCatalogItem
            modelBuilder.Entity<PartCatalogItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.DefaultUnitPrice).HasPrecision(15, 2);
            });

            // Configuración de WarrantyClaim
            modelBuilder.Entity<WarrantyClaim>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<WorkOrder>().WithMany().HasForeignKey("OriginalWorkOrderId");
                entity.HasOne<WorkOrder>().WithMany().HasForeignKey("ClaimWorkOrderId");
            });
        }
    }
}
```

### Crear Migration Inicial

```powershell
cd "src/Infrastructure/BusinessManagementSystem.Infrastructure"

# Crear migration
dotnet ef migrations add InitialCreate --project . --startup-project . --context ApplicationDbContext

# Resultado: Migration creada en folder Migrations/
```

Si no puedes crear migration, asegúrate que:

```powershell
# 1. Estés en la carpeta correcta
pwd
# Resultado: C:\...\src\Infrastructure\BusinessManagementSystem.Infrastructure

# 2. DbContext exista
ls Data/ApplicationDbContext.cs

# 3. Intenta nuevamente
dotnet ef migrations add InitialCreate
```

---

## ?? Paso 6: Ejecutar Aplicación

### Actualizar Base de Datos

```powershell
cd "src/Infrastructure/BusinessManagementSystem.Infrastructure"

# Aplicar migration a la BD
dotnet ef database update

# Resultado: Build started... Applied migration 'xxxxxxx_InitialCreate'
```

### Agregar Connection String

**Crear archivo: `appsettings.json` en la carpeta raíz**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ayr_servicio;Username=postgres;Password=PostgresPass123;Application Name=AYR.Servicio.Tecnico;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

?? **Cambiar `PostgresPass123` por tu contraseña actual**

### Crear Aplicación Console para Testing

**Crear: `Program.cs` en raíz**

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BusinessManagementSystem.Infrastructure.Data;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Application.Clients;
using BusinessManagementSystem.Domain.Entities;

// Configurar
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found");

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseNpgsql(connectionString)
    .Build();

using var context = new ApplicationDbContext(options);

// Crear tablas si no existen
await context.Database.MigrateAsync();

Console.WriteLine("? Base de datos lista!");

// Testear: Crear cliente
var createClientCmd = new CreateClient.Command(
    fullName: "Juan García",
    phone: "0972123456",
    email: "juan@email.com",
    address: "Calle Principal 123"
);

// Mock repository (después lo reemplazaremos)
var clientId = Guid.NewGuid();
Console.WriteLine($"? Cliente creado con ID: {clientId}");

Console.WriteLine("\n? ¡Sistema funcionando correctamente!");
```

### Ejecutar

```powershell
# En la carpeta raíz del proyecto
dotnet run

# Resultado esperado:
# ? Base de datos lista!
# ? Cliente creado con ID: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
# ? ¡Sistema funcionando correctamente!
```

---

## ? Paso 7: Verificar & Testear

### Verificar en pgAdmin

```
1. Abrir http://localhost:5050
2. Login
3. Expandir: Servers > PostgreSQL 15 > Databases > ayr_servicio
4. Expandir: Schemas > public > Tables
5. Deberías ver todas tus tablas:
   - Clients
   - Equipment
   - Users
   - WorkOrders
   - PartCatalogItems
   - WarrantyClaims
```

### Ejecutar Query de Prueba

En pgAdmin, crear nueva query:

```sql
-- Verificar tablas
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
ORDER BY table_name;

-- Resultado esperado: Lista de todas tus tablas

-- Ver clientes
SELECT * FROM "Clients";

-- Contar registros
SELECT COUNT(*) as total_clientes FROM "Clients";
```

### Insertar Datos de Prueba

```sql
-- Insertar cliente de prueba
INSERT INTO "Clients" (
    "Id", "FullName", "Phone", "Email", "Address", "Observations", "CreatedAtUtc"
) VALUES (
    '550e8400-e29b-41d4-a716-446655440000',
    'García Juan',
    '0972123456',
    'juan@email.com',
    'Hipódromo, Asunción',
    'Cliente de prueba',
    NOW()
);

-- Verificar
SELECT * FROM "Clients" LIMIT 5;
```

---

## ?? Troubleshooting

### Error: "Connection refused"

```
Problema: PostgreSQL no está corriendo

Solución:
1. Verificar que PostgreSQL esté activo:
   - En Windows: Services > PostgreSQL

2. Iniciar PostgreSQL:
   - Control Panel > Services
   - Buscar "postgresql-x64-15"
   - Click derecho > Start

3. Verificar con:
   psql -U postgres
   (Si funciona, ya está corriendo)
```

### Error: "password authentication failed"

```
Problema: Contraseña incorrecta en connection string

Solución:
1. Verificar contraseña:
   - appsettings.json > Password

2. Resetear contraseña de postgres:
   - Abrir pgAdmin
   - Right-click en "Servers"
   - Properties > Connection > Password
   - Cambiar contraseña
   - Usar misma en appsettings.json
```

### Error: "database 'ayr_servicio' does not exist"

```
Problema: Database no fue creada

Solución:
1. Crear database manualmente:
   
   psql -U postgres
   postgres=# CREATE DATABASE ayr_servicio;
   postgres=# \q

2. O usando pgAdmin (ver Paso 2)
```

### Error: "No suitable constructor was found"

```
Problema: DbContext no está configurado correctamente

Solución:
1. Asegúrate que DbContext tenga:
   public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options) { }

2. Que esté en Microsoft.EntityFrameworkCore namespace

3. Ejecuta nuevamente:
   dotnet ef migrations add InitialCreate
```

### Error: "The entity type 'X' requires a primary key"

```
Problema: Alguna entidad no tiene Id

Solución:
1. Verificar que TODAS las entidades tengan:
   public Guid Id { get; set; }

2. Y que estén configuradas en DbContext:
   entity.HasKey(e => e.Id);
```

---

## ?? Estructura Final Esperada

```
business-management-system-dotnet/
?
??? BusinessManagementSystem/
?   ??? Domain/
?       ??? Entities/
?       ??? Enums/
?
??? src/
?   ??? Application/
?   ?   ??? BusinessManagementSystem.Application/
?   ?       ??? Abstractions/
?   ?       ??? Clients/
?   ?       ??? Users/
?   ?       ??? WorkOrders/
?   ?       ??? PartCatalog/
?   ?
?   ??? Infrastructure/          ? NUEVO
?       ??? BusinessManagementSystem.Infrastructure/
?           ??? Data/
?           ?   ??? ApplicationDbContext.cs
?           ?   ??? Migrations/
?           ??? Repositories/
?
??? appsettings.json            ? NUEVO
??? Program.cs                  ? NUEVO
??? (otros archivos)
```

---

## ?? Próximos Pasos

### Implementar Repositories

Una vez que la BD está funcionando:

```csharp
// Crear en Infrastructure/Repositories/

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task AddAsync(Client client, CancellationToken ct)
    {
        await _context.Clients.AddAsync(client, ct);
        await _context.SaveChangesAsync(ct);
    }

    // ... más métodos
}
```

### Registrar en DI Container

```csharp
// En Program.cs (cuando crees API):

services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
// etc.
```

---

## ? CHECKLIST

- [ ] PostgreSQL instalado y corriendo
- [ ] Database `ayr_servicio` creada
- [ ] Infrastructure project creado
- [ ] EF Core packages instalados
- [ ] DbContext creado
- [ ] Migration inicial ejecutada
- [ ] Base de datos actualizada
- [ ] Conexión verificada en pgAdmin
- [ ] Datos de prueba insertados
- [ ] Sistema listo para desarrollo

---

**¡Listo! Tu sistema está completamente funcional localmente.** ??

Próximo paso: [Crear API REST](./README.md)

