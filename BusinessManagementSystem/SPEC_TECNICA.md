# ?? ESPECIFICACIÓN TÉCNICA - A Y R Servicio Técnico

**Documento técnico detallado del sistema**

---

## 1. INTRODUCCIÓN

Este documento describe las especificaciones técnicas completas del sistema de gestión para A Y R Servicio Técnico.

### 1.1 Audiencia
- Desarrolladores
- Arquitectos de software
- DevOps / Administradores de sistemas
- QA / Testers

### 1.2 Documentos Relacionados
- [REQUISITOS.md](./REQUISITOS.md) - Requisitos funcionales
- [README.md](./README.md) - Guía general
- [GUIA_DE_USO_CASOS_DE_USO.md](./GUIA_DE_USO_CASOS_DE_USO.md) - Ejemplos de código

---

## 2. ARQUITECTURA GENERAL

### 2.1 Diagrama de Capas

```
???????????????????????????????????????????????????????
?  Presentación (Web)                                 ?
?  Blazor Server / React                              ?
???????????????????????????????????????????????????????
                          ?
                          ? HTTP/REST
                          ?
???????????????????????????????????????????????????????
?  API Layer (ASP.NET Core 8)                        ?
?  • REST Controllers                                 ?
?  • DTOs                                             ?
?  • Validation Middleware                            ?
?  • Exception Handling                               ?
?  • Authentication (JWT)                             ?
???????????????????????????????????????????????????????
                          ?
                          ? Casos de Uso
                          ?
???????????????????????????????????????????????????????
?  Application Layer ? (Implementado)                ?
?  • 47 Casos de Uso                                  ?
?  • Commands & Queries                               ?
?  • Repository Interfaces                            ?
?  • Mappers / DTOs                                   ?
???????????????????????????????????????????????????????
                          ?
                          ? Repository
                          ?
???????????????????????????????????????????????????????
?  Domain Layer ? (Implementado)                     ?
?  • 12 Entidades                                     ?
?  • Lógica de Negocio Pura                           ?
?  • Value Objects                                    ?
?  • Validaciones                                     ?
???????????????????????????????????????????????????????
                          ?
                          ? EF Core
                          ?
???????????????????????????????????????????????????????
?  Infrastructure Layer (Por Implementar)             ?
?  • EF Core DbContext                                ?
?  • Repository Implementations                       ?
?  • Database Migrations                              ?
?  • PostgreSQL Driver                                ?
???????????????????????????????????????????????????????
                          ?
                          ? SQL
                          ?
???????????????????????????????????????????????????????
?  PostgreSQL Database                                ?
?  • Tables                                           ?
?  • Índices                                          ?
?  • Constraints                                      ?
?  • Backups                                          ?
???????????????????????????????????????????????????????
```

### 2.2 Patrones Arquitectónicos

| Patrón | Implementación | Estado |
|--------|----------------|--------|
| **Domain-Driven Design (DDD)** | Entidades + Agregados | ? |
| **Clean Architecture** | Separación de capas | ? |
| **Repository Pattern** | Abstracción de persistencia | ? |
| **CQRS-like** | Commands & Queries | ? |
| **Dependency Injection** | IoC Container | ? Futuro |
| **Unit of Work** | Transacciones | ? Futuro |
| **Specification Pattern** | Consultas complejas | ? Futuro |

---

## 3. COMPONENTES DEL SISTEMA

### 3.1 Domain Layer (Implementado ?)

#### Entidades
```
??? Client                          - Cliente del taller
??? Equipment                       - Equipo a reparar
??? User                            - Usuario del sistema
??? WorkOrder                       - Orden de trabajo (AGREGADO RAÍZ)
?   ??? WorkOrderAccessory         - Accesorios del equipo
?   ??? WorkOrderPart              - Repuestos utilizados
?   ??? WorkOrderDiagnosis         - Diagnóstico
?   ??? WorkOrderQuote             - Presupuesto
?   ??? WorkOrderServiceReport     - Reporte de trabajo
?   ??? WarrantyClaim              - Reclamo de garantía
??? PartCatalogItem                - Catálogo de repuestos
??? WarrantyClaim                  - Registro de garantía
```

#### Enumeraciones
```
??? UserRole
?   ??? Admin
?   ??? Receptionist (Nuevo)
?   ??? Mechanic
??? WorkOrderStatus
    ??? Recibido (1)
    ??? EnDiagnostico (2)
    ??? PresupuestoPendiente (3)
    ??? PresupuestoAprobado (4)
    ??? PresupuestoRechazado (5)
    ??? EnReparacion (6)
    ??? Finalizado (7)
    ??? Entregado (8)
```

#### Métodos de Negocio (30+ métodos)
- WorkOrder: Crear, asignar, diagnosticar, presupuestar, reparar, entregar
- Client: Crear, actualizar, eliminar
- User: Crear, cambiar rol, activar, desactivar
- PartCatalogItem: Crear, preciar, activar, desactivar

### 3.2 Application Layer (Implementado ?)

#### Casos de Uso Implementados: 47

**WorkOrders: 27**
- 3 Creación/Base
- 2 Diagnóstico
- 4 Repuestos
- 3 Presupuesto
- 3 Reparación
- 2 Entrega
- 2 Accesorios
- 2 Garantía
- 3 Consultas avanzadas

**Clientes: 5**
- CRUD completo + lista

**Usuarios: 9**
- CRUD + roles + activ/desac

**Catálogo: 8**
- CRUD + activ/desac + consultas

**Garantías: 3**
- Consultas especializadas

#### Interfaces de Repositorio: 5
```
??? IWorkOrderRepository
?   ??? GetByIdAsync
?   ??? GetByNumberAsync
?   ??? GetAllAsync
?   ??? GetByStatusAsync
?   ??? GetByClientAsync
?   ??? GetByMechanicAsync
?   ??? GetUnderWarrantyAsync
?   ??? AddAsync
?   ??? UpdateAsync
?
??? IClientRepository
?   ??? CRUD estándar
?   ??? Consultas
?
??? IUserRepository
?   ??? CRUD estándar
?   ??? Búsquedas
?
??? IPartCatalogRepository
?   ??? CRUD estándar
?   ??? Filtros
?
??? IWarrantyClaimRepository
    ??? GetByIdAsync
    ??? GetByOriginalWorkOrderAsync
    ??? GetAllAsync
    ??? AddAsync
    ??? UpdateAsync
```

### 3.3 Infrastructure Layer (Por Implementar ?)

#### Entity Framework Core Setup
```csharp
DbContext : DbContext
{
    DbSet<Client> Clients { get; set; }
    DbSet<Equipment> Equipment { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<WorkOrder> WorkOrders { get; set; }
    DbSet<PartCatalogItem> PartCatalog { get; set; }
    DbSet<WarrantyClaim> WarrantyClaims { get; set; }
}
```

#### Implementación de Repositorios
- WorkOrderRepository
- ClientRepository
- UserRepository
- PartCatalogRepository
- WarrantyClaimRepository

#### Migrations
- Migration 001: Schema inicial
- Migration 002: Índices y constraints
- Migration 003: Data seed (usuarios, categorías)

### 3.4 API Layer (Por Implementar ?)

#### Controllers Planeados: 5

**WorkOrdersController**
```
GET    /api/workorders
GET    /api/workorders/{id}
POST   /api/workorders
PUT    /api/workorders/{id}
GET    /api/workorders/search
GET    /api/workorders/client/{clientId}
GET    /api/workorders/mechanic/{mechanicId}
GET    /api/workorders/warranty
```

**ClientsController**
```
GET    /api/clients
GET    /api/clients/{id}
POST   /api/clients
PUT    /api/clients/{id}
DELETE /api/clients/{id}
```

**UsersController**
```
GET    /api/users
POST   /api/users/login
POST   /api/users/register
PUT    /api/users/{id}/role
GET    /api/users/mechanics
```

**PartCatalogController**
```
GET    /api/parts
GET    /api/parts/{id}
POST   /api/parts
PUT    /api/parts/{id}
DELETE /api/parts/{id}
```

**WarrantyClaimsController**
```
GET    /api/warranties
GET    /api/warranties/{id}
POST   /api/warranties
GET    /api/warranties/original/{workOrderId}
```

---

## 4. ESPECIFICACIONES DE BASE DE DATOS

### 4.1 Motor Seleccionado: PostgreSQL

| Aspecto | Especificación |
|--------|----------------|
| **Versión Mínima** | 12.0 |
| **Versión Recomendada** | 14+ |
| **Charset** | UTF-8 |
| **Timezone** | America/Asuncion |

### 4.2 Esquema de Base de Datos

#### Tablas Principales (Planeadas)

```sql
-- Usuarios
CREATE TABLE Users (
    Id UUID PRIMARY KEY,
    FullName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    IsActive BOOLEAN DEFAULT true,
    CreatedAtUtc TIMESTAMP NOT NULL,
    UpdatedAtUtc TIMESTAMP
);

-- Clientes
CREATE TABLE Clients (
    Id UUID PRIMARY KEY,
    FullName VARCHAR(255) NOT NULL,
    Phone VARCHAR(20) UNIQUE NOT NULL,
    Email VARCHAR(255),
    Address VARCHAR(500),
    Observations TEXT,
    CreatedAtUtc TIMESTAMP NOT NULL
);

-- Equipos
CREATE TABLE Equipment (
    Id UUID PRIMARY KEY,
    Type VARCHAR(100) NOT NULL,
    Brand VARCHAR(100) NOT NULL,
    Model VARCHAR(100) NOT NULL,
    SerialNumber VARCHAR(100),
    IsIdentified BOOLEAN,
    CreatedAtUtc TIMESTAMP NOT NULL
);

-- Órdenes de Trabajo
CREATE TABLE WorkOrders (
    Id UUID PRIMARY KEY,
    WorkOrderNumber VARCHAR(50) UNIQUE NOT NULL,
    ClientId UUID NOT NULL FOREIGN KEY,
    EquipmentId UUID NOT NULL FOREIGN KEY,
    Status VARCHAR(50) NOT NULL,
    RequestedWorkDescription TEXT NOT NULL,
    AssignedMechanicUserId UUID FOREIGN KEY,
    DeliveredAtLocal TIMESTAMP,
    WarrantyDays INT DEFAULT 30,
    CreatedAtUtc TIMESTAMP NOT NULL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (EquipmentId) REFERENCES Equipment(Id),
    FOREIGN KEY (AssignedMechanicUserId) REFERENCES Users(Id)
);

-- Diagnósticos
CREATE TABLE WorkOrderDiagnoses (
    Id UUID PRIMARY KEY,
    WorkOrderId UUID NOT NULL,
    Findings TEXT NOT NULL,
    RecommendedWork TEXT NOT NULL,
    Notes TEXT,
    MechanicUserId UUID NOT NULL,
    CreatedAtUtc TIMESTAMP NOT NULL,
    FOREIGN KEY (WorkOrderId) REFERENCES WorkOrders(Id),
    FOREIGN KEY (MechanicUserId) REFERENCES Users(Id)
);

-- Presupuestos
CREATE TABLE WorkOrderQuotes (
    Id UUID PRIMARY KEY,
    WorkOrderId UUID NOT NULL,
    LaborCost DECIMAL(15,2) NOT NULL,
    PartsTotal DECIMAL(15,2) NOT NULL,
    Total DECIMAL(15,2) NOT NULL,
    Notes TEXT,
    CreatedByUserId UUID NOT NULL,
    CreatedAtUtc TIMESTAMP NOT NULL,
    FOREIGN KEY (WorkOrderId) REFERENCES WorkOrders(Id),
    FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);

-- Repuestos
CREATE TABLE WorkOrderParts (
    Id UUID PRIMARY KEY,
    WorkOrderId UUID NOT NULL,
    PartName VARCHAR(255) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(15,2),
    LineTotal DECIMAL(15,2),
    CreatedAtUtc TIMESTAMP NOT NULL,
    FOREIGN KEY (WorkOrderId) REFERENCES WorkOrders(Id)
);

-- Catálogo de Repuestos
CREATE TABLE PartCatalogItems (
    Id UUID PRIMARY KEY,
    Name VARCHAR(255) UNIQUE NOT NULL,
    DefaultUnitPrice DECIMAL(15,2) NOT NULL,
    IsActive BOOLEAN DEFAULT true,
    CreatedAtUtc TIMESTAMP NOT NULL
);

-- Garantías
CREATE TABLE WarrantyClaims (
    Id UUID PRIMARY KEY,
    OriginalWorkOrderId UUID NOT NULL,
    ClaimWorkOrderId UUID NOT NULL,
    Reason TEXT NOT NULL,
    CreatedByUserId UUID NOT NULL,
    CreatedAtUtc TIMESTAMP NOT NULL,
    FOREIGN KEY (OriginalWorkOrderId) REFERENCES WorkOrders(Id),
    FOREIGN KEY (ClaimWorkOrderId) REFERENCES WorkOrders(Id),
    FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);
```

### 4.3 Índices Recomendados

```sql
-- Búsquedas frecuentes
CREATE INDEX idx_workorders_status ON WorkOrders(Status);
CREATE INDEX idx_workorders_client ON WorkOrders(ClientId);
CREATE INDEX idx_workorders_mechanic ON WorkOrders(AssignedMechanicUserId);
CREATE INDEX idx_clients_phone ON Clients(Phone);
CREATE INDEX idx_users_email ON Users(Email);
CREATE INDEX idx_workorders_number ON WorkOrders(WorkOrderNumber);
```

### 4.4 Constraints y Validaciones

```sql
-- Garantías solo en OT entregadas
ALTER TABLE WarrantyClaims
ADD CONSTRAINT check_warranty_entregada
CHECK (EXISTS (
    SELECT 1 FROM WorkOrders 
    WHERE id = OriginalWorkOrderId 
    AND Status = 'Entregado'
));

-- Teléfono único en clientes
ALTER TABLE Clients
ADD CONSTRAINT unique_phone UNIQUE(Phone);

-- Email único en usuarios
ALTER TABLE Users
ADD CONSTRAINT unique_email UNIQUE(Email);
```

---

## 5. ESPECIFICACIONES DE SEGURIDAD

### 5.1 Autenticación (Futuro)
- **Tipo**: JWT (JSON Web Tokens)
- **Duración**: 2 horas (configurable)
- **Refresh Token**: 7 días
- **Encriptación**: HS256 (mínimo)

### 5.2 Autorización
- **Modelo**: Role-Based Access Control (RBAC)
- **Roles**: Admin, Receptionist, Mechanic (extensible)
- **Políticas**: Por endpoint

### 5.3 Protección de Datos
- **Contraseñas**: Bcrypt o Argon2
- **Datos Sensibles**: Encriptación en tránsito (HTTPS)
- **Teléfonos/Emails**: Validación y sanitización
- **Logs**: Sin información sensible

### 5.4 Auditoría
- **Registro de cambios**: Por usuario
- **Timestamp**: Todas las operaciones
- **Trazabilidad**: Quién, qué, cuándo

---

## 6. ESPECIFICACIONES DE DESPLIEGUE

### 6.1 Requisitos del Servidor

**Mínimo:**
- CPU: Intel i5 o equivalente
- RAM: 8GB
- Disco: SSD 500GB
- S.O.: Windows Server 2019+ o Ubuntu 20.04+

**Recomendado:**
- CPU: Intel i7 o Ryzen 7
- RAM: 16GB
- Disco: SSD 1TB
- S.O.: Ubuntu 22.04 LTS

### 6.2 Stack de Despliegue

**Windows:**
```
IIS 10+ 
??? .NET 8 Hosting Bundle
??? PostgreSQL 14+
??? SSL Certificate
```

**Linux:**
```
Nginx/Apache
??? .NET 8 Runtime
??? PostgreSQL 14+
??? SSL Certificate (Let's Encrypt)
??? Systemd service
```

### 6.3 Docker (Futuro)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 5000
```

---

## 7. ESPECIFICACIONES DE INTEGRACIONES

### 7.1 WhatsApp API

**Proveedor**: Meta (Facebook)

**Configuración:**
```
API Version: v18.0+
Endpoints:
- POST /messages (enviar)
- GET /messages (webhook)
Rate Limit: 1000 msg/día (configurable)
```

**Eventos:**
- Equipo recibido
- Presupuesto generado
- Equipo reparado
- Listo para retiro

### 7.2 Email (SMTP)

**Servidor**: Por definir
- **Local**: Postfix/Sendmail
- **Cloud**: SendGrid, MailChimp, etc.

**Configuración:**
```
SMTP Host: [servidor]
SMTP Port: 587 (TLS)
Username: [usuario]
Password: [contraseña encriptada]
From: noreply@ayrserviciotecnico.py
```

### 7.3 PDF Generation

**Librería**: QuestPDF

**Especificaciones:**
```csharp
Document
??? Header
?   ??? Logo
?   ??? Empresa info
??? Body
?   ??? OT number
?   ??? Cliente info
?   ??? Diagnóstico
?   ??? Presupuesto detallado
?   ??? Términos
??? Footer
    ??? Firma
    ??? Fecha validez
```

---

## 8. ESPECIFICACIONES DE PERFORMANCE

### 8.1 Objetivos

| Métrica | Objetivo |
|---------|----------|
| **Respuesta API** | < 200ms |
| **Consulta BD** | < 100ms |
| **Carga de página** | < 2s |
| **Concurrencia** | 50+ usuarios |
| **Uptime** | 99.5% |

### 8.2 Optimizaciones

- [x] Índices en BD
- [x] Paginación (20 items/página)
- [x] Caché de consultas
- [x] Lazy loading
- [x] Compresión GZIP

---

## 9. PLAN DE IMPLEMENTACIÓN

### Fase 1-4: Logic (? COMPLETO)
- Domain Layer
- Application Layer
- Documentación

### Fase 5: Infrastructure (2-3 semanas)
- [ ] EF Core setup
- [ ] PostgreSQL schema
- [ ] Migrations
- [ ] Repositorios

### Fase 6: API (2-3 semanas)
- [ ] Controllers
- [ ] DTOs
- [ ] Middleware
- [ ] Tests

### Fase 7: Auth (1-2 semanas)
- [ ] JWT
- [ ] Roles
- [ ] Policies

### Fase 8: Frontend (4-6 semanas)
- [ ] Blazor/React setup
- [ ] Componentes
- [ ] Formularios
- [ ] Dashboard

### Fase 9: Integraciones (2-3 semanas)
- [ ] WhatsApp
- [ ] Email
- [ ] PDF
- [ ] Backups

### Fase 10: Testing (2-3 semanas)
- [ ] Unit tests
- [ ] Integration tests
- [ ] E2E tests

---

## 10. CONCLUSIÓN

Este documento especifica todas las características técnicas del sistema A Y R Servicio Técnico. La implementación seguirá esta especificación durante todas las fases de desarrollo.

**Estado Actual**: ? Lógica de negocio implementada
**Próxima Fase**: Infrastructure (EF Core + PostgreSQL)

---

**Especificación Técnica - A Y R Servicio Técnico**  
**Versión 1.0**  
**Enero 2026**
