## Sistema de Administración para Taller Mecánico - Casos de Uso Implementados

### ?? GESTIÓN DE ÓRDENES DE TRABAJO (WorkOrders)

#### Operaciones de Creación y Control
- **CreateWorkOrder** - Crear nueva orden de trabajo
- **AssignMechanicToWorkOrder** - Asignar mecánico a una OT
- **CancelWorkOrder** - Cancelar una OT existente

#### Diagnóstico
- **StartWorkOrderDiagnosis** - Iniciar fase de diagnóstico
- **SetWorkOrderDiagnosis** - Registrar hallazgos y trabajo recomendado

#### Gestión de Repuestos/Partes
- **AddPartToWorkOrder** - Agregar repuesto a la OT
- **PriceWorkOrderPart** - Asignar precio a un repuesto
- **UpdateWorkOrderPartQuantity** - Actualizar cantidad de repuesto
- **RemovePartFromWorkOrder** - Eliminar repuesto de la OT

#### Presupuesto
- **GenerateWorkOrderQuote** - Generar/actualizar presupuesto
- **ApproveWorkOrder** - Aprobar presupuesto y OT
- **RejectWorkOrderQuote** - Rechazar presupuesto

#### Reparación
- **StartRepairWorkOrder** - Iniciar fase de reparación
- **SetWorkOrderServiceReport** - Registrar trabajo realizado
- **MarkWorkOrderFinished** - Marcar como terminada

#### Entrega
- **MarkWorkOrderReadyForDelivery** - Preparar para entrega
- **MarkWorkOrderDelivered** - Registrar entrega al cliente

#### Accesorios
- **AddAccessoryToWorkOrder** - Agregar accesorio (cables, etc.)
- **UpdateAccessoryInWorkOrder** - Actualizar estado de accesorio
- **RemoveAccessoryFromWorkOrder** - Eliminar accesorio

#### Garantía
- **SetWorkOrderWarrantyDays** - Configurar días de garantía
- **MarkWorkOrderAsWarrantyClaim** - Registrar OT como claim de garantía

#### Consultas
- **GetWorkOrderById** - Obtener detalles completos de una OT
- **GetWorkOrderByNumber** - Buscar OT por número de talonario
- **GetAllWorkOrders** - Listar todas las OTs
- **GetWorkOrdersByStatus** - Filtrar OTs por estado
- **GetWorkOrdersByClient** - Listar OTs de un cliente
- **GetWorkOrdersByMechanic** - Listar OTs asignadas a un mecánico
- **GetWorkOrdersUnderWarranty** - Obtener OTs en periodo de garantía

---

### ?? GESTIÓN DE CLIENTES

#### CRUD Completo
- **CreateClient** - Crear nuevo cliente
- **UpdateClient** - Actualizar información del cliente
- **DeleteClient** - Eliminar cliente
- **GetClientById** - Obtener detalles de cliente
- **GetAllClients** - Listar todos los clientes

**Validaciones:**
- Nombre obligatorio
- Teléfono único en el sistema
- Permite actualización de dirección y teléfono

---

### ????? GESTIÓN DE USUARIOS

#### CRUD Completo
- **CreateUser** - Crear nuevo usuario (Admin, Mechanic, etc.)
- **UpdateUserName** - Actualizar nombre de usuario
- **ChangeUserRole** - Cambiar rol del usuario
- **ActivateUser** - Activar usuario desactivado
- **DeactivateUser** - Desactivar usuario
- **DeleteUser** - Eliminar usuario

#### Consultas
- **GetUserById** - Obtener detalles de usuario
- **GetAllUsers** - Listar todos los usuarios
- **GetMechanics** - Listar solo mecánicos activos

**Validaciones:**
- Email único en el sistema (case-insensitive)
- Nombre obligatorio
- Roles: Admin, Mechanic (extensible)

---

### ?? GESTIÓN DE CATÁLOGO DE REPUESTOS

#### CRUD Completo
- **CreatePartCatalogItem** - Agregar repuesto al catálogo
- **UpdatePartCatalogPrice** - Actualizar precio de repuesto
- **ActivatePartCatalogItem** - Reactivar repuesto
- **DeactivatePartCatalogItem** - Desactivar repuesto
- **DeletePartCatalogItem** - Eliminar repuesto del catálogo

#### Consultas
- **GetPartCatalogItem** - Obtener detalles de repuesto
- **GetAllPartCatalogItems** - Listar todos los repuestos
- **GetActivePartCatalogItems** - Listar solo repuestos activos

**Validaciones:**
- Nombre único en el catálogo
- Precio no negativo
- Estado de activo/inactivo

---

### ?? GESTIÓN DE GARANTÍAS (WarrantyClaims)

#### Consultas
- **GetWarrantyClaimById** - Obtener detalles de un reclamo
- **GetWarrantyClaimsByOriginalWorkOrder** - Listar claims de una OT original
- **GetAllWarrantyClaims** - Listar todos los reclamos de garantía

**Validaciones:**
- Solo se pueden crear claims para OTs entregadas
- Solo dentro del período de garantía
- Cliente debe ser el mismo que en OT original

---

## ?? FLUJO DE ESTADOS DE ÓRDENES DE TRABAJO

```
Ingresada
    ?
Asignada (al asignar mecánico)
    ?
EnDiagnostico (inicia diagnóstico)
    ?
EsperandoAprobacion (se genera presupuesto)
    ?? PresupuestoRechazado (rechaza presupuesto ? vuelve a EnDiagnostico)
    ?
Aprobada (cliente aprueba presupuesto)
    ?
EnReparacion (inicia reparación)
    ?
Terminada (se registra trabajo realizado)
    ?
ListaParaEntrega (preparada)
    ?
Entregada (entregada al cliente) ? Aquí comienza garantía
    ?
[CERRADA]

Cancelada (desde cualquier estado excepto Entregada/Cancelada)
    ?
[CERRADA]
```

---

## ??? ARQUITECTURA DE CAPAS

### Domain Layer (Reglas de Negocio)
- **Entities**: Client, Equipment, User, WorkOrder, WarrantyClaim, PartCatalogItem
- **Value Objects**: WorkOrderAccessory, WorkOrderPart, WorkOrderDiagnosis, WorkOrderQuote, WorkOrderServiceReport
- **Enums**: UserRole, WorkOrderStatus

### Application Layer (Casos de Uso)
- **Abstractions**: IWorkOrderRepository, IClientRepository, IUserRepository, IPartCatalogRepository, IWarrantyClaimRepository
- **Commands**: Operaciones que modifican estado (Create, Update, Delete, Approve, Cancel, etc.)
- **Queries**: Operaciones de lectura (Get, GetAll, GetBy...)

---

## ? CONSIDERACIONES IMPLEMENTADAS

1. **Validaciones de Negocio Exhaustivas**
   - Números de OT únicos globales
   - Teléfonos y emails únicos
   - Estados permitidos en cada transición
   - Reglas de garantía (fecha entrega, período)

2. **Prevención de Estados Inválidos**
   - No se puede modificar OT cerrada (Entregada/Cancelada)
   - No se puede generar presupuesto sin todas las partes precificadas
   - No se puede aprobar sin presupuesto

3. **Auditoria**
   - CreatedAtUtc en todas las entidades principales
   - Usuario creador en presupuestos, diagnósticos, reportes
   - Razones de rechazo y cancelación

4. **Flexibilidad**
   - Garantía configurable por OT (1-365 días, default 30)
   - Accesorios con estado de presencia y condiciones
   - Repuestos desde catálogo O manuales

5. **Trazabilidad de Garantías**
   - Vinculación OT Original ? OT Claim
   - Registro de razón del claim
   - Usuario que registra la garantía
   - Validación de período de garantía automática

---

## ?? PRÓXIMAS IMPLEMENTACIONES (Recomendadas)

1. **Capa de Infraestructura**
   - Implementar repositorios con Entity Framework Core
   - Migrations de base de datos

2. **API Layer**
   - Controllers REST para cada módulo
   - DTOs para request/response
   - Mapeos de entidades a DTOs

3. **Validaciones Cruzadas**
   - Pipeline de validación adicional
   - Business rules engine

4. **Reportes**
   - Órdenes por período
   - Ingresos por mecánico
   - Garantías pendientes
   - Equipos por tipo

5. **Notificaciones**
   - Email al cliente sobre estado de OT
   - Alertas de garantía venciendo
   - Recordatorios de entrega

6. **Autenticación y Autorización**
   - Identity/JWT
   - Políticas por rol

---

**Fecha de Documentación**: 04/02/2026
**Versión**: 1.0 - Casos de Uso Completados
