# ?? Guía de Despliegue - BusinessManagementSystem

Esta guía describe las mejores prácticas para desplegar BusinessManagementSystem a producción.

> **Nota**: Esta sección es para futuro uso cuando se implemente la capa de infraestructura.

---

## ?? Tabla de Contenidos

- [Requisitos de Producción](#requisitos-de-producción)
- [Despliegue en IIS](#despliegue-en-iis)
- [Despliegue en Docker](#despliegue-en-docker)
- [Despliegue en Azure](#despliegue-en-azure)
- [Configuración de Seguridad](#configuración-de-seguridad)
- [Monitoreo](#monitoreo)
- [Backup y Recuperación](#backup-y-recuperación)

---

## ? Requisitos de Producción

### Servidor Windows

- **S.O.**: Windows Server 2019+
- **IIS**: 10.0+
- **.NET 8 Runtime**: Hosting Bundle
- **SQL Server**: 2019+ (o PostgreSQL/MySQL)
- **RAM**: Mínimo 4GB
- **Espacio**: 5GB
- **HTTPS**: Certificado SSL válido

### Servidor Linux

- **S.O.**: Ubuntu 20.04 LTS+
- **Nginx/Apache**: Reverse proxy
- **.NET 8 Runtime**
- **PostgreSQL**: 12+ (recomendado)
- **RAM**: Mínimo 4GB
- **Espacio**: 5GB
- **HTTPS**: Certificado SSL válido

---

## ?? Despliegue en IIS (Windows Server)

### Paso 1: Preparar el Servidor

```powershell
# 1. Instalar .NET 8 Hosting Bundle
# Descargar: https://dotnet.microsoft.com/download/dotnet/8.0
# Buscar "Hosting Bundle"

# 2. Instalar IIS
dism.exe /online /enable-feature /featurename:IIS-WebServerRole

# 3. Habilitar módulos requeridos
dism.exe /online /enable-feature /featurename:IIS-ApplicationInit
dism.exe /online /enable-feature /featurename:IIS-ASPNET45
```

### Paso 2: Preparar la Aplicación

```powershell
# Publicar en Release
dotnet publish -c Release -o ./publish

# Resultado: Carpeta ./publish con archivos compilados
```

### Paso 3: Crear Sitio en IIS

```powershell
# 1. Abre IIS Manager
inetmgr

# 2. Right-click Sites > Add Website
# Nombre: BusinessManagementSystem
# Path: C:\inetpub\wwwroot\app
# Port: 80 (o 443 para HTTPS)

# 3. Copia archivos publicados a la carpeta del sitio
Copy-Item -Path ./publish/* -Destination "C:\inetpub\wwwroot\app" -Recurse
```

### Paso 4: Configurar Application Pool

```powershell
# 1. En IIS Manager, selecciona Application Pool
# 2. Configura:
# - .NET CLR version: No Managed Code
# - Managed pipeline mode: Integrated
# - Identity: ApplicationPoolIdentity (o específico)

# 3. Set Permissions (asegura que carpeta sea accesible)
icacls "C:\inetpub\wwwroot\app" /grant "IIS APPPOOL\BusinessManagementSystem:(OI)(CI)F"
```

### Paso 5: Configurar HTTPS

```powershell
# 1. Obtén certificado SSL (Let's Encrypt, etc)
# 2. En IIS:
#    - Binding: https://<dominio>
#    - Selecciona certificado SSL
# 3. Redirige HTTP a HTTPS
```

### Paso 6: Configurar Base de Datos

```powershell
# 1. Actualiza appsettings.json con connection string de producción
# 2. Ejemplo SQL Server:
# "DefaultConnection": "Server=prod-sql.contoso.com;Database=BMS;User Id=sa;Password=***;"

# 3. Aplica migrations:
dotnet ef database update -c YourDbContext --connection "tu-connection-string"
```

---

## ?? Despliegue en Docker

### Dockerfile

```dockerfile
# Multi-stage build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar proyectos
COPY ["BusinessManagementSystem.Domain/", "Domain/"]
COPY ["BusinessManagementSystem.Application/", "Application/"]

# Restaurar y compilar
RUN dotnet restore "Application/BusinessManagementSystem.Application.csproj"
RUN dotnet build "Application/BusinessManagementSystem.Application.csproj" -c Release -o /app/build

# Stage de publicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar binarios compilados
COPY --from=build /app/build .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "BusinessManagementSystem.Application.dll"]
```

### docker-compose.yml

```yaml
version: '3.8'

services:
  app:
    build: .
    ports:
      - "5000:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=postgres;Database=BMS;User Id=sa;Password=***;
    depends_on:
      - postgres
    volumes:
      - ./logs:/app/logs

  postgres:
    image: postgres:15
    environment:
      POSTGRES_USER: sa
      POSTGRES_PASSWORD: ***
      POSTGRES_DB: BMS
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"

volumes:
  postgres_data:
```

### Compilar y Ejecutar

```bash
# Build
docker-compose build

# Ejecutar
docker-compose up -d

# Logs
docker-compose logs -f app

# Detener
docker-compose down
```

---

## ?? Despliegue en Azure

### Opción 1: Azure App Service

```powershell
# 1. Crear grupo de recursos
az group create --name bms-rg --location eastus

# 2. Crear App Service Plan
az appservice plan create `
  --name bms-plan `
  --resource-group bms-rg `
  --sku B2 --is-linux

# 3. Crear Web App
az webapp create `
  --resource-group bms-rg `
  --plan bms-plan `
  --name businessmanagementsystem `
  --runtime "DOTNETCORE|8.0"

# 4. Configurar connection strings
az webapp config appsettings set `
  --resource-group bms-rg `
  --name businessmanagementsystem `
  --settings ConnectionStrings__DefaultConnection="Server=...;Database=BMS;"

# 5. Publicar
dotnet publish -c Release -o ./publish
cd publish
dotnet tool install -g Azure.Tools.Cli
azure webapp up --resource-group bms-rg --name businessmanagementsystem
```

### Opción 2: Azure Container Instances

```bash
# 1. Build Docker image
docker build -t businessmanagementsystem:latest .

# 2. Push a Azure Container Registry
az acr build --registry <registry-name> --image businessmanagementsystem:latest .

# 3. Deploy a Container Instances
az container create \
  --resource-group bms-rg \
  --name bms-app \
  --image <registry-name>.azurecr.io/businessmanagementsystem:latest \
  --cpu 1 --memory 1.5 \
  --ports 80 443 \
  --environment-variables ASPNETCORE_ENVIRONMENT=Production
```

---

## ?? Configuración de Seguridad

### Certificado SSL/TLS

```powershell
# Let's Encrypt (Gratuito)
# Instala Certbot: https://certbot.eff.org/

certbot certonly --standalone -d tudominio.com

# Renovación automática
certbot renew --dry-run
```

### Configuración de Firewall

```bash
# Linux
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw allow 22/tcp
sudo ufw enable

# Windows (PowerShell as Admin)
New-NetFirewallRule -DisplayName "Allow HTTP" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 80
New-NetFirewallRule -DisplayName "Allow HTTPS" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 443
```

### Variables de Entorno Sensibles

```bash
# No hardcodear en appsettings.json

# Linux - .env
export ConnectionString="Server=prod-db;..."
export JwtSecret="your-secret-key"

# Windows - Usar Secrets Manager
dotnet user-secrets init
dotnet user-secrets set "ConnectionString" "Server=prod-db;..."

# Mejor: Usar Key Vault (Azure)
```

### CORS Configuration

```csharp
// En Program.cs (futuro)
services.AddCors(options =>
{
    options.AddPolicy("Production",
        builder =>
        {
            builder
                .WithOrigins("https://yourdomain.com")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});
```

---

## ?? Monitoreo

### Application Insights (Azure)

```csharp
// En Program.cs (futuro)
builder.Services.AddApplicationInsightsTelemetry();
```

### Logging Estructurado

```csharp
// Serilog (recomendado)
builder.Host.UseSerilog((context, services, config) =>
{
    config
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File(
            "logs/log-.txt",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] {Level:u3} {Message:lj}{NewLine}{Exception}");
});
```

### Health Checks

```csharp
// En Program.cs (futuro)
app.MapHealthChecks("/health");

services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddCheck<CustomHealthCheck>("custom");
```

---

## ?? Backup y Recuperación

### Backup Automatizado

```powershell
# SQL Server - Backup diario
$BackupPath = "C:\Backups\BMS_$(Get-Date -Format 'yyyyMMdd').bak"
Backup-SqlDatabase -ServerInstance "prod-sql" -Database "BMS" -BackupFile $BackupPath
```

### Plan de Recuperación

```
1. Database: Backup cada 24 horas
2. Código: Versioning en GitHub
3. Configuración: Versionada en appsettings.json
4. Logs: Almacenados 90 días
5. Certificados: Backup seguro
```

### Disaster Recovery

```powershell
# 1. Restaurar BD
Restore-SqlDatabase -ServerInstance "prod-sql" -Database "BMS" -BackupFile $BackupPath

# 2. Redeploy aplicación
git clone <repo>
dotnet build -c Release
# Publicar a servidor

# 3. Validar
# Ejecutar tests
# Verificar conectividad BD
# Revisar logs
```

---

## ?? Checklist Pre-Producción

- [ ] Aplicación compilada en Release
- [ ] Base de datos creada y migrada
- [ ] Connection strings configurados
- [ ] SSL/TLS certificado
- [ ] Variables sensibles en Key Vault
- [ ] Firewall configurado
- [ ] Backups habilitados
- [ ] Monitoreo configurado
- [ ] Health checks funcionando
- [ ] Load testing realizado
- [ ] Documentación actualizada
- [ ] Plan de recuperación listo
- [ ] Equipo entrenado
- [ ] Rollback plan documentado

---

## ?? Despliegue Continuo (CI/CD)

### GitHub Actions (Ejemplo)

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0
    
    - name: Restore
      run: dotnet restore
    
    - name: Build
      run: dotnet build -c Release
    
    - name: Test
      run: dotnet test
    
    - name: Publish
      run: dotnet publish -c Release -o ./publish
    
    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: businessmanagementsystem
        publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
        package: ./publish
```

---

## ?? Escaling

### Horizontal Scaling (Multi-servidor)

```bash
# Load Balancer (nginx)
upstream bms_backend {
    server app1.example.com;
    server app2.example.com;
    server app3.example.com;
}

server {
    listen 80;
    location / {
        proxy_pass http://bms_backend;
    }
}
```

### Vertical Scaling

- Aumentar RAM del servidor
- Mejorar CPU
- Optimizar índices de BD
- Implementar caching (Redis)

---

## ?? Troubleshooting Producción

### Aplicación lenta

```bash
# Verificar logs
tail -f /var/log/app/error.log

# Verificar recursos
top -p $(pgrep -f dotnet)
free -h
df -h

# Profile
dotnet-trace collect -p <PID>
```

### Errores 500

```bash
# Verificar connection string
cat appsettings.json

# Verificar BD está accesible
sqlcmd -S servidor -U usuario -P pass -Q "SELECT 1"

# Ver logs detallados
cat logs/*.txt
```

---

## ?? Soporte Post-Despliegue

- Monitoreo 24/7
- Alertas configuradas
- On-call rotation
- Documentación actualizada
- Contacto de soporte

---

**Nota**: Esta guía será más específica una vez que se implemente la capa de Infraestructura.

Happy deployment! ??
