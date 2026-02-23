# ?? Guía Detallada de Instalación - BusinessManagementSystem

Esta guía te ayudará a instalar y configurar el proyecto paso a paso.

---

## ?? Tabla de Contenidos

- [Requisitos Previos](#requisitos-previos)
- [Instalación en Windows](#instalación-en-windows)
- [Instalación en macOS](#instalación-en-macos)
- [Instalación en Linux](#instalación-en-linux)
- [Verificación de Instalación](#verificación-de-instalación)
- [Troubleshooting](#troubleshooting)
- [Próximos Pasos](#próximos-pasos)

---

## ? Requisitos Previos

### Software Requerido

1. **.NET 8 SDK** (Obligatorio)
2. **Git** (Para clonar el repositorio)
3. **Visual Studio 2022** O **VS Code** (Opcional, pero recomendado)
4. **PowerShell** o **Terminal** (Incluido en todos los OS)

### Requisitos de Sistema

| Requisito | Mínimo | Recomendado |
|-----------|--------|------------|
| **RAM** | 2GB | 8GB |
| **Espacio Disco** | 500MB | 2GB |
| **Procesador** | Dual-core | Quad-core |
| **SO** | Windows 10+ | Windows 11+ |

---

## ?? Instalación en Windows

### Paso 1: Descargar e Instalar .NET 8

1. Ve a https://dotnet.microsoft.com/download/dotnet/8.0
2. Haz clic en **Download** (versión recomendada para Windows)
3. Ejecuta el instalador descargado
4. Sigue el asistente de instalación
5. Reinicia tu computadora (recomendado)

### Verificar Instalación de .NET

Abre **PowerShell** y ejecuta:

```powershell
dotnet --version
```

**Resultado esperado**: `8.0.x` o superior

Si no funciona:
- Asegúrate de haber reiniciado
- Añade .NET al PATH manualmente
- Intenta abrir una nueva ventana PowerShell

### Paso 2: Descargar e Instalar Git

1. Ve a https://git-scm.com/download/win
2. Descarga el instalador (64-bit o 32-bit)
3. Ejecuta el instalador
4. Sigue el asistente (usa valores por defecto)
5. Reinicia tu computadora

### Verificar Instalación de Git

Abre **PowerShell** y ejecuta:

```powershell
git --version
```

**Resultado esperado**: `git version 2.x.x`

### Paso 3: Clonar el Repositorio

Abre **PowerShell** en la carpeta donde deseas guardar el proyecto:

```powershell
# Navega a donde quieres el proyecto
# Ejemplo: Documentos
cd Documents

# Clona el repositorio
git clone https://github.com/RoqueLuis07/business-management-system-dotnet.git

# Entra a la carpeta
cd business-management-system-dotnet
```

**Resultado esperado**:
```
Cloning into 'business-management-system-dotnet'...
remote: Enumerating objects: XXX, done.
...
Resolving deltas: 100% (XXX/XXX), done.
```

### Paso 4: Restaurar Dependencias

En PowerShell (dentro de la carpeta del proyecto):

```powershell
dotnet restore
```

**Resultado esperado**:
```
Determining projects to restore...
  Restored C:\...\BusinessManagementSystem.Domain.csproj
  Restored C:\...\BusinessManagementSystem.Application.csproj
Restore completed in XXXX ms.
```

### Paso 5: Compilar el Proyecto

```powershell
dotnet build
```

**Resultado esperado**:
```
Build started...
...
Build succeeded. 0 Error(s), 0 Warning(s) in XX.XXs.
```

Si hay errores, ve a la sección [Troubleshooting](#troubleshooting).

### Paso 6: (Opcional) Abrir en Visual Studio

```powershell
# Abre en Visual Studio (si está instalado)
start BusinessManagementSystem.sln
```

---

## ?? Instalación en macOS

### Paso 1: Descargar e Instalar .NET 8

#### Opción A: Usando Homebrew (Recomendado)

```bash
# Instala Homebrew si no lo tienes
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Instala .NET 8
brew install dotnet
```

#### Opción B: Descarga Manual

1. Ve a https://dotnet.microsoft.com/download/dotnet/8.0
2. Descarga macOS ARM64 o Intel (según tu Mac)
3. Ejecuta el instalador
4. Sigue el asistente

### Verificar Instalación

```bash
dotnet --version
# Resultado esperado: 8.0.x
```

### Paso 2: Instalar Git

```bash
# Usando Homebrew
brew install git

# O descárgalo desde: https://git-scm.com/download/mac
```

### Verificar Git

```bash
git --version
# Resultado esperado: git version 2.x.x
```

### Paso 3: Clonar el Repositorio

```bash
# Navega a donde quieres el proyecto
cd Documents

# Clona el repositorio
git clone https://github.com/RoqueLuis07/business-management-system-dotnet.git

# Entra a la carpeta
cd business-management-system-dotnet
```

### Paso 4: Restaurar Dependencias

```bash
dotnet restore
```

### Paso 5: Compilar

```bash
dotnet build
```

### Paso 6: (Opcional) Abrir en Visual Studio for Mac

```bash
open -a "Visual Studio" BusinessManagementSystem.sln
```

O en VS Code:

```bash
code .
```

---

## ?? Instalación en Linux

### Ubuntu/Debian

#### Paso 1: Instalar .NET 8

```bash
# Actualiza apt
sudo apt update

# Instala .NET 8
sudo apt install dotnet-sdk-8.0
```

#### Paso 2: Instalar Git

```bash
sudo apt install git
```

#### Paso 3: Clonar y Compilar

```bash
# Clona el repositorio
git clone https://github.com/RoqueLuis07/business-management-system-dotnet.git
cd business-management-system-dotnet

# Restaura dependencias
dotnet restore

# Compila
dotnet build
```

### CentOS/RHEL

```bash
# Instala .NET 8
sudo dnf install dotnet-sdk-8.0

# Instala Git
sudo dnf install git

# Sigue los pasos 3+ de Ubuntu
```

### Fedora

```bash
# Instala .NET 8
sudo dnf install dotnet-sdk-8.0

# Instala Git
sudo dnf install git

# Sigue los pasos 3+ de Ubuntu
```

---

## ?? Verificación de Instalación

Ejecuta esto para verificar que todo funciona:

```powershell
# PowerShell en Windows
$env:Path -split ';' | Where-Object { $_ -like '*dotnet*' }
```

```bash
# Bash en macOS/Linux
which dotnet
which git
```

### Verificación Completa

```bash
# Verifica .NET
dotnet --version

# Verifica Git
git --version

# Verifica proyecto compilado
dotnet build
```

**Resultado esperado**: Todo compila sin errores

---

## ?? Troubleshooting

### Error: "dotnet: command not found"

**Solución Windows**:
```powershell
# Reinicia PowerShell completamente
# O agrega .NET al PATH manualmente:
# Control Panel > System > Environment Variables
```

**Solución macOS/Linux**:
```bash
# Verifica que .NET está instalado
which dotnet

# Si no aparece, reinstala:
# Descarga desde https://dotnet.microsoft.com/download/dotnet/8.0
```

### Error: "git: command not found"

**Solución**:
1. Reinicia terminal/PowerShell
2. Reinstala Git
3. Verifica: `git --version`

### Error en Build: "TargetFramework error"

```
error NETSDK1005: Assets file 'C:\..\obj\project.assets.json' doesn't have a target for '.NETCoreApp,Version=v8.0'
```

**Solución**:
```bash
dotnet clean
dotnet restore
dotnet build
```

### Error: "Unable to restore NuGet packages"

**Solución**:
```bash
# Limpia cache de NuGet
dotnet nuget locals all --clear

# Intenta restaurar de nuevo
dotnet restore
```

### Error: "Requires .NET version X.X"

**Solución**:
1. Verifica tu versión: `dotnet --version`
2. Necesitas .NET 8.0 o superior
3. Descárgalo: https://dotnet.microsoft.com/download/dotnet/8.0

### Visual Studio no reconoce el proyecto

**Solución**:
```powershell
# Abre solución desde terminal
dotnet sln list

# O reinicia Visual Studio completamente
```

---

## ?? Estructura Después de Instalación

Después de clonar y compilar, deberías tener:

```
business-management-system-dotnet/
??? .git/                    (Repositorio Git)
??? .gitignore              (Archivos a ignorar)
??? .vs/                    (Cache de Visual Studio)
??? bin/                    (Binarios compilados)
??? obj/                    (Objetos compilados)
??? BusinessManagementSystem/
?   ??? Domain/
?       ??? Entities/
?       ??? Enums/
?       ??? bin/, obj/
??? src/Application/
?   ??? BusinessManagementSystem.Application/
?       ??? Abstractions/
?       ??? WorkOrders/
?       ??? Clients/
?       ??? Users/
?       ??? PartCatalog/
?       ??? WarrantyClaims/
?       ??? bin/, obj/
??? README.md
??? CONTRIBUTING.md
??? LICENSE
??? BusinessManagementSystem.sln
??? global.json
??? ... otros archivos de doc
```

---

## ?? Verificar Estructura de Proyectos

```bash
# Lista proyectos en la solución
dotnet sln list

# Debería mostrar:
# src/Application/BusinessManagementSystem.Application/BusinessManagementSystem.Application.csproj
# BusinessManagementSystem/BusinessManagementSystem.Domain/BusinessManagementSystem.Domain.csproj
```

---

## ?? Próximos Pasos

### 1. Explorar el Código

```bash
# Abre en Visual Studio/VS Code
code .

# O específicamente:
start BusinessManagementSystem.sln  # Windows
open -a "Visual Studio" BusinessManagementSystem.sln  # macOS
```

### 2. Leer Documentación

```bash
# Lee el README principal
cat README.md

# Lee el índice de documentación
cat INDICE_DOCUMENTACION.md

# Lee guía de casos de uso
cat GUIA_DE_USO_CASOS_DE_USO.md
```

### 3. Compilar Nuevamente

```bash
# Asegúrate de que compila
dotnet build

# Con verbose para más detalles
dotnet build --verbosity detailed
```

### 4. Prepararse para Próximas Fases

Cuando estés listo para implementar infraestructura:
- Lee [README_CHECKLIST.md](./README_CHECKLIST.md) - FASE 5
- Prepara BD (SQL Server, PostgreSQL, MySQL)
- Crea proyecto Infrastructure

---

## ?? Tips y Trucos

### Limpiar Compilación

```bash
# Limpia completamente
dotnet clean

# Y restaura/compila de nuevo
dotnet restore
dotnet build
```

### Ver Detalles de Build

```bash
# Más verboso
dotnet build --verbosity detailed

# O
dotnet build -v d
```

### Restablecer a Estado Original

```bash
# Si algo va mal, puedes resetear Git
git reset --hard
git clean -fdx

# Y volver a compilar
dotnet clean
dotnet restore
dotnet build
```

### Acelerar Compilaciones Futuras

```bash
# Build incremental (solo cambios)
dotnet build

# Mucho más rápido si solo editas código
```

---

## ?? Verificación Final

Una vez completada la instalación, deberías ser capaz de:

- [ ] Ejecutar `dotnet --version` sin errores
- [ ] Ver .NET 8.x como resultado
- [ ] Clonar el repositorio exitosamente
- [ ] Ver la carpeta `bin/` y `obj/` después de compilar
- [ ] Abrir la solución en Visual Studio o VS Code
- [ ] Explorar los archivos de código
- [ ] Leer la documentación sin problemas

---

## ? Preguntas Frecuentes

**P: ¿Necesito Visual Studio?**  
R: No es obligatorio. VS Code + C# extension funciona bien.

**P: ¿Puedo usar .NET 9?**  
R: Sí, pero está optimizado para .NET 8.

**P: ¿Dónde va el proyecto en mi computadora?**  
R: Donde ejecutes `git clone`. Ejemplo: `C:\Users\Roque\Documents\`

**P: ¿Necesito permisos de administrador?**  
R: Solo para instalar .NET y Git. El proyecto en sí no.

**P: ¿Cuánto espacio necesito?**  
R: ~500MB para código + 500MB para NuGet packages = ~1GB

**P: ¿Cómo actualizo .NET?**  
R: Descarga la nueva versión desde dotnet.microsoft.com

---

## ?? Ayuda Adicional

Si tienes problemas:

1. Verifica [README.md](./README.md)
2. Consulta [INDICE_DOCUMENTACION.md](./INDICE_DOCUMENTACION.md)
3. Abre un [Issue](https://github.com/RoqueLuis07/business-management-system-dotnet/issues)
4. Contacta al autor

---

**¡Felicidades! ?? Instalación completada.**

Próximo paso: Leer [README.md](./README.md) y empezar a explorar el código.

Happy coding! ??
