# ?? GUÍA DE INICIO RÁPIDO - A Y R Servicio Técnico

**Para: Equipo del taller**  
**Objetivo: Entender el sistema en 30 minutos**

---

## ¿QUÉ ES ESTO?

Este es un **sistema de gestión de taller profesional** diseñado específicamente para **A Y R Servicio Técnico** en Asunción, Paraguay.

### Funciona para:
? Registrar clientes  
? Registrar equipos a reparar  
? Crear órdenes de trabajo  
? Registrar diagnósticos  
? Generar presupuestos (PDF)  
? Enviar presupuestos por WhatsApp  
? Registrar reparaciones  
? Entregar equipos con garantía  

---

## USUARIOS DEL SISTEMA

### ????? Administrador (Dueño/Gerente)
**Qué puede hacer:**
- Acceso total al sistema
- Gestionar usuarios
- Ver todos los datos
- Generar reportes
- Configurar el sistema

**Responsable de:** Supervisión general

### ????? Recepcionista
**Qué puede hacer:**
- Registrar clientes nuevos
- Registrar equipos
- Crear órdenes de trabajo
- Ver estado de órdenes
- Registrar entregas

**Responsable de:** Atención al cliente, entrada de datos

### ?? Mecánico
**Qué puede hacer:**
- Ver sus órdenes asignadas
- Registrar diagnósticos
- Registrar reparaciones
- Agregar repuestos usados
- Actualizar estado

**Responsable de:** Diagnóstico y reparación

---

## FLUJO TÍPICO DE UNA REPARACIÓN

### PASO 1: Cliente llega con equipo

```
Cliente: "Necesito reparar mi motosierra"
                    ?
        Recepcionista registra al cliente
                    ?
        Recepcionista registra el equipo
                    ?
        Sistema crea ORDEN DE TRABAJO (OT)
```

**Estado**: ?? Recibido

### PASO 2: Diagnóstico

```
Mecánico: "Voy a revisar qué tiene"
                    ?
        Mecánico examina el equipo
                    ?
        Mecánico registra lo que encontró
                    ?
        Sistema guarda el diagnóstico
```

**Estado**: ?? En diagnóstico ? Presupuesto Pendiente

### PASO 3: Presupuesto

```
Admin: "Calculo cuánto cuesta reparar"
                    ?
        Admin registra repuestos necesarios
                    ?
        Admin suma costos
                    ?
        Sistema genera PRESUPUESTO (PDF)
                    ?
        ¡Se envía por WHATSAPP al cliente!
```

**Estado**: ?? Presupuesto Pendiente

### PASO 4: Aprobación

```
Cliente: "Sí, quiero que lo reparen" (por WhatsApp)
                    ?
        Admin marca presupuesto como APROBADO
                    ?
        Sistema notifica al taller
```

**Estado**: ?? Presupuesto Aprobado

### PASO 5: Reparación

```
Mecánico: "Ahora sí reparo el equipo"
                    ?
        Mecánico trabaja en el equipo
                    ?
        Mecánico registra trabajo realizado
                    ?
        Mecánico prueba que funciona
```

**Estado**: ?? En reparación ? Finalizado

### PASO 6: Entrega

```
Cliente: "Vengo a buscar mi equipo"
                    ?
        Recepcionista verifica que funcione
                    ?
        Recepcionista registra entrega
                    ?
        Cliente paga y se va
                    ?
        ¡GARANTÍA COMIENZA AQUÍ!
```

**Status**: ?? Entregado

**LA GARANTÍA EMPIEZA 30 DÍAS (o más si lo decide) DESDE LA ENTREGA**

---

## ESTADOS DE UNA ORDEN

```
?? Recibido
   ? (Mecánico revisa)
?? En diagnóstico
   ? (Se identifica problema)
?? Presupuesto Pendiente
   ?
   ??? ?? Presupuesto Rechazado (cliente dice que no)
   ?         ? (Admin hace nuevo presupuesto)
   ?         ?? Presupuesto Pendiente (nuevamente)
   ?
   ??? ?? Presupuesto Aprobado (cliente dice que sí)
           ? (Mecánico empieza a reparar)
       ?? En reparación
           ? (Termina el trabajo)
       ?? Finalizado
           ? (Cliente retira)
       ?? Entregado ? AQUÍ COMIENZA GARANTÍA
```

---

## EJEMPLOS DE USO

### Ejemplo 1: Crear una nueva orden

**1. Recepcionista abre el sistema**
```
Clientes ? Nuevo cliente
??? Nombre: Juan García
??? Apellido: López
??? Teléfono: 0972123456 ? Único importante
??? Email: juan@email.com
??? Dirección: Calle Principal 123
```

**2. Registra el equipo**
```
Equipos ? Nuevo equipo
??? Tipo: Motosierra
??? Marca: Stihl
??? Modelo: MS 210
??? Serie: ABC123456
```

**3. Crea la orden**
```
Órdenes ? Nueva orden
??? Cliente: Juan García López
??? Equipo: Stihl MS 210
??? Descripción: "No enciende, falla el motor"
??? Mecánico asignado: Carlos
??? Estado: Recibido
```

### Ejemplo 2: Registrar diagnóstico

**Mecánico Carlos:**
```
Mi orden ? OT-2024-001234
??? Hallazgos: "Bujía rota, filtro de aire sucio"
??? Trabajo recomendado: "Cambiar bujía, limpiar filtro, revisar carburador"
??? Repuestos necesarios: 
?   ??? Bujía SP-12: 150.000 Gs
?   ??? Filtro aire: 80.000 Gs
??? Guardar
```

**Sistema actualiza estado:**
- De: ?? En diagnóstico
- A: ?? Presupuesto Pendiente

### Ejemplo 3: Generar presupuesto

**Admin:**
```
OT-2024-001234
??? Diagnóstico: ? Leído
??? Mano de obra: 200.000 Gs
??? Bujía: 150.000 Gs
??? Filtro: 80.000 Gs
??? TOTAL: 430.000 Gs
??? Generar PDF
??? Enviar por WhatsApp al cliente
```

**Cliente recibe por WhatsApp:**
```
"Hola Juan! Tu motosierra está diagnosticada.
OT: #2024-001234
Total: 430.000 Gs
¿Autorizas la reparación?"
```

---

## ESTADOS EN DETALLE

### ?? Recibido
- Cliente acaba de dejar el equipo
- Se registró en el sistema
- Espera diagnóstico

### ?? En Diagnóstico
- Mecánico está revisando qué tiene
- Identifica problemas
- Anota hallazgos

### ?? Presupuesto Pendiente
- Diagnóstico hecho
- Se calculó costo total
- Cliente espera para decidir

### ?? Presupuesto Rechazado
- Cliente dijo "NO"
- Equipo está listo para retiro sin reparación
- O se espera otro presupuesto

### ?? Presupuesto Aprobado
- Cliente dijo "SÍ"
- Se autoriza la reparación
- Mecánico empieza a trabajar

### ?? En Reparación
- Mecánico está reparando
- Se están usando repuestos
- Puede llevar horas o días

### ?? Finalizado
- Reparación lista
- Equipo fue probado
- Cliente puede venir a buscar

### ?? Entregado
- Cliente retiró el equipo
- Pagó el servicio
- **¡GARANTÍA COMIENZA AQUÍ!**

---

## GARANTÍA EXPLICADA

### ¿Qué es la Garantía?

Es una promesa: **"Si falla lo que reparé, lo arreglo gratis"**

### ¿Cuándo empieza?
**Cuando el cliente retira el equipo reparado**

### ¿Cuánto tiempo dura?
**Por defecto: 30 días**  
(Pero puede ser más: 60, 90 días, etc.)

### Ejemplo:

```
Lunes 15/01/2024: Cliente retira motosierra reparada
                  Estado: Entregado
                  Garantía: 30 días

Martes 16/01: El reloj de garantía comienza
              (La cuenta regresiva empieza)

Jueves 14/02: Garantía se vence (30 días después)

DURANTE ESTOS 30 DÍAS:
? Si la motosierra falla ? Reparación GRATIS
? Después de 30 días ? Hay que cobrar

¿Cómo registramos que falló?
? Se crea una NUEVA ORDEN vinculada a la original
? Sistema sabe que es garantía
? Se puede hacer gratis
```

---

## NOTIFICACIONES (WhatsApp + Email)

El sistema envía automáticamente mensajes en estos momentos:

### 1?? Equipo Recibido
```
"¡Hola Juan! Recibimos tu motosierra.
OT: #2024-001234
Te avisaremos cuando tengamos diagnóstico.
Gracias por confiar en nosotros."
```

### 2?? Presupuesto Generado
```
"Tu presupuesto está listo!
OT: #2024-001234
Total: 430.000 Gs
Adjuntamos PDF con detalles.
¿Lo aprobas?"
```

### 3?? Equipo Reparado
```
"¡Tu motosierra está reparada y funcionando!
OT: #2024-001234
Puedes pasar a buscarla.
Horario: Lunes a Viernes 8am-6pm"
```

### 4?? Listo para Retiro
```
"Tu equipamiento está listo para retirar.
Ubicación: La Pachos Casicarios N° 277
Horario: L-V 8:00 - 18:00
Sábado 9:00 - 13:00"
```

---

## REPUESTOS

### Tipos de Repuestos

#### 1. Del Catálogo
```
Repuestos que registramos una vez:
??? Bujía SP-12: 150.000 Gs
??? Filtro aire: 80.000 Gs
??? Cadena motosierra: 320.000 Gs
??? Aceite 2T: 45.000 Gs

Cuando usamos uno:
? Sistema sugiere el precio guardado
? Se puede cambiar si es diferente
? Se guarda automáticamente
```

#### 2. Manuales (No en catálogo)
```
Repuestos que NO están en lista:
"Cable de arranque Stihl MS 200"

? Recepcionista lo agrega manualmente
? Pone el precio que costó
? Se guarda para próximas veces
```

---

## ATAJOS Y TIPS

### Búsquedas Rápidas
```
¿Dónde está la orden de Juan?
? Buscar "Juan" o "OT-2024-001234"

¿Cuántas órdenes tiene Carlos?
? Ver "Órdenes por mecánico" ? Carlos

¿Qué órdenes están esperando presupuesto?
? Estado: "Presupuesto Pendiente"
```

### Datos Importantes

**TELÉFONO es ÚNICO**
```
No puedes registrar dos clientes con el mismo teléfono
?
Sistema previene errores
```

**NÚMERO DE ORDEN es ÚNICO**
```
Cada orden tiene un número único: OT-2024-001234
?
Sistema lo genera automáticamente
?
Se usa en presupuestos, WhatsApp, emails
```

**EMAIL OPCIONAL pero útil**
```
Con email: Envías presupuesto por email también
Sin email: Solo WhatsApp
```

---

## ERRORES COMUNES Y SOLUCIONES

### ? "No puedo crear orden, me dice error"
**Soluciones:**
1. ¿El cliente está registrado?
2. ¿El equipo está registrado?
3. ¿Hay un mecánico disponible?

### ? "No puedo generar presupuesto"
**Soluciones:**
1. ¿El diagnóstico está completado?
2. ¿Todos los repuestos tienen precio?
3. ¿La mano de obra está anotada?

### ? "El WhatsApp no se envía"
**Soluciones:**
1. ¿El teléfono está correctamente guardado?
2. ¿Es un teléfono de Paraguay (0972...)?
3. ¿Verificaste que Internet está activo?

### ? "¿Cómo creo una segunda cotización?"
**Solución:**
Sistema permite múltiples presupuestos
1. Rechaza el anterior
2. Crea uno nuevo
3. El cliente puede ver ambos

---

## SEGURIDAD Y PRIVACIDAD

### Datos del Cliente
? Guardados localmente (en el taller)  
? No se envían a servidores externos  
? Encriptados en la base de datos  
? Respaldados automáticamente  

### Contraseña
? Cambiar periódicamente  
? No compartir con otros  
? Si olvidas: Pregunta al Admin  

### Logout
? Cerrá sesión al terminar el día  
? No dejar la computadora sin cerrrar  
? Más seguridad = Menos errores  

---

## REPORTES ÚTILES

### Para el Dueño
```
? OT por mecánico ? Ver productividad
? Ingresos por período ? Ver ganancias
? OT por cliente ? Ver clientes frecuentes
? Estadísticas de garantía ? Ver problemas recurrentes
```

### Para la Recepción
```
? OT pendiente de diagnóstico ? Saber qué priorizar
? OT pendiente de presupuesto ? Cliente esperando
? Clientes activos ? Historia de contacto
```

### Para Mecánicos
```
? Mis órdenes ? Ver qué tengo que hacer
? OT en reparación ? Control de progreso
```

---

## CONTACTO Y SOPORTE

### Dudas del Sistema
?? Contacta al Administrador  
?? O al desarrollador (ver README.md)  

### Problemas Técnicos
?? Reinicia la aplicación  
??? Si persiste, contacta soporte  

---

## RESUMEN FINAL

| Rol | Acciones | Responsabilidad |
|-----|----------|-----------------|
| **Admin** | Todo | Supervisión general |
| **Recepcionista** | CRUD clientes, equipos, órdenes | Entrada de datos |
| **Mecánico** | Diagnóstico, reparación | Trabajo técnico |

| Documento | Para | Dónde |
|-----------|------|-------|
| **REQUISITOS.md** | Entender qué hay | Documentación |
| **SPEC_TECNICA.md** | Detalles técnicos | Documentación |
| **GUIA_DE_USO_CASOS_DE_USO.md** | Ejemplos de código | Documentación |

---

## AHORA QUÉ?

? **Instalación**: Ejecuta `dotnet build` (compilar)  
? **Próxima Fase**: Crear base de datos (Fase 5)  
? **Luego**: API y frontend web  
? **Final**: Sistema completo en producción  

---

**¡Bienvenido a tu sistema de gestión!** ??

**A Y R Servicio Técnico**  
**Asunción, Paraguay**  
**Enero 2026**
