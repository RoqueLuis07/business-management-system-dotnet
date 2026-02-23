## ESTRUCTURA DEL PROYECTO - BusinessManagementSystem

```
BusinessManagementSystem/
??? BusinessManagementSystem.Domain/
?   ??? Entities/
?   ?   ??? Client.cs                          // Cliente del taller
?   ?   ??? Equipment.cs                       // Equipo a reparar
?   ?   ??? User.cs                            // Usuario del sistema
?   ?   ??? WorkOrder.cs                       // Orden de trabajo (PRINCIPAL)
?   ?   ??? WorkOrderAccessory.cs              // Accesorios (cables, cuchillas, etc)
?   ?   ??? WorkOrderPart.cs                   // Repuestos usados
?   ?   ??? WorkOrderDiagnosis.cs              // Diagnóstico de la OT
?   ?   ??? WorkOrderQuote.cs                  // Presupuesto
?   ?   ??? WorkOrderServiceReport.cs          // Reporte de trabajo realizado
?   ?   ??? PartCatalogItem.cs                 // Catálogo de repuestos
?   ?   ??? WarrantyClaim.cs                   // Reclamo de garantía
?   ??? Enums/
?       ??? UserRole.cs                        // Enum: Admin, Mechanic
?       ??? WorkOrderStatus.cs                 // Enum: 11 estados de OT
?
??? src/Application/BusinessManagementSystem.Application/
    ??? Abstractions/
    ?   ??? IWorkOrderRepository.cs             // Interface para OTs
    ?   ??? IClientRepository.cs                // Interface para Clientes
    ?   ??? IUserRepository.cs                  // Interface para Usuarios
    ?   ??? IPartCatalogRepository.cs           // Interface para Catálogo
    ?   ??? IWarrantyClaimRepository.cs         // Interface para Garantías
    ?
    ??? WorkOrders/                            // ?? CASOS DE USO PRINCIPALES
    ?   ??? CreateWorkOrder.cs
    ?   ??? AssignMechanicToWorkOrder.cs
    ?   ??? CancelWorkOrder.cs
    ?   ??? StartWorkOrderDiagnosis.cs
    ?   ??? SetWorkOrderDiagnosis.cs
    ?   ??? AddPartToWorkOrder.cs
    ?   ??? UpdateWorkOrderPartQuantity.cs
    ?   ??? RemovePartFromWorkOrder.cs
    ?   ??? PriceWorkOrderPart.cs
    ?   ??? GenerateWorkOrderQuote.cs
    ?   ??? ApproveWorkOrder.cs
    ?   ??? RejectWorkOrderQuote.cs
    ?   ??? StartRepairWorkOrder.cs
    ?   ??? SetWorkOrderServiceReport.cs
    ?   ??? MarkWorkOrderFinished.cs
    ?   ??? MarkWorkOrderReadyForDelivery.cs
    ?   ??? MarkWorkOrderDelivered.cs
    ?   ??? SetWorkOrderWarrantyDays.cs
    ?   ??? MarkWorkOrderAsWarrantyClaim.cs
    ?   ??? AddAccessoryToWorkOrder.cs
    ?   ??? UpdateAccessoryInWorkOrder.cs
    ?   ??? RemoveAccessoryFromWorkOrder.cs
    ?   ??? GetWorkOrderById.cs
    ?   ??? GetWorkOrderByNumber.cs
    ?   ??? GetAllWorkOrders.cs
    ?   ??? GetWorkOrdersByStatus.cs
    ?   ??? GetWorkOrdersByClient.cs
    ?   ??? GetWorkOrdersByMechanic.cs
    ?   ??? GetWorkOrdersUnderWarranty.cs
    ?
    ??? Clients/                               // ?? CASOS DE USO DE CLIENTES
    ?   ??? CreateClient.cs
    ?   ??? UpdateClient.cs
    ?   ??? DeleteClient.cs
    ?   ??? GetClientById.cs
    ?   ??? GetAllClients.cs
    ?
    ??? Users/                                 // ?? CASOS DE USO DE USUARIOS
    ?   ??? CreateUser.cs
    ?   ??? UpdateUserName.cs
    ?   ??? ChangeUserRole.cs
    ?   ??? ActivateUser.cs
    ?   ??? DeactivateUser.cs
    ?   ??? DeleteUser.cs
    ?   ??? GetUserById.cs
    ?   ??? GetAllUsers.cs
    ?   ??? GetMechanics.cs
    ?
    ??? PartCatalog/                           // ?? CASOS DE USO DE CATÁLOGO
    ?   ??? CreatePartCatalogItem.cs
    ?   ??? UpdatePartCatalogPrice.cs
    ?   ??? ActivatePartCatalogItem.cs
    ?   ??? DeactivatePartCatalogItem.cs
    ?   ??? DeletePartCatalogItem.cs
    ?   ??? GetPartCatalogItem.cs
    ?   ??? GetAllPartCatalogItems.cs
    ?   ??? GetActivePartCatalogItems.cs
    ?
    ??? WarrantyClaims/                        // ?? CASOS DE USO DE GARANTÍAS
        ??? GetWarrantyClaimById.cs
        ??? GetWarrantyClaimsByOriginalWorkOrder.cs
        ??? GetAllWarrantyClaims.cs
```

---

## ?? ESTADÍSTICAS DEL PROYECTO

### Entidades Domain: 12
- WorkOrder (principal)
- Client
- Equipment
- User
- WorkOrderAccessory
- WorkOrderPart
- WorkOrderDiagnosis
- WorkOrderQuote
- WorkOrderServiceReport
- PartCatalogItem
- WarrantyClaim
- (Value Objects)

### Enumeraciones: 2
- UserRole
- WorkOrderStatus (11 estados)

### Interfaces de Repositorio: 5
- IWorkOrderRepository
- IClientRepository
- IUserRepository
- IPartCatalogRepository
- IWarrantyClaimRepository

### Casos de Uso: 47

#### WorkOrders: 27 casos de uso
- 4 Gestión básica (Create, Assign, Cancel, Get)
- 3 Diagnóstico
- 4 Repuestos
- 3 Presupuesto
- 3 Reparación
- 2 Entrega
- 2 Accesorios
- 2 Garantía
- 6 Consultas

#### Clientes: 5 casos de uso
- CRUD completo + GetAll

#### Usuarios: 9 casos de uso
- CRUD completo + GetAll + GetByRole

#### Catálogo: 8 casos de uso
- CRUD completo + GetAll + GetActive

#### Garantías: 3 casos de uso
- Consultas especializadas

---

## ?? RELACIONES ENTRE ENTIDADES

```
???????????
? Client  ???????????????????????
???????????                     ?
     ?                          ?
     ? 1:N                      ?
     ?                          ?
????????????????                ?
?  WorkOrder   ?                ?
?              ?        N:1     ?
? - Parts (N)  ?                ?
? - Accessories?                ?
? - Diagnosis  ?                ?
? - Quote      ?                ?
? - Report     ?                ?
? - Warranty   ?                ?
????????????????                ?
       ?                        ?
       ? N                      ?
       ?                        ?
????????????????     ???????????????????
?   Equipment  ?     ?  WarrantyClaim  ?
????????????????     ?                 ?
                     ? references both ?
????????????????     ? original & claim?
?  PartWorkOrders?   ? WorkOrder IDs   ?
????????????????     ???????????????????

???????????????
?   User      ?
?  - role     ?
?  - active   ?
???????????????
     ? M:1 (Mechanic assignment)
     ?
   WorkOrder (AssignedMechanicUserId)

????????????????????
? PartCatalogItem  ? (Referencia opcional en WorkOrderPart)
????????????????????
```

---

## ?? FLUJO TÍPICO DE OPERACIÓN

```
1. CREACIÓN DE CLIENTE
   CreateClient ? IClientRepository.AddAsync
   
2. CREACIÓN DE OT
   CreateWorkOrder ? IWorkOrderRepository.AddAsync
   Estado: Ingresada

3. ASIGNACIÓN
   AssignMechanicToWorkOrder ? IWorkOrderRepository.UpdateAsync
   Estado: Asignada

4. DIAGNÓSTICO
   StartWorkOrderDiagnosis ? UpdateAsync [Estado: EnDiagnostico]
   SetWorkOrderDiagnosis ? UpdateAsync [Registra diagnóstico]

5. REPUESTOS Y PRESUPUESTO
   AddPartToWorkOrder ? UpdateAsync
   PriceWorkOrderPart ? UpdateAsync
   GenerateWorkOrderQuote ? UpdateAsync
   Estado: EsperandoAprobacion

6. APROBACIÓN
   ApproveWorkOrder ? UpdateAsync
   Estado: Aprobada

7. REPARACIÓN
   StartRepairWorkOrder ? UpdateAsync [Estado: EnReparacion]
   SetWorkOrderServiceReport ? UpdateAsync

8. FINALIZACIÓN
   MarkWorkOrderFinished ? UpdateAsync [Estado: Terminada]
   MarkWorkOrderReadyForDelivery ? UpdateAsync [Estado: ListaParaEntrega]
   MarkWorkOrderDelivered ? UpdateAsync [Estado: Entregada]
                            ?
                      ?? COMIENZA GARANTÍA

9. GARANTÍA (Si cliente regresa)
   CreateWorkOrder ? Nueva OT para reparación
   MarkWorkOrderAsWarrantyClaim ? Vincula ambas OTs
   Valida automáticamente período y cliente
```

---

## ?? VALIDACIONES CRÍTICAS IMPLEMENTADAS

### Nivel Domain (En las entidades)
- ? Números de OT únicos (verificado en Application)
- ? Teléfonos únicos en clientes (verificado en Application)
- ? Emails únicos en usuarios (verificado en Application)
- ? Estados permitidos en transiciones
- ? No modificar OT cerrada
- ? Presupuesto completo antes de aprobar
- ? Garantía solo para OTs entregadas
- ? Garantía dentro del período válido
- ? Garantía del mismo cliente

### Nivel Application (En casos de uso)
- ? Entidad existe antes de modificar
- ? Parámetros requeridos no nulos
- ? Mensajes de error en español
- ? Validaciones de negocio específicas

---

## ?? MÉTODOS AGREGADOS A DOMAIN ENTITIES

### Client.cs
```csharp
public void UpdateInfo(string fullName, string phone, string address)
public void UpdatePhone(string phone)
public void UpdateAddress(string address)
```

### WorkOrderAccessory.cs
```csharp
public void UpdateCondition(bool isPresent, string? condition)
```

### WorkOrder.cs
```csharp
public void UpdateAccessory(Guid accessoryId, bool isPresent, string? condition)
public void RemoveAccessory(Guid accessoryId)
```

---

## ?? INTERFACES EXTENDIDAS

### IWorkOrderRepository
Métodos agregados:
```csharp
Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken ct);
Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken ct);
Task<IEnumerable<WorkOrder>> GetByClientAsync(Guid clientId, CancellationToken ct);
Task<IEnumerable<WorkOrder>> GetByMechanicAsync(Guid mechanicUserId, CancellationToken ct);
Task<IEnumerable<WorkOrder>> GetUnderWarrantyAsync(DateTime nowLocal, CancellationToken ct);
```

---

## ??? PATRÓN ARQUITECTÓNICO

### Separación de Responsabilidades

**Domain Layer** ? Lógica de negocio pura
- Entidades con comportamiento
- Validaciones de reglas
- Enumeraciones
- Sin dependencias externas

**Application Layer** ? Orquestación de casos de uso
- Commands & Queries
- Validaciones cruzadas
- Invocación de repositorios
- Mapeo de DTOs

**(Próximas capas - No implementadas aún)**
- **Infrastructure Layer**: EF Core, Repositories
- **API Layer**: Controllers, DTOs
- **Presentation Layer**: UI (Web, Desktop, etc.)

---

## ?? CONVENCIONES DE CÓDIGO

1. **Nombres en español** para negocio (Usuario, OT, Presupuesto)
2. **Nombres en inglés** para código (Guid, CancellationToken, async)
3. **Record** para Commands y Queries (C# 9+)
4. **Static classes** para casos de uso
5. **HandleAsync** como método principal
6. **Excepciones descriptivas** con contexto
7. **Mappers privados** para DTOs

---

## ?? COBERTURA DE FUNCIONALIDAD

| Módulo | Crear | Leer | Actualizar | Eliminar | Listar |
|--------|-------|------|-----------|----------|--------|
| WorkOrders | ? | ? | ? (27 ops) | ? | ? |
| Clientes | ? | ? | ? | ? | ? |
| Usuarios | ? | ? | ? | ? | ? |
| Catálogo | ? | ? | ? | ? | ? |
| Garantías | ? | ? | ? | ? | ? |

*Nota: WorkOrders no tiene "Eliminar" porque se cierran (Entregadas/Canceladas)

---

**Versión**: 1.0
**Compilación**: ? Exitosa (.NET 8)
**Próximo paso**: Implementar capa de Infraestructura (EF Core + SQL)
