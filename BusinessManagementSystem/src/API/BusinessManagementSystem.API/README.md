# ?? API REST - A Y R Servicio Técnico

**ASP.NET Core 8 REST API con Swagger/OpenAPI**

---

## ?? Contenido

```
src/API/BusinessManagementSystem.API/
?
??? Controllers/
?   ??? ClientsController.cs        ? CRUD de clientes
?   ??? WorkOrdersController.cs     ? Gestión de órdenes
?   ??? UsersController.cs          ? Gestión de usuarios
?   ??? PartCatalogController.cs    ? Catálogo de repuestos
?
??? DTOs/
?   ??? ApiDtos.cs                  ? Modelos de datos para API
?
??? Program.cs                       ? Configuración principal
??? appsettings.json                 ? Configuración de BD y logging
??? BusinessManagementSystem.API.csproj
```

---

## ?? Endpoints Implementados

### Clientes

```
GET    /api/clients                  ? Obtener todos
GET    /api/clients/{id}             ? Obtener uno
POST   /api/clients                  ? Crear
PUT    /api/clients/{id}             ? Actualizar
DELETE /api/clients/{id}             ? Eliminar
```

### Órdenes de Trabajo

```
GET    /api/workorders              ? Obtener todas
GET    /api/workorders/{id}         ? Obtener una
POST   /api/workorders              ? Crear
POST   /api/workorders/{id}/diagnosis           ? Registrar diagnóstico
POST   /api/workorders/{id}/start-repair       ? Iniciar reparación
POST   /api/workorders/{id}/service-report     ? Registrar reporte
GET    /api/workorders/client/{clientId}       ? Por cliente
GET    /api/workorders/mechanic/{mechanicId}   ? Por mecánico
```

### Usuarios

```
GET    /api/users                   ? Obtener todos
GET    /api/users/{id}              ? Obtener uno
GET    /api/users/email/{email}     ? Por email
GET    /api/users/active            ? Solo activos
```

### Repuestos

```
GET    /api/partcatalog             ? Obtener todos
GET    /api/partcatalog/active      ? Solo activos
GET    /api/partcatalog/{id}        ? Por ID
GET    /api/partcatalog/name/{name} ? Por nombre
```

---

## ?? Configuración

### 1. appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ayr_servicio;Username=postgres;Password=PostgresPass123;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

### 2. Program.cs Setup

```csharp
// Infrastructure (Repositorios + DbContext)
builder.Services.AddInfrastructure(connectionString);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", ...);
});

// Aplicar migraciones
await app.Services.ApplyMigrationsAsync();
```

---

## ?? DTOs (Data Transfer Objects)

### Request DTOs

```csharp
// Cliente
CreateClientDto(
    string FullName,
    string Phone,
    string Email,
    string Address)

// Orden
CreateWorkOrderDto(
    string WorkOrderNumber,
    Guid ClientId,
    Guid EquipmentId,
    string RequestedWorkDescription)

// Diagnóstico
SetDiagnosisDto(
    string Findings,
    string RecommendedWork,
    string? Notes,
    Guid MechanicUserId)
```

### Response DTOs

```csharp
// Success
SuccessResponse<T>(
    bool Success,
    T? Data,
    string Message)

// Error
ErrorResponse(
    int StatusCode,
    string Message,
    string? Details)
```

---

## ?? Cómo Ejecutar

### 1. Instalar Dependencias

```powershell
cd "src/API/BusinessManagementSystem.API"
dotnet restore
```

### 2. Ejecutar API

```powershell
dotnet run

# Output:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:5001
#       Now listening on: http://localhost:5000
```

### 3. Acceder a Swagger

```
http://localhost:5000
```

Verás interfaz interactiva donde puedes:
- Ver todos los endpoints
- Probar las APIs
- Ver esquemas de datos
- Descargar OpenAPI spec

---

## ?? Autenticación (JWT)

### Swagger Security Definition

Swagger está configurado con soporte para Bearer tokens:

```csharp
options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    In = ParameterLocation.Header,
    Description = "Please enter a valid token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "Bearer"
});
```

### Usar Token en Swagger

1. Hacer login para obtener token
2. Click en "Authorize" button
3. Ingresar: `Bearer {token}`
4. Hacer requests autenticados

---

## ?? Ejemplos de Uso

### Crear Cliente

```bash
curl -X POST http://localhost:5000/api/clients \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Juan García",
    "phone": "0972123456",
    "email": "juan@email.com",
    "address": "Calle Principal 123"
  }'
```

**Respuesta:**
```json
{
  "success": true,
  "data": null,
  "message": "Cliente creado exitosamente"
}
```

### Obtener Todos los Clientes

```bash
curl http://localhost:5000/api/clients
```

**Respuesta:**
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "fullName": "Juan García",
      "phone": "0972123456",
      "email": "juan@email.com",
      "address": "Calle Principal 123",
      "createdAtUtc": "2024-02-23T10:30:00Z"
    }
  ],
  "message": "Clientes obtenidos"
}
```

### Crear Orden de Trabajo

```bash
curl -X POST http://localhost:5000/api/workorders \
  -H "Content-Type: application/json" \
  -d '{
    "workOrderNumber": "OT-2024-001",
    "clientId": "550e8400-e29b-41d4-a716-446655440000",
    "equipmentId": "550e8400-e29b-41d4-a716-446655440001",
    "requestedWorkDescription": "Reparación de motosierra"
  }'
```

### Registrar Diagnóstico

```bash
curl -X POST http://localhost:5000/api/workorders/550e8400-e29b-41d4-a716-446655440002/diagnosis \
  -H "Content-Type: application/json" \
  -d '{
    "findings": "Bujía rota",
    "recommendedWork": "Cambiar bujía y revisar filtro",
    "notes": "Cliente menciona que no enciende desde hace 2 días",
    "mechanicUserId": "550e8400-e29b-41d4-a716-446655440003"
  }'
```

---

## ?? Response Format

Todas las respuestas siguen este formato:

```json
{
  "success": true,
  "data": { /* datos */ },
  "message": "Descripción de lo que ocurrió"
}
```

O en caso de error:

```json
{
  "statusCode": 400,
  "message": "El nombre del cliente es obligatorio",
  "details": null
}
```

---

## ??? Troubleshooting

### Error: "Connection refused"

```powershell
# Verificar que PostgreSQL está corriendo
psql -U postgres -c "SELECT 1"

# Si no, iniciar PostgreSQL
# Windows: Services > PostgreSQL > Start
```

### Error: "Database not found"

```powershell
# Verificar que la BD existe
psql -U postgres -l | grep ayr_servicio

# Si no existe, crearla
psql -U postgres -c "CREATE DATABASE ayr_servicio"
```

### Error: "Migrations not applied"

```powershell
# Las migraciones se aplican automáticamente al iniciar
# Pero si no funcionó:
dotnet ef database update
```

---

## ?? Próximos Pasos

1. **Agregar Autenticación JWT**
   - Login endpoint
   - Refresh tokens
   - Role-based authorization

2. **Agregar Validaciones**
   - FluentValidation
   - Custom validators
   - Error handling

3. **Agregar Logging**
   - Serilog
   - Structured logging
   - Request/Response logging

4. **Agregar Tests**
   - Integration tests
   - Controller tests
   - E2E tests

---

## ?? Notas Importantes

```
? Controllers inyectan repositorios
? Métodos son async
? Logging en todos los endpoints
? Error handling global
? DTOs para cada operación
? Swagger completamente documentado
? CORS habilitado para testing
? Health check en startup
```

---

## ?? Referencias

- [ASP.NET Core docs](https://docs.microsoft.com/aspnet/core)
- [Swagger/OpenAPI](https://swagger.io)
- [REST Best Practices](https://restfulapi.net)

---

**¡API REST lista! ??**
