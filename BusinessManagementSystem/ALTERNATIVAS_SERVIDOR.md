# ??? ALTERNATIVAS DE SERVIDOR SIMPLE - A Y R Servicio Técnico

**Guía de opciones más simples que Linux para desplegar tu sistema**

---

## ?? Tabla de Contenidos

- [Comparativa de Opciones](#comparativa-de-opciones)
- [Opción 1: Windows Server (Recomendado)](#opción-1-windows-server-recomendado)
- [Opción 2: Solo Windows 11/10 Pro](#opción-2-solo-windows-1110-pro)
- [Opción 3: Azure (Cloud Simple)](#opción-3-azure-cloud-simple)
- [Opción 4: DigitalOcean App Platform](#opción-4-digitalocean-app-platform)
- [Opción 5: Replit (La más sencilla)](#opción-5-replit-la-más-sencilla)
- [Comparación Final](#comparación-final)

---

## ?? Comparativa de Opciones

| Opción | Complejidad | Costo | Dificultad | Ideal Para |
|--------|------------|-------|-----------|-----------|
| **Windows Server** | Medio | Medio-Alto | Media | Producción profesional |
| **Windows 10/11 Pro** | Muy Baja | $0 | Muy fácil | Desarrollo/Local |
| **Azure App Service** | Baja | Bajo-Medio | Fácil | Cloud moderno |
| **DigitalOcean** | Baja-Media | Bajo | Media | Cloud económico |
| **Replit** | Muy Baja | Bajo | Muy fácil | Testing rápido |
| **Docker Desktop** | Media | $0 | Media | Desarrollo |

---

## ?? OPCIÓN 1: Windows Server (Recomendado)

### ¿Por qué es la mejor para A Y R?

```
? Interfaz gráfica familiar (como Windows normal)
? IIS integrado (no necesitas aprender nginx)
? SQL Server o PostgreSQL fácil de instalar
? Entorno profesional
? No necesitas conocimiento de Linux
? .NET está optimizado para Windows
```

### Hardware Mínimo

```
Procesador: Intel i5/i7 o Ryzen 5/7
RAM: 8GB mínimo
Disco: SSD 500GB
Sistema: Windows Server 2022 Standard
Conexión: LAN local estable
```

### Costo

```
Windows Server 2022 Standard: $500-1000 USD (compra única)
O: $15-20 USD/mes en Azure
```

### Instalación (Paso a Paso)

**Paso 1: Instalar Windows Server 2022**

```powershell
# En una computadora nueva o máquina virtual
# Descargar desde: https://www.microsoft.com/en-us/windows-server

# O en Azure (más sencillo):
# 1. Ir a portal.azure.com
# 2. Create Resource > Windows Server 2022
# 3. Seleccionar B2s (2 vCPU, 4GB RAM) = $50/mes
```

**Paso 2: Instalar IIS**

```powershell
# En PowerShell como Administrador
dism.exe /online /enable-feature /featurename:IIS-WebServerRole /all
dism.exe /online /enable-feature /featurename:IIS-ASPNET45 /all
dism.exe /online /enable-feature /featurename:IIS-ApplicationInit /all

# Verificar (abre navegador)
# http://localhost/
# Deberías ver página de bienvenida de IIS
```

**Paso 3: Instalar .NET 8 Hosting Bundle**

```powershell
# Descargar desde:
# https://dotnet.microsoft.com/download/dotnet/8.0
# Buscar "Hosting Bundle for Windows"

# O descargarlo directamente:
# https://aka.ms/dotnet/8.0/windowshosting

# Ejecutar el instalador .exe
# Reiniciar el servidor después
```

**Paso 4: Instalar PostgreSQL**

```powershell
# Descargar desde: https://www.postgresql.org/download/windows/

# Versión recomendada: PostgreSQL 15 o superior
# Instalación:
# 1. Ejecutar instalador
# 2. Establecer contraseña (importante: guardar)
# 3. Puerto: 5432 (dejar por defecto)
# 4. Usar pgAdmin para administrar
```

**Paso 5: Publicar tu aplicación**

```powershell
# En tu computadora de desarrollo:
dotnet publish -c Release -o "C:\temp\publish"

# Copiar carpeta "publish" al servidor
# En el servidor:
# 1. Crear carpeta: C:\inetpub\wwwroot\ayr-sistema
# 2. Copiar contenido de "publish" ahí
# 3. Abrir IIS Manager (inetmgr)
# 4. Crear nuevo sitio web:
#    - Nombre: AYR-Sistema
#    - Ruta: C:\inetpub\wwwroot\ayr-sistema
#    - Puerto: 80 (o 443 para HTTPS)
# 5. Asignar Application Pool (.NET CLR v4.0)
```

**Paso 6: Configurar Connection String**

```powershell
# En servidor, editar: appsettings.json
# Cambiar:
# "DefaultConnection": "Host=localhost;Database=ayr_servicio;Username=postgres;Password=TU_PASSWORD;"
```

**Paso 7: Crear base de datos**

```powershell
# En pgAdmin (interfaz web de PostgreSQL):
# 1. Abrir http://localhost:5050
# 2. Conectarse con usuario postgres
# 3. Crear nueva base de datos: ayr_servicio
# 4. Ejecutar migrations desde tu aplicación

# O desde línea de comandos:
# dotnet ef database update
```

### Acceder desde el taller

```
PC Mecánico: http://NOMBRE-SERVIDOR/ayr-sistema
PC Recepción: http://NOMBRE-SERVIDOR/ayr-sistema
PC Admin: http://NOMBRE-SERVIDOR/ayr-sistema

O si está en cloud:
http://tu-dominio.com
http://IP-DEL-SERVIDOR
```

### Ventajas vs Linux

```
? Interfaz gráfica (no línea de comandos)
? IIS es más fácil que nginx para principiantes
? SQL Server integrado si lo quieres
? Antivirus Windows incluido
? Actualizaciones automáticas
? Soporte profesional disponible
```

### Desventajas

```
? Más caro que Linux
? Requiere licencia (o Azure)
? Usa más recursos
```

---

## ?? OPCIÓN 2: Solo Windows 11/10 Pro (La más SIMPLE)

### ¿Cuándo usarla?

```
? Para testing durante desarrollo
? Si solo 1-2 personas usan el sistema
? Para demostración al cliente
? No necesitas servidor separado
?? NO para producción con múltiples usuarios
```

### Costo

```
$0 - Usa tu PC actual
Solo necesitas Windows 11/10 Pro (si no la tienes)
```

### Instalación (5 minutos)

**Paso 1: Instalar PostgreSQL localmente**

```powershell
# Descargar: https://www.postgresql.org/download/windows/
# Seguir asistente de instalación
# Anotar contraseña de postgres
```

**Paso 2: Configurar connection string**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ayr_servicio;Username=postgres;Password=tu_password;"
  }
}
```

**Paso 3: Ejecutar la aplicación**

```powershell
cd "C:\ruta\a\tu\proyecto"
dotnet build
dotnet run

# O si tienes el API:
# Salida esperada: http://localhost:5000/
```

**Paso 4: Acceder desde otra PC en la red**

```
En otro equipo de la red:
http://IP-DE-TU-PC:5000

Para saber tu IP:
ipconfig /all
(Buscar IPv4 Address)
```

### Ventajas

```
? Cero instalación de servidor
? Funciona en tu PC actual
? Muy fácil de actualizar
? Gratis
? Ideal para testing
```

### Desventajas

```
? Si tu PC se apaga, el sistema cae
? No es profesional para producción
? Limitado a 1-2 usuarios simultáneos
? Si la PC se reinicia, se pierde acceso
```

---

## ?? OPCIÓN 3: Azure (Cloud Simple)

### ¿Por qué Azure?

```
? Lo usa Microsoft
? Interfaz gráfica en navegador
? No necesitas servidor físico
? Escala automáticamente
? Backups automáticos
? SSL/HTTPS gratis
? 30 días gratis para probar
```

### Costo Estimado

```
Azure App Service (B1S): $11 USD/mes
PostgreSQL Server: $50-100 USD/mes
Total: ~$70 USD/mes (~1,200,000 Gs)
```

### Instalación (20 minutos)

**Paso 1: Crear cuenta en Azure**

```
1. Ir a https://portal.azure.com
2. Click "Start free"
3. Registrar con email
4. Agregar tarjeta (solo validación, no cobra)
```

**Paso 2: Crear App Service**

```
En Azure Portal:
1. Click "Create a resource"
2. Buscar "App Service"
3. Click "Create"
4. Llenar:
   - Name: ayr-sistema
   - Runtime: .NET 8
   - Region: East (Brazil o USA)
   - Pricing: B1S (más barato)
5. Click "Create"
```

**Paso 3: Crear PostgreSQL**

```
1. Create Resource > Azure Database for PostgreSQL
2. Llenar:
   - Server name: ayr-db
   - Username: postgres
   - Password: (guardar!)
   - Region: Mismo que App Service
   - Pricing: Basic
3. Click "Create"
```

**Paso 4: Publicar tu aplicación**

```powershell
# En tu PC:
dotnet publish -c Release

# En Azure Portal:
# App Service > Deployment Center
# Conectar GitHub
# O usar ZIP Deploy:
# 1. Crear ZIP de carpeta publish
# 2. Deploy > Upload
```

**Paso 5: Configurar Connection String**

```
En Azure Portal:
App Service > Configuration > Application settings
Agregar:
Name: ConnectionStrings__DefaultConnection
Value: Host=ayr-db.postgres.database.azure.com;Database=ayr_servicio;Username=postgres@ayr-db;Password=TU_PASSWORD;SSL Mode=Require;
```

### Ventajas

```
? Acceso desde cualquier lugar (internet)
? Backups automáticos
? SSL/HTTPS gratis
? Escalable si crece
? No necesitas mantenimiento
? Microsoft lo mantiene
```

### Desventajas

```
? Requiere conexión internet constante
? Costo mensual ($70 USD aprox)
? Más complejo que opción local
```

---

## ?? OPCIÓN 4: DigitalOcean App Platform

### ¿Por qué DigitalOcean?

```
? Más barato que Azure
? Muy simple de usar
? Perfecto para .NET + PostgreSQL
? App Platform (sin Docker)
? Excelente documentación
? Soporte en español
```

### Costo Estimado

```
App Platform: $12 USD/mes
PostgreSQL: $15 USD/mes
Total: ~$27 USD/mes (~460,000 Gs)
```

### Instalación (15 minutos)

**Paso 1: Crear cuenta DigitalOcean**

```
1. Ir a https://www.digitalocean.com
2. Click "Sign up"
3. Registrarse
4. Agregar tarjeta de crédito
```

**Paso 2: Crear App**

```
En DigitalOcean Dashboard:
1. Click "Create" > "Apps"
2. Conectar GitHub
3. Seleccionar tu repositorio
4. Esperar que detecte .NET 8
5. DigitalOcean configura automáticamente
```

**Paso 3: Crear PostgreSQL**

```
1. Click "Create" > "Databases" > "PostgreSQL"
2. Llenar:
   - Name: ayr-db
   - Region: Sao Paulo o Miami
   - Version: 15
3. Click "Create"
```

**Paso 4: Conectar BD con App**

```
En DigitalOcean:
1. App > Resources > Add Resource
2. Seleccionar PostgreSQL database
3. Automáticamente obtiene connection string
```

### Ventajas

```
? MÁS BARATO ($27 vs $70 en Azure)
? Muy simple (solo GitHub + click)
? PostgreSQL integrado
? Deployments automáticos
? Buena documentación en español
```

### Desventajas

```
? Menos opciones que Azure
? Soporte no 24/7
```

---

## ?? OPCIÓN 5: Replit (La más SENCILLA)

### ¿Para qué?

```
? Testing rápido sin servidor
? Demostración al cliente
? No necesitas instalar nada
? NO para producción
```

### Costo

```
Gratuito (con limitaciones)
Premium: $7 USD/mes
```

### Cómo funciona (3 pasos)

**Paso 1: Subir código**

```
1. Ir a https://replit.com
2. Click "Create" > "Import from GitHub"
3. Conectar tu repositorio
4. Esperar que Replit lo prepare
```

**Paso 2: Instalar dependencias**

```
Replit automáticamente instala:
- .NET 8
- PostgreSQL
- Todo lo necesario
```

**Paso 3: Ejecutar**

```
Click "Run"
Replit abre URL pública: https://tu-proyecto-replit.com
¡Listo! Tu sistema está online
```

### Ventajas

```
? Cero instalación
? URL pública inmediata
? Ideal para mostrar al cliente
? Gratis para empezar
```

### Desventajas

```
? Limitado (1 GB RAM)
? No es producción real
? Se duerme si no se usa
? Limitado a 500 MB/mes en plan gratuito
```

---

## ?? MI RECOMENDACIÓN PARA A Y R SERVICIO TÉCNICO

### Mejor Opción: **Windows Server en Azure (B2s)**

**Por qué:**
```
? Costo: ~$50 USD/mes (~850,000 Gs)
? Interfaz gráfica (familiar para ti)
? IIS es simple (no necesitas Linux)
? PostgreSQL instalable en Windows
? Profesional para producción
? Escalable si crece el taller
? No necesitas mantenimiento (Azure lo hace)
```

### Alternativa (Más económica): **DigitalOcean App Platform**

**Por qué:**
```
? Costo: ~$27 USD/mes (~460,000 Gs)
? Todo automático (GitHub + click)
? PostgreSQL incluido
? Perfecto para startups
? Muy simple
```

### Para Testing: **Windows 11/10 Pro Local**

**Por qué:**
```
? Costo: $0
? Funciona ahora mismo
? Perfecto para ver cómo se vería
? Mostrar al equipo
```

---

## ?? PLAN RECOMENDADO PARA A Y R

### Fase Actual (Testing)

```
1. Usa tu PC Windows 11/10 Pro
2. Instala PostgreSQL local
3. Ejecuta "dotnet run"
4. Prueba con el equipo del taller
```

### Fase 2 (Demo/Producción inicial)

```
1. Crear Windows Server en Azure ($50/mes)
2. Instalar IIS + PostgreSQL
3. Publicar aplicación
4. URL accesible desde internet
```

### Fase 3 (Escalamiento)

```
1. Si crece: Agregar más recursos
2. O cambiar a DigitalOcean si quieres ahorrar
3. Implementar backups automáticos
4. Configurar monitoreo
```

---

## ?? PRÓXIMOS PASOS

### Este mes

```
1. ? Código completo (YA HECHO)
2. ? Documentación (YA HECHO)
3. ? Implementar Infrastructure (EF Core)
4. ? Crear API REST
```

### Mes siguiente

```
1. ? Desplegar a Azure/DigitalOcean
2. ? Crear Frontend (Blazor/React)
3. ? Integrar WhatsApp + Email
```

---

## ?? RESUMEN COMPARATIVO

| Aspecto | Windows Server | DigitalOcean | Azure | Local PC |
|---------|---|---|---|---|
| **Costo** | $50/mes | $27/mes | $70/mes | Gratis |
| **Facilidad** | Media | Fácil | Fácil | Muy Fácil |
| **Producción** | ? Sí | ? Sí | ? Sí | ? No |
| **Escalable** | ? Sí | ? Sí | ? Muy | ? No |
| **Linux Requerido** | ? No | ? No | ? No | ? No |
| **Mantenimiento** | Poco | Poco | Nada | Nada |

---

## ?? DECISIÓN FINAL

**Mi recomendación personal para A Y R:**

1. **Ahora**: Usa tu PC con Windows 10/11
2. **En 1 mes**: Sube a DigitalOcean ($27/mes)
3. **Si crece**: Migra a Azure o Windows Server

**¡Ninguna opción requiere Linux!**

---

**Alternativas de Servidor - A Y R Servicio Técnico**
**Enero 2026**
**¡Haz tu elección y contacta si necesitas ayuda!**
