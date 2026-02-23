# ?? Guía de Contribución - BusinessManagementSystem

Primero que todo, ¡gracias por tu interés en contribuir a este proyecto!

Este documento proporciona pautas y direcciones para contribuir.

---

## ?? Tabla de Contenidos

- [Código de Conducta](#código-de-conducta)
- [Cómo Contribuir](#cómo-contribuir)
- [Proceso de Pull Request](#proceso-de-pull-request)
- [Estándares de Código](#estándares-de-código)
- [Convenciones de Nombres](#convenciones-de-nombres)
- [Estructura de Commits](#estructura-de-commits)
- [Testing](#testing)
- [Documentación](#documentación)

---

## ?? Código de Conducta

Este proyecto adhiere a un código de conducta. Al participar, se espera que:

- Seas respetuoso con otros contribuidores
- Proporciones feedback constructivo
- Aceptes críticas constructivas
- Enfoques en lo mejor para la comunidad

---

## ?? Cómo Contribuir

### Tipos de Contribución Bienvenidas

1. **Reportar Bugs**
   - Describir el problema claramente
   - Pasos para reproducir
   - Comportamiento esperado vs actual

2. **Sugerir Mejoras**
   - Explicar el caso de uso
   - Beneficio de la mejora
   - Posible implementación

3. **Implementar Features**
   - Seguir la arquitectura DDD
   - Agregar nuevos casos de uso
   - Extender funcionalidad existente

4. **Mejorar Documentación**
   - Corregir errores
   - Agregar ejemplos
   - Mejorar claridad

5. **Reportar Issues de Seguridad**
   - ?? NO abrir issue público
   - Contactar al autor privadamente
   - Detallar vulnerabilidad

---

## ?? Proceso de Pull Request

### Paso 1: Fork el Repositorio

```bash
# En GitHub, haz clic en "Fork"
# O usa GitHub CLI:
gh repo fork RoqueLuis07/business-management-system-dotnet --clone
```

### Paso 2: Crear Rama de Feature

```bash
# Actualiza main
git checkout main
git pull origin main

# Crea rama feature
git checkout -b feature/tu-feature-nombre

# Ejemplos:
# git checkout -b feature/add-email-notifications
# git checkout -b fix/warranty-calculation-bug
# git checkout -b docs/improve-installation-guide
```

### Paso 3: Haz los Cambios

```bash
# Edita los archivos necesarios
# Sigue los estándares (ver sección abajo)
# Compila y verifica
dotnet build
```

### Paso 4: Commit con Mensaje Claro

```bash
# Ver sección "Estructura de Commits" abajo
git commit -m "feat: agregar notificaciones por email"
```

### Paso 5: Push a tu Fork

```bash
git push origin feature/tu-feature-nombre
```

### Paso 6: Abre un Pull Request

- Ve a GitHub
- Haz clic en "New Pull Request"
- Llena la plantilla PR
- Describe qué cambiaste y por qué

### Paso 7: Responde Feedback

- Sé receptivo a comentarios
- Haz cambios si es necesario
- Re-pushea cambios
- El PR se actualiza automáticamente

---

## ? Proceso de Pull Request Checklist

Antes de enviar tu PR, asegúrate de:

- [ ] Has forkeado el repo correctamente
- [ ] Tu rama es basada en la última version de `main`
- [ ] Has compilado sin errores: `dotnet build`
- [ ] El código sigue los estándares (ver abajo)
- [ ] Has agregado/actualizado tests si corresponde
- [ ] Has actualizado documentación
- [ ] Commit messages son claros y descriptivos
- [ ] No hay cambios no relacionados en el PR

---

## ?? Estándares de Código

### Lenguaje y Framework

- **Lenguaje**: C# 11+
- **Framework**: .NET 8
- **IDE Recomendado**: Visual Studio 2022

### Convenciones de Código

```csharp
// ? BIEN - Naming claro
public class WorkOrderRepository : IWorkOrderRepository
{
    public async Task<WorkOrder?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}

// ? MAL - Naming poco claro
public class WORepo : IWORepo
{
    public async Task<WO?> GetById(Guid id)
    {
        // Implementation
    }
}
```

### Indentación y Espaciado

```csharp
// ? 4 espacios (estándar C#)
public void MyMethod()
{
    var result = SomeMethod();
    return result;
}

// ? Línea en blanco entre métodos
public void Method1()
{
    // code
}

public void Method2()
{
    // code
}
```

### Async/Await

```csharp
// ? BIEN
public async Task<Guid> HandleAsync(
    IRepository repo, 
    Command cmd, 
    CancellationToken ct)
{
    var entity = await repo.GetByIdAsync(cmd.Id, ct);
    // ...
}

// ? MAL - Synchronous cuando debería ser async
public Guid Handle(IRepository repo, Command cmd)
{
    var entity = repo.GetById(cmd.Id);
    // ...
}
```

### Validaciones

```csharp
// ? BIEN - Validaciones claras
if (string.IsNullOrWhiteSpace(name))
    throw new ArgumentException("El nombre es obligatorio.", nameof(name));

if (price < 0)
    throw new ArgumentOutOfRangeException(nameof(price), "El precio no puede ser negativo.");

// ? MAL - Sin validaciones
var item = new Item(name, price);
```

### Documentación

```csharp
// ? BIEN - XML Comments
/// <summary>
/// Crea una nueva orden de trabajo.
/// </summary>
/// <param name="repo">Repositorio de órdenes.</param>
/// <param name="cmd">Comando con datos de entrada.</param>
/// <param name="ct">Token de cancelación.</param>
/// <returns>ID de la nueva orden creada.</returns>
/// <exception cref="InvalidOperationException">Si el número ya existe.</exception>
public static async Task<Guid> HandleAsync(
    IWorkOrderRepository repo,
    Command cmd,
    CancellationToken ct)
{
    // Implementation
}

// ? MAL - Sin documentación
public static async Task<Guid> HandleAsync(...)
{
    // Implementation
}
```

### Manejo de Errores

```csharp
// ? BIEN - Mensajes descriptivos en español
if (item is null)
    throw new InvalidOperationException("No se encontró el repuesto en el catálogo.");

// ? MAL - Mensajes genéricos
if (item is null)
    throw new Exception("Error");
```

---

## ?? Convenciones de Nombres

### Entidades Domain

```csharp
// ? Nombres singulares y claros
public class WorkOrder { }
public class Client { }
public class PartCatalogItem { }
public class WorkOrderDiagnosis { }

// ? Plurales o confusos
public class WorkOrders { }
public class OrderWO { }
```

### Casos de Uso

```csharp
// ? Verbo + Sustantivo claro
public static class CreateWorkOrder { }
public static class ApproveWorkOrder { }
public static class GetWorkOrdersByStatus { }

// ? Nombres confusos
public static class WorkOrderCreate { }
public static class Approve { }
public static class GetWOs { }
```

### Variables y Propiedades

```csharp
// ? camelCase para variables locales
var workOrderId = Guid.NewGuid();
var isPresent = true;
var clientName = "Juan";

// ? PascalCase para propiedades públicas
public string FullName { get; set; }
public Guid Id { get; private set; }

// ? snake_case (Python style)
var work_order_id = Guid.NewGuid();
```

---

## ?? Estructura de Commits

Usa el formato [Conventional Commits](https://www.conventionalcommits.org/):

```
<tipo>(<alcance>): <descripción>

<cuerpo opcional>

<pie opcional>
```

### Tipos

- `feat`: Nueva funcionalidad
- `fix`: Corrección de bug
- `docs`: Cambios en documentación
- `style`: Cambios de formato, no lógica
- `refactor`: Refactorización sin cambios de features
- `perf`: Mejoras de performance
- `test`: Agregar o actualizar tests
- `chore`: Cambios en build, deps, etc

### Ejemplos

```bash
# ? Bueno
git commit -m "feat(work-orders): agregar validación de período de garantía"
git commit -m "fix(clients): corregir duplicación de teléfono en búsqueda"
git commit -m "docs: mejorar ejemplos en guía de uso"
git commit -m "test(application): agregar tests para ApproveWorkOrder"

# ? Malo
git commit -m "arreglé cosas"
git commit -m "Updates"
git commit -m "fix bug"
```

---

## ?? Testing

### Ejecutar Tests Existentes

```bash
# Todos los tests
dotnet test

# Tests de un proyecto específico
dotnet test ./tests/BusinessManagementSystem.Application.Tests

# Tests con cobertura
dotnet test /p:CollectCoverage=true
```

### Agregar Nuevos Tests

1. Crear clase de test
2. Usar patrón AAA (Arrange, Act, Assert)
3. Nombres descriptivos

```csharp
// ? BIEN
public class CreateWorkOrderTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ReturnsWorkOrderId()
    {
        // Arrange
        var repository = new Mock<IWorkOrderRepository>();
        var command = new CreateWorkOrder.Command(
            WorkOrderNumber: "OT-001",
            ClientFullName: "Juan",
            // ...
        );

        // Act
        var result = await CreateWorkOrder.HandleAsync(repository.Object, command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        repository.Verify(r => r.AddAsync(It.IsAny<WorkOrder>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithDuplicateNumber_ThrowsException()
    {
        // Arrange
        var repository = new Mock<IWorkOrderRepository>();
        repository.Setup(r => r.GetByNumberAsync("OT-001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new WorkOrder(...));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => CreateWorkOrder.HandleAsync(repository.Object, cmd, ct));
    }
}
```

### Target de Cobertura

- Mínimo: 70%
- Meta: 85%
- Domain Layer: 100%
- Application Layer: 90%+

---

## ?? Documentación

### Documentación Requerida

1. **Cambios en funcionalidad**
   - Actualizar CASOS_DE_USO_IMPLEMENTADOS.md
   - Agregar ejemplo en GUIA_DE_USO_CASOS_DE_USO.md

2. **Nuevas entidades**
   - Actualizar ESTRUCTURA_Y_ORGANIZACION.md
   - Actualizar diagramas en DIAGRAMAS_Y_FLUJOS.md

3. **Cambios en API (futuro)**
   - Actualizar Swagger docs
   - Actualizar README.md

### Formato de Documentación

```markdown
## Nueva Funcionalidad

### Descripción
Explicar qué se agregó y por qué.

### Ejemplo de Uso
\`\`\`csharp
// Código de ejemplo
\`\`\`

### Validaciones
- Validación 1
- Validación 2

### Related Issues
Closes #123
```

---

## ?? Revisión de Código

### Qué Busco en un PR

? **Bueno**:
- Código limpio y legible
- Sigue arquitectura DDD
- Validaciones exhaustivas
- Documentación completa
- Tests relevantes
- Mensajes de commit claros

? **Problemático**:
- Código complicado o poco claro
- Viola la arquitectura
- Falta validaciones
- Sin documentación
- Sin tests
- Commits confusos

### Proceso de Review

1. Revisión automática (compilación, linting)
2. Revisión manual del código
3. Verificación de tests
4. Feedback y mejoras
5. Aprobación y merge

---

## ?? Reportar Bugs

### Plantilla de Issue

```markdown
## Descripción
Descripción clara del bug.

## Pasos para Reproducir
1. Paso 1
2. Paso 2
3. Paso 3

## Comportamiento Esperado
Qué debería pasar.

## Comportamiento Actual
Qué realmente pasa.

## Entorno
- S.O.: [Windows 10 / macOS / Linux]
- .NET: [8.0.x]
- Visual Studio: [2022 / VS Code]

## Logs/Errores
```
Logs o mensajes de error
```

## Screenshots
Si es aplicable.
```

---

## ?? Sugerir Mejoras

### Plantilla de Feature Request

```markdown
## Descripción
Descripción clara de la mejora.

## Caso de Uso
Por qué se necesita.

## Solución Propuesta
Cómo implementarlo.

## Alternativas Consideradas
Otros enfoques.

## Contexto Adicional
Información relevante.
```

---

## ?? Comunicación

### Canales

- **Issues**: Reportar bugs, features, preguntas
- **Discussions**: Conversaciones generales
- **Pull Requests**: Cambios de código

### Esperado

- Respuestas dentro de 48 horas
- Comunicación clara y respetuosa
- Feedback constructivo

---

## ?? Checklist Final

Antes de hacer Push:

```bash
# 1. Actualizar rama desde main
git fetch origin
git rebase origin/main

# 2. Compilar sin errores
dotnet build

# 3. Correr tests
dotnet test

# 4. Revisar código
# (Visual Studio / VS Code)

# 5. Verificar cambios
git status
git diff

# 6. Commit
git commit -m "tipo(alcance): descripción"

# 7. Push
git push origin feature/tu-rama
```

---

## ?? Ejemplos Prácticos

### Ejemplo 1: Agregar Nuevo Caso de Uso

```bash
# 1. Crear rama
git checkout -b feature/add-print-work-order

# 2. Crear archivo
touch src/Application/BusinessManagementSystem.Application/WorkOrders/PrintWorkOrder.cs

# 3. Implementar
# (Ver GUIA_DE_USO_CASOS_DE_USO.md para patrón)

# 4. Actualizar tests
# (Agregar tests)

# 5. Documentar
# (Actualizar CASOS_DE_USO_IMPLEMENTADOS.md)

# 6. Commit
git commit -m "feat(work-orders): agregar funcionalidad de imprimir OT"

# 7. Push y PR
git push origin feature/add-print-work-order
# Abre PR en GitHub
```

### Ejemplo 2: Corregir Bug

```bash
# 1. Crear rama
git checkout -b fix/warranty-validation-bug

# 2. Localizar bug
# (En este ejemplo: cálculo de garantía incorrecto)

# 3. Escribir test que falla
# (Reproduce el bug)

# 4. Corregir
# (En Domain o Application layer)

# 5. Verificar que test pasa
dotnet test

# 6. Commit
git commit -m "fix(work-orders): corregir cálculo de período de garantía"

# 7. Push y PR
git push origin fix/warranty-validation-bug
```

---

## ?? Reconocimiento

¡Agradezco las contribuciones!

Los contribuidores serán reconocidos en:
- README.md (sección Contributors)
- Changelog
- Releases notes

---

## ?? Más Información

- [README.md](./README.md) - Guía principal
- [INDICE_DOCUMENTACION.md](./INDICE_DOCUMENTACION.md) - Documentación
- [CASOS_DE_USO_IMPLEMENTADOS.md](./CASOS_DE_USO_IMPLEMENTADOS.md) - UCs
- [GUIA_DE_USO_CASOS_DE_USO.md](./GUIA_DE_USO_CASOS_DE_USO.md) - Ejemplos

---

**¡Gracias por considerar contribuir! ??**

Esperamos trabajar contigo para mejorar este proyecto.

Happy coding! ??
