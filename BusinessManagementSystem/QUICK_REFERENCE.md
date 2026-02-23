## ?? QUICK REFERENCE - BusinessManagementSystem

Una guía rápida de referencia para encontrar lo que necesitas en 30 segundos.

---

## ?? EMPEZAR AQUÍ

| Quiero... | Lee esto | Tiempo |
|----------|----------|--------|
| **Instalar el proyecto** | [INSTALLATION.md](./INSTALLATION.md) | 10 min |
| **Entender qué existe** | [README.md](./README.md) | 10 min |
| **Ver código de ejemplo** | [GUIA_DE_USO_CASOS_DE_USO.md](./GUIA_DE_USO_CASOS_DE_USO.md) | 30 min |
| **Entender arquitectura** | [ESTRUCTURA_Y_ORGANIZACION.md](./ESTRUCTURA_Y_ORGANIZACION.md) | 15 min |
| **Ver diagramas** | [DIAGRAMAS_Y_FLUJOS.md](./DIAGRAMAS_Y_FLUJOS.md) | 10 min |
| **Saber próximos pasos** | [README_CHECKLIST.md](./README_CHECKLIST.md) | 10 min |
| **Resumen de todo** | [PROJECT_SUMMARY.md](./PROJECT_SUMMARY.md) | 5 min |

---

## ?? ÍNDICE DE DOCUMENTACIÓN

```
?? DOCUMENTACIÓN
??? ?? PRINCIPALES
?   ??? README.md                    ? EMPIEZA AQUÍ
?   ??? INSTALLATION.md              ? Instalación paso a paso
?   ??? PROJECT_SUMMARY.md           ? Resumen de todo
?
??? ?? REFERENCIAS TÉCNICAS
?   ??? CASOS_DE_USO_IMPLEMENTADOS.md
?   ??? GUIA_DE_USO_CASOS_DE_USO.md
?   ??? ESTRUCTURA_Y_ORGANIZACION.md
?   ??? DIAGRAMAS_Y_FLUJOS.md
?
??? ?? ROADMAP
?   ??? README_CHECKLIST.md
?   ??? DEPLOYMENT.md
?   ??? RESUMEN_RAPIDO.md
?
??? ?? CONTRIBUCIÓN
?   ??? CONTRIBUTING.md
?   ??? LICENSE
?   ??? .gitignore
?
??? ??? NAVEGACIÓN
    ??? INDICE_DOCUMENTACION.md
    ??? QUICK_REFERENCE.md (este archivo)
    ??? README_EJECUTIVO.md
```

---

## ?? COMANDOS RÁPIDOS

### Compilar
```bash
dotnet build
```

### Restaurar dependencias
```bash
dotnet restore
```

### Limpiar compilación
```bash
dotnet clean
```

### Compilar en Release
```bash
dotnet build -c Release
```

### Listar proyectos
```bash
dotnet sln list
```

---

## ?? ESTADÍSTICAS

```
Casos de Uso:        47
Entidades:           12
Enumeraciones:       2
Líneas de Código:    2,300+
Documentación:       85+ páginas
Ejemplos de Código:  30+
Diagramas:           8
Interfaces:          5
```

---

## ??? ESTRUCTURA DEL CÓDIGO

```
Domain Layer (Lógica Pura)
  ??? Entities/          (12 entidades)
  ??? Enums/             (2 enumeraciones)

Application Layer (Casos de Uso)
  ??? Abstractions/      (5 interfaces)
  ??? WorkOrders/        (27 casos de uso)
  ??? Clients/           (5 casos de uso)
  ??? Users/             (9 casos de uso)
  ??? PartCatalog/       (8 casos de uso)
  ??? WarrantyClaims/    (3 casos de uso)
```

---

## ?? ENCONTRAR FUNCIONALIDAD

### Crear Orden de Trabajo
?? `src/Application/WorkOrders/CreateWorkOrder.cs`  
?? Ver: [GUIA_DE_USO_CASOS_DE_USO.md](./GUIA_DE_USO_CASOS_DE_USO.md) - Sección WorkOrders

### Gestionar Clientes
?? `src/Application/Clients/`  
?? Ver: [CASOS_DE_USO_IMPLEMENTADOS.md](./CASOS_DE_USO_IMPLEMENTADOS.md) - Sección Clientes

### Gestionar Usuarios
?? `src/Application/Users/`  
?? Ver: [CASOS_DE_USO_IMPLEMENTADOS.md](./CASOS_DE_USO_IMPLEMENTADOS.md) - Sección Usuarios

### Gestionar Catálogo
?? `src/Application/PartCatalog/`  
?? Ver: [CASOS_DE_USO_IMPLEMENTADOS.md](./CASOS_DE_USO_IMPLEMENTADOS.md) - Sección Catálogo

### Gestionar Garantías
?? `src/Application/WarrantyClaims/`  
?? Ver: [DIAGRAMAS_Y_FLUJOS.md](./DIAGRAMAS_Y_FLUJOS.md) - Sección Garantías

---

## ?? APRENDER EL PROYECTO

### Día 1 (2 horas)
- [ ] Lee README.md (10 min)
- [ ] Lee INSTALLATION.md (10 min)
- [ ] Compila `dotnet build` (5 min)
- [ ] Lee ESTRUCTURA_Y_ORGANIZACION.md (20 min)
- [ ] Lee DIAGRAMAS_Y_FLUJOS.md (20 min)
- [ ] Explora carpetas en VS Code (55 min)

### Día 2 (2 horas)
- [ ] Lee CASOS_DE_USO_IMPLEMENTADOS.md (20 min)
- [ ] Lee GUIA_DE_USO_CASOS_DE_USO.md (40 min)
- [ ] Abre archivos .cs y estudia (40 min)
- [ ] Prueba compilar/cambiar código (20 min)

### Día 3 (1 hora)
- [ ] Lee README_CHECKLIST.md (15 min)
- [ ] Lee PROJECT_SUMMARY.md (10 min)
- [ ] Planifica próximos pasos (35 min)

---

## ? PREGUNTAS FRECUENTES

**¿Por dónde empiezo?**  
? Sigue el plan "Aprender el Proyecto" arriba

**¿Cómo instalo?**  
? [INSTALLATION.md](./INSTALLATION.md)

**¿Dónde están los casos de uso?**  
? `src/Application/` (por módulo)

**¿Cómo veo ejemplos de código?**  
? [GUIA_DE_USO_CASOS_DE_USO.md](./GUIA_DE_USO_CASOS_DE_USO.md)

**¿Cuál es la siguiente fase?**  
? [README_CHECKLIST.md](./README_CHECKLIST.md) - Fase 5

**¿Cómo contribuyo?**  
? [CONTRIBUTING.md](./CONTRIBUTING.md)

**¿Cuál es la licencia?**  
? [LICENSE](./LICENSE) (MIT)

---

## ?? ACCESO RÁPIDO

### GitHub
?? https://github.com/RoqueLuis07/business-management-system-dotnet

### Ramas
- `main` - Código estable (actual)
- `develop` - Desarrollo (próximo)
- `feature/*` - Nuevas características

### Issues
?? https://github.com/RoqueLuis07/business-management-system-dotnet/issues

---

## ?? CASOS DE USO POR CATEGORÍA

### WorkOrders (27)
```
Crear               ? CreateWorkOrder
Asignar             ? AssignMechanicToWorkOrder
Diagnóstico         ? StartWorkOrderDiagnosis, SetWorkOrderDiagnosis
Repuestos           ? AddPartToWorkOrder, PriceWorkOrderPart, ...
Presupuesto         ? GenerateWorkOrderQuote, ApproveWorkOrder, ...
Reparación          ? StartRepairWorkOrder, SetServiceReport, ...
Entrega             ? MarkReadyForDelivery, MarkDelivered
Garantía            ? SetWarrantyDays, MarkAsWarrantyClaim
Consultas           ? GetWorkOrderById, GetByStatus, ...
```

### Clients (5)
```
Crear               ? CreateClient
Actualizar          ? UpdateClient
Eliminar            ? DeleteClient
Obtener             ? GetClientById
Listar              ? GetAllClients
```

### Users (9)
```
Crear               ? CreateUser
Cambiar Rol         ? ChangeUserRole
Activar/Desactivar  ? ActivateUser, DeactivateUser
Obtener             ? GetUserById
Listar              ? GetAllUsers, GetMechanics
Actualizar          ? UpdateUserName
Eliminar            ? DeleteUser
```

### PartCatalog (8)
```
Crear               ? CreatePartCatalogItem
Precio              ? UpdatePartCatalogPrice
Activ/Desac         ? ActivatePartCatalogItem, ...
Obtener             ? GetPartCatalogItem
Listar              ? GetAllPartCatalogItems, GetActivePartCatalogItems
Eliminar            ? DeletePartCatalogItem
```

### WarrantyClaims (3)
```
Obtener             ? GetWarrantyClaimById
Listar              ? GetAllWarrantyClaims
Por Original        ? GetWarrantyClaimsByOriginalWorkOrder
```

---

## ?? FLUJO DE ESTADOS - OT

```
Ingresada
   ?
Asignada
   ?
EnDiagnostico
   ?
EsperandoAprobacion ?? PresupuestoRechazado
   ?
Aprobada
   ?
EnReparacion
   ?
Terminada
   ?
ListaParaEntrega
   ?
Entregada ? GARANTÍA COMIENZA
```

---

## ??? HERRAMIENTAS RECOMENDADAS

| Herramienta | Descarga |
|-------------|----------|
| **.NET 8 SDK** | https://dotnet.microsoft.com/download |
| **Visual Studio 2022** | https://visualstudio.microsoft.com |
| **VS Code** | https://code.visualstudio.com |
| **Git** | https://git-scm.com |
| **Postman** | https://www.postman.com (para API futura) |

---

## ?? CHECKLIST DE CONFIGURACIÓN

- [ ] .NET 8 SDK instalado
- [ ] Git instalado
- [ ] Repositorio clonado
- [ ] `dotnet restore` ejecutado
- [ ] `dotnet build` exitoso
- [ ] Proyecto abierto en IDE
- [ ] Documentación leída
- [ ] Listo para desarrollar

---

## ?? PRÓXIMOS PASOS

1. ? **Fase 1-4 Completadas** (Actual)
   - Domain & Application implementado
   - Documentación completa

2. ? **Fase 5: Infrastructure** (Próxima)
   - Entity Framework Core
   - Base de datos
   - Repositorios concretos

3. ? **Fase 6: API**
   - ASP.NET Core REST API
   - Controllers
   - Swagger

4. ? **Fase 7: Authentication**
   - JWT
   - Roles

5. ? **Fase 8-10: Finishing**
   - Testing
   - UI
   - DevOps

---

## ?? SOPORTE

- ?? Documentación: Ver índice arriba
- ?? Bugs: Abre Issue en GitHub
- ?? Preguntas: Consulta documentación primero
- ?? Contacto: Ver README.md

---

## ? ONE-LINERS

```bash
# Compilar
dotnet build

# Ver proyectos
dotnet sln list

# Limpiar
dotnet clean

# Restaurar
dotnet restore

# Build Release
dotnet build -c Release
```

---

## ?? TU PRÓXIMA ACCIÓN

### Opción 1: Aprender (Recomendado)
```bash
1. Lee README.md
2. Lee INSTALLATION.md
3. Ejecuta: dotnet build
4. Lee documentación técnica
```

### Opción 2: Instalar Rápido
```bash
1. git clone repo
2. dotnet restore
3. dotnet build
4. ¡Listo!
```

### Opción 3: Comenzar Infraestructura
```bash
1. Lee README_CHECKLIST.md - Fase 5
2. Crea proyecto Infrastructure
3. Setup EF Core
```

---

**Última Actualización**: Enero 2026  
**Status**: ? Production-Ready (Logic)  
**Próxima Revisión**: Después de Fase 5

---

?? **¡Happy Coding!**
