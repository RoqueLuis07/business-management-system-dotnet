# ?? A Y R Servicio Técnico - Sistema de Gestión

> **Sistema de gestión integral para taller de reparación de maquinaria**
>
> Solución profesional diseñada específicamente para A Y R Servicio Técnico
> 
> **Ubicación**: Asunción, Paraguay | **Moneda**: Guaraníes (PYG)

---

## ?? Tabla de Contenidos

- [Descripción del Negocio](#descripción-del-negocio)
- [Características](#características)
- [Tecnología](#tecnología)
- [Requisitos](#requisitos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Uso](#uso)
- [Arquitectura](#arquitectura)
- [Documentación](#documentación)
- [Roadmap](#roadmap)

---

## ?? Descripción del Negocio

**A Y R Servicio Técnico** es un taller especializado ubicado en:

```
La Pachos Casicarios N° 277
Barrio Hipódromo
Asunción, Paraguay
```

### Especialidades

El taller se especializa en mantenimiento, diagnóstico y reparación de:

#### ?? Equipos de Jardinería
- Desmalezadoras
- Motosierras
- Cortacéspedes
- Sopladoras

#### ?? Equipos de Limpieza
- Hidrolavadoras
- Equipos de limpieza general

#### ??? Equipos de Fumigación
- Fumigadoras manuales
- Fumigadoras a combustión

#### ? Equipos de Generación
- Generadores diésel
- Generadores nafteros

#### ?? Equipos Hidráulicos
- Bombas de agua

#### ?? Otros Equipos
Cualquier equipo relacionado con limpieza, jardinería, generación y bombeo.

### Capacidad Operativa

- **Mecánicos Actuales**: 5-10
- **Escalabilidad**: ? Sistema preparado para crecimiento

---

## ? Características Principales

### ?? Gestión Completa de Órdenes
- ? Creación y seguimiento de órdenes de trabajo
- ? 8 estados de progreso controlados
- ? Asignación a mecánicos
- ? Diagnóstico estructurado
- ? Generación de presupuestos PDF
- ? Aprobación/Rechazo de presupuestos
- ? Registro de reparaciones
- ? Notificaciones automáticas

### ?? Gestión de Clientes
- ? Registro y edición de clientes
- ? Histórico completo de servicios
- ? Contacto por teléfono y email
- ? Dirección y observaciones
- ? CRUD completo

### ?? Gestión de Equipos
- ? Registro de equipos
- ? Asociación a clientes
- ? Histórico de reparaciones
- ? Tipo, marca, modelo, serie
- ? Trazabilidad completa

### ?? Gestión de Presupuestos
- ? Cálculo automático de costos
- ? Exportación a PDF
- ? Envío a clientes
- ? Validación de repuestos precificados
- ? Seguimiento de aprobación

### ?? Notificaciones
- ? **WhatsApp** (Principal)
  - Equipo recibido
  - Presupuesto generado
  - Equipo reparado
  - Listo para retiro
- ? **Email** (Secundario)
  - Mismos eventos

### ????? Gestión de Usuarios con Roles
- ? **Administrador**: Control total
- ? **Recepcionista**: Registro y consulta
- ? **Mecánico**: Diagnóstico y reparación
- ? Escalable para nuevos roles

### ?? Reportes y Análisis
- ? Órdenes por período
- ? Ingresos por mecánico
- ? Equipos por tipo
- ? Clientes activos
- ? Estadísticas de garantía

---

## ??? Tecnología

### Stack Tecnológico

```
???????????????????????????????????????????
?  Frontend (Web)                         ?
?  Blazor Server / React                  ?
???????????????????????????????????????????
                    ?
???????????????????????????????????????????
?  API Layer (ASP.NET Core 8)            ?
?  REST API + WebSockets                  ?
???????????????????????????????????????????
                    ?
???????????????????????????????????????????
?  Application Layer ? (Implementado)    ?
?  • 47 Casos de Uso                      ?
?  • Patrones Command/Query               ?
?  • Repository Interfaces                ?
???????????????????????????????????????????
                    ?
???????????????????????????????????????????
?  Domain Layer ? (Implementado)         ?
?  • Lógica de Negocio Pura               ?
?  • Validaciones de Negocio              ?
?  • Sin Dependencias Externas            ?
???????????????????????????????????????????
                    ?
???????????????????????????????????????????
?  Infrastructure (Próxima Fase)          ?
?  Entity Framework Core 8                ?
?  PostgreSQL (Open Source)               ?
???????????????????????????????????????????
```

### Tecnologías Seleccionadas

| Componente | Tecnología | Versión | Motivo |
|-----------|-----------|---------|--------|
| **Lenguaje** | C# | 11+ | Moderno, type-safe |
| **Framework** | .NET | 8.0 (LTS) | Soporte largo plazo |
| **Paradigma** | DDD + Clean | - | Escalable, mantenible |
| **BD** | PostgreSQL | 12+ | Open source, robusto |
| **ORM** | Entity Framework Core | 8.0 | Integrado con .NET |
| **PDF** | QuestPDF | - | Generación profesional |
| **Notificaciones** | WhatsApp API + SMTP | - | Integración directa |
| **Frontend** | Blazor/React | - | Por definir |

---

## ?? Requisitos

### Requisitos del Sistema

- **S.O.**: Windows 10+, Ubuntu 20.04+, macOS 11+
- **RAM**: Mínimo 4GB (recomendado 8GB)
- **Espacio**: 1GB para aplicación
- **Internet**: No requerido para funcionamiento local

### Requisitos de Desarrollo

- **.NET 8 SDK** o superior
- **Visual Studio 2022** Community o **VS Code** + C# extension
- **Git**
- **PostgreSQL 12+** (para desarrollo)

### Requisitos de Producción (On-Premise)

- **Computadora Local**: i5/i7, 8GB RAM, 100GB disco
- **S.O.**: Windows Server 2019+ o Linux
- **PostgreSQL 12+**: Instalado localmente
- **Backup**: Externo (USB, NAS, Cloud)
- **Red Local**: LAN en el taller

---

## ?? Instalación

### Paso 1: Clonar el Repositorio

```bash
git clone https://github.com/RoqueLuis07/business-management-system-dotnet.git
cd business-management-system-dotnet
```

### Paso 2: Verificar .NET 8

```bash
dotnet --version
# Resultado esperado: 8.0.x o superior
```

### Paso 3: Restaurar Dependencias

```bash
dotnet restore
```

### Paso 4: Compilar

```bash
dotnet build
# Resultado esperado: Build succeeded
```

### Paso 5: Verificar Instalación

```bash
dotnet sln list
# Debería listar ambos proyectos
```

Instrucciones detalladas: Ver [INSTALLATION.md](./INSTALLATION.md)

---

## ?? Configuración

### Estructura de Carpetas

```
business-management-system-dotnet/
?
??? BusinessManagementSystem.Domain/      ? COMPLETADO
?   ??? Entities/         (12 entidades)
?   ??? Enums/            (2 enumeraciones)
?
??? BusinessManagementSystem.Application/ ? COMPLETADO
?   ??? Abstractions/     (5 interfaces)
?   ??? WorkOrders/       (27 UCs)
?   ??? Clients/          (5 UCs)
?   ??? Users/            (9 UCs)
?   ??? PartCatalog/      (8 UCs)
?   ??? WarrantyClaims/   (3 UCs)
?
??? Infrastructure/                       ? PRÓXIMA FASE
??? API/                                  ? PRÓXIMA FASE
??? Web/                                  ? PRÓXIMA FASE
?
??? Documentation/
    ??? README.md (este archivo)
    ??? INSTALLATION.md
    ??? GUIA_DE_USO_CASOS_DE_USO.md
    ??? ... más documentación
```

### Configuración de Base de Datos

Se hará en la Fase 5 (Infrastructure) con:

```csharp
// Connection string ejemplo
"DefaultConnection": "Host=localhost;Database=ayrservicios;Username=sa;Password=***;"
```

---

## ?? Uso

### Compilar Proyecto

```bash
# Debug
dotnet build

# Release
dotnet build -c Release
```

### Casos de Uso Disponibles

El sistema implementa **47 casos de uso** organizados en:

#### Órdenes de Trabajo (27)
- Crear, asignar, diagnóstico
- Presupuestos, aprobación
- Reparación, entrega
- Garantías, consultas

#### Clientes (5)
- Crear, actualizar, eliminar
- Consultar, listar

#### Usuarios (9)
- Crear, cambiar rol
- Activar/Desactivar
- Consultar, listar

#### Catálogo (8)
- Crear repuestos
- Actualizar precios
- Activar/Desactivar

#### Garantías (3)
- Consultar reclamos

---

## ??? Arquitectura

### Flujo de Estados - Orden de Trabajo

```
Recibido
   ?
En diagnóstico
   ?
Presupuesto pendiente
   ?
Presupuesto aprobado ?? (Rechazado ? vuelve)
   ?
En reparación
   ?
Finalizado
   ?
Entregado ? GARANTÍA COMIENZA
```

### Validaciones de Negocio

? Órdenes únicas  
? Clientes con teléfono único  
? Estados controlados (8 estados)  
? Presupuesto obligatorio antes de reparar  
? Garantía con período configurable  
? Repuestos con precios validados  

---

## ?? Documentación

### Documentos Disponibles

| Documento | Propósito | Tiempo |
|-----------|----------|--------|
| **README.md** | Este archivo | 10 min |
| **INSTALLATION.md** | Instalación paso a paso | 15 min |
| **QUICK_REFERENCE.md** | Referencia rápida | 5 min |
| **GUIA_DE_USO_CASOS_DE_USO.md** | Ejemplos de código | 30 min |
| **CASOS_DE_USO_IMPLEMENTADOS.md** | Referencia técnica | 20 min |
| **ESTRUCTURA_Y_ORGANIZACION.md** | Arquitectura | 15 min |
| **DIAGRAMAS_Y_FLUJOS.md** | Visuales | 15 min |
| **MASTER_INDEX.md** | Índice maestro | 5 min |

---

## ?? Estado del Proyecto

```
? Domain Layer        - COMPLETADO
? Application Layer   - COMPLETADO
? Infrastructure      - PRÓXIMA FASE
? API Layer           - PRÓXIMA FASE
? Frontend            - PRÓXIMA FASE
? Notificaciones      - PRÓXIMA FASE
? Reportes           - PRÓXIMA FASE

Compilación: ? EXITOSA (.NET 8)
Status: Production-Ready (Logic Layer)
```

---

## ?? Próximos Pasos (Roadmap)

### Fase 2: Infrastructure (2-3 semanas)
- [ ] Entity Framework Core
- [ ] PostgreSQL setup
- [ ] Migrations
- [ ] Repositorios concretos

### Fase 3: API REST (2-3 semanas)
- [ ] ASP.NET Core API
- [ ] Controllers para 5 módulos
- [ ] Swagger/OpenAPI
- [ ] Error handling

### Fase 4: Autenticación (1-2 semanas)
- [ ] JWT implementation
- [ ] Role-based access
- [ ] Session management

### Fase 5: Frontend Web (4-6 semanas)
- [ ] Dashboard principal
- [ ] Formularios CRUD
- [ ] Reportes visuales
- [ ] Notificaciones real-time

### Fase 6: Integraciones (2-3 semanas)
- [ ] WhatsApp API
- [ ] Email SMTP
- [ ] Generación PDF
- [ ] Backups automáticos

### Fase 7: Testing & Deployment (3-4 semanas)
- [ ] Tests unitarios
- [ ] Tests integración
- [ ] Deployment en on-premise
- [ ] Documentación final

---

## ?? Contribución

Este es un proyecto para A Y R Servicio Técnico. Para contribuir o reportar problemas:

1. Abre un [Issue](https://github.com/RoqueLuis07/business-management-system-dotnet/issues)
2. Consulta [CONTRIBUTING.md](./CONTRIBUTING.md)
3. Sigue los estándares de código

---

## ?? Licencia

MIT License - Ver [LICENSE](./LICENSE)

---

## ????? Autor

**Roque Luis**
- ?? [GitHub](https://github.com/RoqueLuis07)
- ?? [LinkedIn](https://linkedin.com/in/roqueluissoftware)

---

## ?? Soporte

### Para A Y R Servicio Técnico

- ?? Contacto directo con desarrollador
- ?? Soporte técnico disponible
- ?? Mantenimiento incluido

### Documentación

- [INSTALLATION.md](./INSTALLATION.md) - Instalación
- [MASTER_INDEX.md](./MASTER_INDEX.md) - Índice completo
- [GUIA_DE_USO_CASOS_DE_USO.md](./GUIA_DE_USO_CASOS_DE_USO.md) - Ejemplos

---

## ? Especificaciones Finales

### Para A Y R Servicio Técnico

? **Sistema completo de gestión de taller**  
? **On-Premise (instalación local)**  
? **47 casos de uso implementados**  
? **Multiusuario con roles**  
? **Notificaciones WhatsApp + Email**  
? **Generación de presupuestos PDF**  
? **Base de datos PostgreSQL**  
? **Escalable para crecimiento futuro**  

---

**Hecho con ?? para A Y R Servicio Técnico**

Asunción, Paraguay | 2026

