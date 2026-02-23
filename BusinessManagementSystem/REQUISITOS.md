# ?? REQUISITOS DEL SISTEMA - A Y R Servicio Técnico

**Documento Oficial de Especificaciones**

---

## 1. INFORMACIÓN GENERAL

### 1.1 Nombre del Negocio
**A Y R Servicio Técnico**

### 1.2 Ubicación
```
Dirección: La Pachos Casicarios N° 277
Barrio: Hipódromo
Ciudad: Asunción
País: Paraguay
```

### 1.3 Contexto Operativo
- **Moneda**: Guaraníes Paraguayos (PYG)
- **Tipo de Operación**: Taller físico local
- **Alcance**: Servicio técnico para zona de Asunción
- **Horas de Operación**: A definir

---

## 2. DESCRIPCIÓN DEL NEGOCIO

A Y R Servicio Técnico es un taller especializado en:

? **Mantenimiento** de equipos técnicos  
? **Diagnóstico** profesional  
? **Reparación** de maquinaria  
? **Servicio integral** desde recepción hasta entrega  

### Servicios Ofrecidos
- Recepción de equipos
- Diagnóstico técnico
- Generación de presupuestos
- Reparación profesional
- Notificación de cliente
- Entrega final con garantía

---

## 3. TIPOS DE EQUIPOS A REPARAR

### 3.1 Equipos de Jardinería
- ? Desmalezadoras
- ? Motosierras
- ? Cortacéspedes
- ? Sopladoras (tipo hojas)

### 3.2 Equipos de Limpieza
- ? Hidrolavadoras (a presión)
- ? Equipos de limpieza general

### 3.3 Equipos de Fumigación
- ? Fumigadoras manuales
- ? Fumigadoras a combustión

### 3.4 Equipos de Generación Eléctrica
- ? Generadores diésel
- ? Generadores nafteros (gasolina)

### 3.5 Equipos Hidráulicos
- ? Bombas de agua (domésticas e industriales)

### 3.6 Otros Equipos Relacionados
- ? Cualquier equipo de:
  - Limpieza
  - Jardinería
  - Generación eléctrica
  - Bombeo de agua
  - Mantenimiento técnico general

---

## 4. CAPACIDAD OPERATIVA

### 4.1 Recursos Humanos
- **Mecánicos Mínimos**: 5
- **Mecánicos Máximos**: 10
- **Escalabilidad**: ? Sistema preparado para crecer

### 4.2 Infraestructura
- **Tipo de Despliegue**: On-Premise (local)
- **Ubicación del Servidor**: En el taller
- **Acceso a Internet**: No obligatorio para funcionamiento
- **Base de Datos**: Local en PostgreSQL

### 4.3 Escalabilidad Futura
- Sistema diseñado para crecer sin limitaciones técnicas
- Posibilidad de migración a Cloud (Azure/AWS)
- Soporte para múltiples sucursales (futuro)

---

## 5. REQUISITOS FUNCIONALES

### 5.1 Gestión de Clientes

#### Funcionalidades Obligatorias
- [x] Registrar nuevos clientes
- [x] Editar información de cliente
- [x] Eliminar clientes (si corresponde)
- [x] Consultar clientes específicos
- [x] Listar todos los clientes
- [x] Ver historial completo de servicios por cliente

#### Datos a Registrar (Mínimo)
- Nombre completo
- Apellido (separado)
- Teléfono (único, validado)
- Email
- Dirección física
- Observaciones/Notas adicionales

#### Datos Adicionales (Sugeridos)
- Fecha de registro
- Última reparación
- Equipos frecuentes
- Preferencias de contacto
- Código de cliente

### 5.2 Gestión de Equipos

#### Funcionalidades Obligatorias
- [x] Registrar equipos nuevos
- [x] Asociar equipos a clientes
- [x] Consultar historial completo de equipos
- [x] Actualizar información de equipos
- [x] Consultar reparaciones por equipo

#### Datos del Equipo (Obligatorios)
- Tipo de equipo (Desplegable: Motosierra, Demalezadora, etc.)
- Marca del fabricante
- Modelo del equipo
- Número de serie (si lo tiene)

#### Datos Adicionales (Opcionales)
- Descripción general
- Año de fabricación
- Observaciones técnicas
- Foto del equipo (futuro)

### 5.3 Gestión de Órdenes de Trabajo

#### Funcionalidades Obligatorias
- [x] Crear órdenes de trabajo (OT)
- [x] Asignar mecánico responsable
- [x] Registrar diagnóstico
- [x] Registrar reparación realizada
- [x] Registrar repuestos utilizados
- [x] Registrar costos
- [x] Actualizar estado de la OT
- [x] Seguimiento completo de la OT
- [x] Generar presupuestos
- [x] Registrar aprobación/rechazo de presupuesto

#### Estados de la Orden de Trabajo (Obligatorios)

**8 Estados Principales:**

1. **Recibido**
   - Equipo ingresó al taller
   - Cliente registrado
   - Datos iniciales cargados

2. **En Diagnóstico**
   - Mecánico analiza el equipo
   - Identifica problemas
   - Anota hallazgos

3. **Presupuesto Pendiente**
   - Diagnóstico completado
   - Presupuesto en elaboración
   - Cliente esperando

4. **Presupuesto Aprobado**
   - Cliente aprobó presupuesto
   - Autorización para reparar
   - Listo para reparación

5. **Presupuesto Rechazado**
   - Cliente rechazó presupuesto
   - OT en espera de decisión
   - Posible: Nueva cotización o retiro

6. **En Reparación**
   - Trabajo técnico en progreso
   - Mecánico trabajando
   - Se usan repuestos

7. **Finalizado**
   - Reparación completada
   - Control de calidad pasado
   - Listo para entrega

8. **Entregado**
   - Cliente retiró equipo
   - Pago realizado
   - **AQUÍ COMIENZA GARANTÍA**

### 5.4 Generación de Presupuestos

#### Requisito: ? OBLIGATORIO

#### Funcionalidades
- [x] Crear presupuestos desde OT
- [x] Calcular automáticamente costos:
  - Labor/mano de obra
  - Costo de repuestos
  - Total general
- [x] Incluir detalles de:
  - Diagnóstico
  - Trabajo a realizar
  - Repuestos necesarios
  - Plazo estimado

#### Exportación y Distribución
- [x] Exportar a PDF profesional
- [x] Formato: A4 / Carta
- [x] Incluir:
  - Logo/Membrete del taller
  - Número de OT
  - Datos del cliente
  - Descripción de trabajo
  - Costos desglosados
  - Total a pagar
  - Período de validez

#### Envío a Cliente
- [x] Enviar vía WhatsApp (Preferido)
- [x] Enviar vía Email (Alternativo)
- [x] Registrar envío en sistema

### 5.5 Notificaciones

#### Requisito: ? OBLIGATORIO

#### Canal Principal: WhatsApp
- [x] Integración con WhatsApp API
- [x] Mensajes automáticos para:

**1. Equipo Recibido**
```
"Hola! Recibimos tu [EQUIPO]. 
OT: #[NUMERO]
Te avisaremos cuando tengamos diagnóstico."
```

**2. Presupuesto Generado**
```
"Tu presupuesto está listo!
OT: #[NUMERO]
Te enviamos el PDF con los detalles.
¿Lo aprobas?"
```

**3. Equipo Reparado**
```
"¡Tu [EQUIPO] está reparado!
OT: #[NUMERO]
Puedes pasar a retirarlo en horario de atención."
```

**4. Listo para Retiro**
```
"Tu equipamiento está listo para retirar.
Ubicación: [DIRECCIÓN]
Horario: [HORARIO]"
```

#### Canal Secundario: Email
- [x] SMTP configurado
- [x] Mismos eventos que WhatsApp
- [x] Cuerpo html profesional
- [x] Adjuntos (PDF de presupuesto)

#### Configuración Técnica
- [ ] Números de teléfono almacenados cifrados
- [ ] Logging de envíos
- [ ] Reintentos automáticos
- [ ] Validación de números

---

## 6. REQUISITOS NO FUNCIONALES

### 6.1 Base de Datos

#### Motor Seleccionado: PostgreSQL
**Razones:**
- ? Open Source (sin costo de licencia)
- ? Alto rendimiento
- ? Muy seguro
- ? Escalable horizontalmente
- ? Soporte en Linux/Windows

#### Especificaciones
- **Versión Mínima**: PostgreSQL 12
- **Versión Recomendada**: PostgreSQL 14+
- **Almacenamiento**: Mínimo 50GB (escalable)
- **Backups**: Automáticos diarios

### 6.2 Despliegue

#### Tipo de Instalación: ON-PREMISE

**Significa:**
- ? Instalado en computadora local (del taller)
- ? Base de datos local
- ? Funcionamiento sin Internet (excepto notificaciones)
- ? Control total sobre datos
- ? Privacidad garantizada

#### Infraestructura Mínima
```
Computadora Principal:
??? Procesador: Intel i5/i7 o AMD Ryzen 5/7
??? RAM: Mínimo 8GB
??? Disco: SSD 500GB mínimo
??? S.O.: Windows Server 2019+ o Linux Ubuntu 20.04+
??? Red: LAN local
??? Backup: USB/NAS externo
```

#### Escalabilidad Futura
- [x] Preparado para migración a Cloud (Azure/AWS)
- [x] Posibilidad de múltiples sucursales
- [x] Puede crecer sin rediseño

---

## 7. USUARIOS DEL SISTEMA

### 7.1 Administrador
**Acceso**: Completo  
**Responsabilidades**: Gestión total del sistema

#### Permisos
- ? Gestión de usuarios (crear, editar, eliminar)
- ? Gestión de clientes
- ? Gestión de equipos
- ? Gestión de órdenes de trabajo
- ? Gestión de presupuestos
- ? Acceso a reportes
- ? Configuración del sistema
- ? Respaldo de datos

#### Usuarios Típicos
- Dueño del taller
- Gerente del taller
- Persona de confianza

### 7.2 Recepcionista
**Acceso**: Limitado a recepción y consulta  
**Responsabilidades**: Atención al cliente, entrada de datos

#### Permisos
- ? Registrar nuevos clientes
- ? Registrar equipos
- ? Crear órdenes de trabajo
- ? Consultar estado de órdenes
- ? Ver historial de cliente
- ? Imprimir comprobantes
- ? Registrar entregas

#### Restricciones
- ? No puede administrar usuarios
- ? No puede eliminar órdenes
- ? No puede cambiar precios
- ? No puede ver reportes financieros

### 7.3 Mecánico
**Acceso**: Específico a sus reparaciones  
**Responsabilidades**: Diagnóstico y reparación

#### Permisos
- ? Ver órdenes asignadas
- ? Registrar diagnóstico
- ? Registrar reparaciones
- ? Agregar repuestos usados
- ? Actualizar estado de OT
- ? Ver historial de equipos

#### Restricciones
- ? No puede gestionar usuarios
- ? No puede eliminar órdenes
- ? No puede ver reportes
- ? No puede cambiar precios (solo admin)
- ? No puede crear presupuestos (solo admin)

---

## 8. CARACTERÍSTICAS CLAVE REQUERIDAS

El sistema deberá proporcionar:

- [x] **Gestión Completa del Taller**
  - Desde recepción hasta entrega
  
- [x] **Histórico Completo por Cliente**
  - Todos los servicios realizados
  - Equipos reparados
  - Costos y garantías
  
- [x] **Histórico Completo por Equipo**
  - Reparaciones anteriores
  - Repuestos usados
  - Diagnósticos
  - Garantías aplicadas
  
- [x] **Generación de Presupuestos PDF**
  - Formato profesional
  - Cálculos automáticos
  - Envío a cliente
  
- [x] **Sistema Multiusuario**
  - Roles diferenciados
  - Control de acceso
  - Auditoria de cambios
  
- [x] **Control de Estados**
  - 8 estados bien definidos
  - Transiciones validadas
  - Prevención de errores
  
- [x] **Notificaciones Automáticas**
  - WhatsApp principal
  - Email secundario
  - Configurables
  
- [x] **Sistema Escalable**
  - Crece con el taller
  - Soporta hasta 10+ mecánicos
  - Preparado para Cloud

---

## 9. ARQUITECTURA TÉCNICA

### Stack Recomendado (APROBADO)

#### Backend
- **Lenguaje**: C# 11+
- **Framework**: .NET 8 (LTS - soporte hasta 2026)
- **Arquitectura**: Clean Architecture + DDD
- **ORM**: Entity Framework Core 8

#### Base de Datos
- **Motor**: PostgreSQL 12+
- **Característica**: Open Source
- **Seguridad**: Encriptación de datos sensibles

#### Frontend (Por definir)
- **Opción A**: Blazor Server
  - Full .NET stack
  - Real-time updates
  - Ideal para aplicación de negocio
  
- **Opción B**: React + .NET API
  - Modern UI
  - Separación clara
  - Más flexible

#### Generación de PDF
- **Librería**: QuestPDF
- **Formato**: PDF profesional
- **Características**: Gráficos, tablas, estilos

#### Notificaciones
- **WhatsApp**: WhatsApp Business API
- **Email**: SMTP (servidor local o externo)
- **Logging**: Sistema de eventos

---

## 10. OBJETIVO DEL SISTEMA

Desarrollar un sistema que permita:

? **Digitalizar** la gestión del taller  
? **Optimizar** procesos operativos  
? **Mejorar** el control de órdenes  
? **Organizar** información de clientes  
? **Seguimiento** automático de reparaciones  
? **Aumentar** productividad del equipo  
? **Reducir** errores manuales  
? **Profesionalizar** la atención al cliente  

---

## 11. ALCANCE DEL PROYECTO

### Inicio: Recepción del Equipo
```
Cliente llega con equipo al taller
    ?
Recepcionista registra cliente y equipo
    ?
Se crea orden de trabajo (OT)
```

### Fin: Entrega al Cliente
```
Equipo reparado y probado
    ?
Cliente retira y paga
    ?
Comienza período de garantía
```

### Incluye Completamente
- [x] Diagnóstico técnico
- [x] Generación de presupuestos
- [x] Aprobación/rechazo
- [x] Reparación
- [x] Registro de repuestos
- [x] Notificación a cliente
- [x] Entrega con garantía
- [x] Seguimiento post-venta

### Excluye (Por ahora)
- ? Sistema de contabilidad
- ? Gestión de inventario avanzada
- ? Análisis financiero profundo
- ? Integración con puntos de venta

---

## 12. CONCLUSIÓN

Este proyecto corresponde a:

### **Sistema de Gestión de Taller Profesional**

? Preparado para uso en entorno real  
? Responde a necesidades específicas del taller  
? Escalable para crecimiento futuro  
? Implementado con arquitectura moderna  
? Documentado completamente  

---

## 13. APROBACIÓN

### Especificaciones Aprobadas por:

- **Nombre**: [Por completar]
- **Puesto**: [Por completar]
- **Fecha**: [Por completar]
- **Firma**: [Por completar]

---

**Documento de Requisitos - A Y R Servicio Técnico**  
**Asunción, Paraguay**  
**Versión 1.0 - Enero 2026**
