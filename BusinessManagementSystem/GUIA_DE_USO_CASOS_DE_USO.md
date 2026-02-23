## GUÍA DE USO DE CASOS DE USO - BusinessManagementSystem

### Patrones Implementados

Todos los casos de uso siguen un patrón consistente:

#### Patrón para Comandos (Operaciones que modifican estado)

```csharp
using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class NombreDelCasoDeUso
    {
        // 1. Define el record con los parámetros de entrada
        public record Command(Guid WorkOrderId, string Parameter);

        // 2. Implementa un manejador async
        public static async Task HandleAsync(
            IWorkOrderRepository repo, 
            Command cmd, 
            CancellationToken ct)
        {
            // 3. Obtén la entidad
            var entity = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (entity is null)
                throw new InvalidOperationException("No se encontró la entidad.");

            // 4. Modifica según reglas de negocio
            entity.HacerAlgo(cmd.Parameter);

            // 5. Persiste cambios
            await repo.UpdateAsync(entity, ct);
        }
    }
}
```

#### Patrón para Queries (Lecturas)

```csharp
using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.SomeModule
{
    public static class GetSomething
    {
        // 1. Define el Query record
        public record Query(Guid SomeId);

        // 2. Define el Result DTO
        public record Result(Guid Id, string Name, string Status);

        // 3. Implementa manejador async
        public static async Task<Result> HandleAsync(
            ISomeRepository repo, 
            Query query, 
            CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(query.SomeId, ct);
            if (entity is null)
                throw new InvalidOperationException("No se encontró.");

            return MapToResult(entity);
        }

        // 4. Mapeo de entidad a DTO
        private static Result MapToResult(SomeEntity entity) =>
            new Result(entity.Id, entity.Name, entity.Status.ToString());
    }
}
```

---

## EJEMPLOS DE USO POR MÓDULO

### ?? ÓRDENES DE TRABAJO

#### Crear una nueva orden de trabajo

```csharp
var createCmd = new CreateWorkOrder.Command(
    WorkOrderNumber: "OT-2026-001",
    ClientFullName: "Juan Pérez",
    ClientPhone: "555-1234",
    ClientAddress: "Calle 123",
    EquipmentType: "Demalezadora",
    EquipmentBrand: "Stihl",
    EquipmentModel: "FS 130",
    EquipmentSerialNumber: "ABC123",
    RequestedWorkDescription: "Cambio de cadena y mantenimiento"
);

var workOrderId = await CreateWorkOrder.HandleAsync(workOrderRepository, createCmd, cancellationToken);
```

#### Asignar un mecánico

```csharp
var assignCmd = new AssignMechanicToWorkOrder.Command(
    WorkOrderId: workOrderId,
    MechanicUserId: mechanicId
);

await AssignMechanicToWorkOrder.HandleAsync(workOrderRepository, assignCmd, cancellationToken);
```

#### Iniciar diagnóstico

```csharp
var startCmd = new StartWorkOrderDiagnosis.Command(WorkOrderId: workOrderId);
await StartWorkOrderDiagnosis.HandleAsync(workOrderRepository, startCmd, cancellationToken);
```

#### Registrar diagnóstico

```csharp
var diagCmd = new SetWorkOrderDiagnosis.Command(
    WorkOrderId: workOrderId,
    Findings: "Cadena desgastada, motor en buen estado",
    RecommendedWork: "Cambio de cadena",
    Notes: "Cliente solicita revisión de tubo",
    MechanicUserId: mechanicId
);

await SetWorkOrderDiagnosis.HandleAsync(workOrderRepository, diagCmd, cancellationToken);
```

#### Agregar repuestos

```csharp
var addPartCmd = new AddPartToWorkOrder.Command(
    WorkOrderId: workOrderId,
    PartName: "Cadena para motosierra",
    Quantity: 1
);

await AddPartToWorkOrder.HandleAsync(workOrderRepository, addPartCmd, cancellationToken);
```

#### Asignar precio a repuesto

```csharp
// Primero obtén el ID del repuesto desde la OT
var workOrder = await workOrderRepository.GetByIdAsync(workOrderId, ct);
var partId = workOrder.Parts.First().Id;

var priceCmd = new PriceWorkOrderPart.Command(
    WorkOrderId: workOrderId,
    PartId: partId,
    UnitPrice: 45.50m,
    CatalogItemId: null // Opcional: si viene del catálogo
);

await PriceWorkOrderPart.HandleAsync(workOrderRepository, priceCmd, cancellationToken);
```

#### Generar presupuesto

```csharp
var quoteCmd = new GenerateWorkOrderQuote.Command(
    WorkOrderId: workOrderId,
    LaborCost: 150.00m,
    Notes: "Incluye revisión completa",
    CreatedByUserId: adminUserId
);

await GenerateWorkOrderQuote.HandleAsync(workOrderRepository, quoteCmd, cancellationToken);
```

#### Aprobar presupuesto y OT

```csharp
var approveCmd = new ApproveWorkOrder.Command(WorkOrderId: workOrderId);
await ApproveWorkOrder.HandleAsync(workOrderRepository, approveCmd, cancellationToken);
```

#### Iniciar reparación

```csharp
var startRepairCmd = new StartRepairWorkOrder.Command(WorkOrderId: workOrderId);
await StartRepairWorkOrder.HandleAsync(workOrderRepository, startRepairCmd, cancellationToken);
```

#### Registrar trabajo realizado

```csharp
var reportCmd = new SetWorkOrderServiceReport.Command(
    WorkOrderId: workOrderId,
    WorkPerformed: "Se cambió cadena, se aplicó lubricante, se realizó prueba",
    Recommendations: "Próximo cambio de cadena en 6 meses",
    Notes: "Cliente satisfecho con resultado",
    MechanicUserId: mechanicId
);

await SetWorkOrderServiceReport.HandleAsync(workOrderRepository, reportCmd, cancellationToken);
```

#### Marcar como terminada

```csharp
var finishCmd = new MarkWorkOrderFinished.Command(WorkOrderId: workOrderId);
await MarkWorkOrderFinished.HandleAsync(workOrderRepository, finishCmd, cancellationToken);
```

#### Preparar para entrega

```csharp
var readyCmd = new MarkWorkOrderReadyForDelivery.Command(WorkOrderId: workOrderId);
await MarkWorkOrderReadyForDelivery.HandleAsync(workOrderRepository, readyCmd, cancellationToken);
```

#### Registrar entrega

```csharp
var deliverCmd = new MarkWorkOrderDelivered.Command(
    WorkOrderId: workOrderId,
    DeliveredAtLocal: DateTime.Now
);

await MarkWorkOrderDelivered.HandleAsync(workOrderRepository, deliverCmd, cancellationToken);
// ?? Aquí comienza el período de garantía
```

#### Obtener detalles completos de OT

```csharp
var getQuery = new GetWorkOrderById.Query(WorkOrderId: workOrderId);
var result = await GetWorkOrderById.HandleAsync(workOrderRepository, getQuery, cancellationToken);

// Result contiene:
// - Datos de cliente y equipo
// - Diagnóstico, presupuesto, reporte
// - Listado de accesorios y repuestos
// - Información de garantía
```

---

### ?? CLIENTES

#### Crear cliente

```csharp
var createCmd = new CreateClient.Command(
    FullName: "Carlos García",
    Phone: "555-5678",
    Address: "Avenida Principal 456"
);

var clientId = await CreateClient.HandleAsync(clientRepository, createCmd, cancellationToken);
```

#### Actualizar cliente

```csharp
var updateCmd = new UpdateClient.Command(
    ClientId: clientId,
    FullName: "Carlos García López",
    Phone: "555-8765", // Nuevo teléfono (único)
    Address: "Avenida Principal 789"
);

await UpdateClient.HandleAsync(clientRepository, updateCmd, cancellationToken);
```

#### Obtener cliente

```csharp
var getQuery = new GetClientById.Query(ClientId: clientId);
var clientResult = await GetClientById.HandleAsync(clientRepository, getQuery, cancellationToken);
```

#### Listar todos los clientes

```csharp
var allClientsResult = await GetAllClients.HandleAsync(clientRepository, cancellationToken);
foreach (var client in allClientsResult)
{
    Console.WriteLine($"{client.FullName} - {client.Phone}");
}
```

---

### ????? USUARIOS

#### Crear usuario (Admin)

```csharp
var createCmd = new CreateUser.Command(
    FullName: "Ana Rodríguez",
    Email: "ana@workshop.com",
    Role: UserRole.Admin
);

var userId = await CreateUser.HandleAsync(userRepository, createCmd, cancellationToken);
```

#### Crear usuario (Mecánico)

```csharp
var createMechCmd = new CreateUser.Command(
    FullName: "Pedro Martínez",
    Email: "pedro@workshop.com",
    Role: UserRole.Mechanic
);

var mechanicId = await CreateUser.HandleAsync(userRepository, createMechCmd, cancellationToken);
```

#### Cambiar rol de usuario

```csharp
var changeRoleCmd = new ChangeUserRole.Command(
    UserId: userId,
    NewRole: UserRole.Mechanic
);

await ChangeUserRole.HandleAsync(userRepository, changeRoleCmd, cancellationToken);
```

#### Obtener todos los mecánicos

```csharp
var mechanicsResult = await GetMechanics.HandleAsync(userRepository, cancellationToken);
foreach (var mechanic in mechanicsResult)
{
    Console.WriteLine($"{mechanic.FullName} - {mechanic.Email}");
}
```

---

### ?? CATÁLOGO DE REPUESTOS

#### Crear repuesto en catálogo

```csharp
var createPartCmd = new CreatePartCatalogItem.Command(
    Name: "Cadena 40 eslabones",
    DefaultUnitPrice: 45.50m
);

var partId = await CreatePartCatalogItem.HandleAsync(partCatalogRepository, createPartCmd, cancellationToken);
```

#### Actualizar precio

```csharp
var updatePriceCmd = new UpdatePartCatalogPrice.Command(
    ItemId: partId,
    NewPrice: 48.99m
);

await UpdatePartCatalogPrice.HandleAsync(partCatalogRepository, updatePriceCmd, cancellationToken);
```

#### Obtener repuestos activos (para interface)

```csharp
var activePartsResult = await GetActivePartCatalogItems.HandleAsync(partCatalogRepository, cancellationToken);
// Para popear dropdown en form de OT
```

---

### ?? GARANTÍAS

#### Registrar OT como garantía (cuando cliente regresa)

```csharp
// 1. Crear nueva OT para la reparación por garantía
var claimWorkOrderId = await CreateWorkOrder.HandleAsync(...);

// 2. Marcar como claim de garantía
var markAsClaimCmd = new MarkWorkOrderAsWarrantyClaim.Command(
    WorkOrderId: claimWorkOrderId,
    OriginalWorkOrderId: originalWorkOrderId,
    Reason: "Problema con la reparación anterior - cadena mal instalada",
    CreatedByUserId: adminUserId,
    NowLocal: DateTime.Now
);

await MarkWorkOrderAsWarrantyClaim.HandleAsync(workOrderRepository, markAsClaimCmd, cancellationToken);
// ?? Valida automáticamente que OT original esté entregada y dentro del período
```

#### Obtener garantías de una OT

```csharp
var claimsQuery = new GetWarrantyClaimsByOriginalWorkOrder.Query(
    OriginalWorkOrderId: originalWorkOrderId
);

var claimsResult = await GetWarrantyClaimsByOriginalWorkOrder.HandleAsync(warrantyRepository, claimsQuery, cancellationToken);
```

#### Obtener OTs bajo garantía

```csharp
var underWarrantyQuery = new GetWorkOrdersUnderWarranty.Query(NowLocal: DateTime.Now);
var underWarrantyResult = await GetWorkOrdersUnderWarranty.HandleAsync(workOrderRepository, underWarrantyQuery, cancellationToken);
```

---

## ?? MANEJO DE ERRORES

Todos los casos de uso lanzan `InvalidOperationException` o `ArgumentException` con mensajes en español:

```csharp
try
{
    await CreateWorkOrder.HandleAsync(repo, cmd, ct);
}
catch (InvalidOperationException ex)
{
    // "Ya existe una OT con ese número."
    // "No se encontró el cliente."
    Console.WriteLine(ex.Message);
}
catch (ArgumentException ex)
{
    // "El nombre del cliente es obligatorio."
    // "El teléfono ya existe."
    Console.WriteLine(ex.Message);
}
```

---

## ?? CONSIDERACIONES DE SEGURIDAD

1. **Validación de IDs**: Siempre se valida que las entidades existan
2. **Autorización**: Implementar en API layer (verificar rol del usuario)
3. **Auditoria**: Todos los casos críticos registran `CreatedByUserId`
4. **Integridad**: Las transiciones de estado son exhaustivas (no hay saltos ilegales)

---

## ?? INTEGRACIÓN CON INFRAESTRUCTURA

Los repositorios son interfaces, implementalos en una capa de Infraestructura:

```csharp
// Infraestructura/EF/WorkOrderRepository.cs
public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly ApplicationDbContext _context;

    public async Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.WorkOrders
            .Include(wo => wo.Parts)
            .Include(wo => wo.Accessories)
            .FirstOrDefaultAsync(wo => wo.Id == id, ct);
    }

    public async Task UpdateAsync(WorkOrder workOrder, CancellationToken ct)
    {
        _context.WorkOrders.Update(workOrder);
        await _context.SaveChangesAsync(ct);
    }

    // ... resto de métodos
}
```

---

## ?? PRÓXIMOS PASOS

1. Implementar repositorios con Entity Framework Core
2. Crear migrations de base de datos
3. Implementar API Controllers que usen estos casos de uso
4. Agregar autenticación/autorización
5. Crear validaciones adicionales con FluentValidation
6. Implementar logging y auditoria
7. Agregar tests unitarios para cada caso de uso

---

**Última actualización**: 04/02/2026
**Versión**: 1.0
