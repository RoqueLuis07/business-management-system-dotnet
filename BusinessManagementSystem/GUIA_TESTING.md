# ?? GUÍA COMPLETA DE TESTING - A Y R Servicio Técnico

**Estrategia exhaustiva de pruebas para garantizar calidad del sistema**

---

## ?? Tabla de Contenidos

- [Estrategia de Testing](#estrategia-de-testing)
- [Tipos de Tests](#tipos-de-tests)
- [Setup de Testing](#setup-de-testing)
- [Tests Unitarios (Domain)](#tests-unitarios-domain)
- [Tests de Aplicación](#tests-de-aplicación)
- [Tests de Integración](#tests-de-integración)
- [Tests End-to-End](#tests-end-to-end)
- [Ejecución de Tests](#ejecución-de-tests)
- [Cobertura de Código](#cobertura-de-código)

---

## ?? Estrategia de Testing

### Pirámide de Testing

```
            /\
           /  \
          / E2E \        Tests End-to-End
         /________\      (10% - Lentos)
        /          \
       / Integration \   Tests de Integración
      /________________\ (20% - Medianos)
     /                  \
    /    Unit Tests     \ Tests Unitarios
   /______________________\ (70% - Rápidos)
```

### Plan Recomendado para A Y R

```
Fase 1: Tests Unitarios (Domain Layer)
??? Entity validations
??? Business logic
??? Enumerations
??? Value objects

Fase 2: Tests de Aplicación
??? Use case handlers
??? Command handling
??? Query handling
??? Error scenarios

Fase 3: Tests de Integración (Futuro - Post Infrastructure)
??? Repository implementations
??? Database operations
??? EF Core mappings
??? Transaction handling

Fase 4: Tests E2E (Futuro - Post API)
??? API endpoints
??? Complete workflows
??? Authentication flows
??? Business rule enforcement
```

---

## ?? Tipos de Tests

### 1. Unit Tests (70% del tiempo)

**Qué testean:**
```
? Métodos individuales
? Validaciones
? Reglas de negocio
? Lógica pura
```

**Características:**
```
- Muy rápidos (ms)
- Sin dependencias externas
- Enfocados en una cosa
- Fáciles de mantener
```

**Ejemplo:**
```csharp
[Fact]
public void Client_CreatesWithValidData()
{
    // Arrange
    var fullName = "Juan García";
    var phone = "0972123456";
    var address = "Calle Principal 123";

    // Act
    var client = new Client(fullName, phone, address);

    // Assert
    Assert.Equal(fullName, client.FullName);
    Assert.Equal(phone, client.Phone);
}
```

---

### 2. Integration Tests (20% del tiempo)

**Qué testean:**
```
? Múltiples componentes juntos
? Persistencia en BD
? Repository implementations
? EF Core mappings
```

**Características:**
```
- Medianos (1-5 segundos)
- Usan base de datos real (o test)
- Prueban colaboración de componentes
- Más complejos de mantener
```

**Ejemplo:**
```csharp
[Fact]
public async Task ClientRepository_CreatesAndRetrievesClient()
{
    // Arrange
    var repo = new ClientRepository(_dbContext);
    var client = new Client("Juan", "0972123456", "Dirección");

    // Act
    await repo.AddAsync(client, CancellationToken.None);
    var retrieved = await repo.GetByIdAsync(client.Id, CancellationToken.None);

    // Assert
    Assert.NotNull(retrieved);
    Assert.Equal(client.FullName, retrieved.FullName);
}
```

---

### 3. End-to-End Tests (10% del tiempo)

**Qué testean:**
```
? Flujos completos de usuario
? API REST endpoints
? Workflows complejos
? Integración de todo el sistema
```

**Características:**
```
- Lentos (5-30 segundos)
- Usan aplicación real
- Prueban desde UI hasta BD
- Más valiosos pero más lentos
```

**Ejemplo:**
```csharp
[Fact]
public async Task CompleteWorkOrderFlow()
{
    // Arrange
    var client = new ClientBuilder().Build();
    var equipment = new EquipmentBuilder().Build();

    // Act
    var workOrder = await _api.CreateWorkOrder(client, equipment);
    await _api.SetDiagnosis(workOrder.Id, "findings");
    await _api.CreateQuote(workOrder.Id, 1000);
    await _api.ApproveQuote(workOrder.Id);
    await _api.StartRepair(workOrder.Id);
    
    // Assert
    var result = await _api.GetWorkOrder(workOrder.Id);
    Assert.Equal("EnReparacion", result.Status);
}
```

---

## ??? Setup de Testing

### Paso 1: Crear Proyecto de Tests

```powershell
cd "C:\Users\roque\source\repos\business-management-system-dotnet"

# Crear carpeta tests
mkdir "tests"
mkdir "tests\BusinessManagementSystem.Domain.Tests"
mkdir "tests\BusinessManagementSystem.Application.Tests"

# Crear proyecto xUnit
cd "tests\BusinessManagementSystem.Domain.Tests"
dotnet new xunit -n BusinessManagementSystem.Domain.Tests
cd ".."
```

### Paso 2: Instalar Paquetes de Testing

```powershell
cd "tests\BusinessManagementSystem.Domain.Tests"

# xUnit (framework de testing)
dotnet add package xunit --version 2.6.0

# xUnit runners
dotnet add package xunit.runner.visualstudio --version 2.5.0

# Assertions mejoradas
dotnet add package FluentAssertions --version 6.11.0

# Mocking
dotnet add package Moq --version 4.20.0

# Builders para tests (opcional pero útil)
dotnet add package Builder --version 1.0.0

# Referencias al proyecto a testear
dotnet add reference "..\..\BusinessManagementSystem\BusinessManagementSystem.Domain\BusinessManagementSystem.Domain.csproj"
```

### Paso 3: Configurar .csproj

```xml
<!-- tests/BusinessManagementSystem.Domain.Tests/BusinessManagementSystem.Domain.Tests.csproj -->

<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <IsTestProject>true</IsTestProject>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="xunit" Version="2.6.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.0">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="FluentAssertions" Version="6.11.0" />
    <PackageReference Include="Moq" Version="4.20.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\BusinessManagementSystem\BusinessManagementSystem.Domain\BusinessManagementSystem.Domain.csproj" />
  </ItemGroup>

</Project>
```

---

## ? Tests Unitarios (Domain)

### Ejemplo 1: Tests de Client Entity

```csharp
// tests/BusinessManagementSystem.Domain.Tests/Entities/ClientTests.cs

using BusinessManagementSystem.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BusinessManagementSystem.Domain.Tests.Entities
{
    public class ClientTests
    {
        [Fact]
        public void Client_CreatesSuccessfully_WithValidData()
        {
            // Arrange
            var fullName = "Juan García López";
            var phone = "0972123456";
            var address = "Calle Principal 123, Asunción";

            // Act
            var client = new Client(fullName, phone, address);

            // Assert
            client.Id.Should().NotBeEmpty();
            client.FullName.Should().Be(fullName);
            client.Phone.Should().Be(phone);
            client.Address.Should().Be(address);
        }

        [Fact]
        public void Client_ThrowsException_WithEmptyFullName()
        {
            // Arrange
            var fullName = "";
            var phone = "0972123456";
            var address = "Dirección";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => new Client(fullName, phone, address)
            );
            exception.Message.Should().Contain("nombre del cliente");
        }

        [Fact]
        public void Client_ThrowsException_WithNullFullName()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => new Client(null!, "0972123456", "Dirección")
            );
            exception.ParamName.Should().Be("fullName");
        }

        [Fact]
        public void Client_UpdatesInfo_Successfully()
        {
            // Arrange
            var client = new Client("Juan", "0972123456", "Calle 1");
            var newName = "Juan García Actualizado";
            var newPhone = "0973654321";
            var newAddress = "Calle 2";

            // Act
            client.UpdateInfo(newName, newPhone, newAddress);

            // Assert
            client.FullName.Should().Be(newName);
            client.Phone.Should().Be(newPhone);
            client.Address.Should().Be(newAddress);
        }

        [Fact]
        public void Client_UpdatesPhone_Successfully()
        {
            // Arrange
            var client = new Client("Juan", "0972123456", "Dirección");
            var newPhone = "0973654321";

            // Act
            client.UpdatePhone(newPhone);

            // Assert
            client.Phone.Should().Be(newPhone);
        }

        [Fact]
        public void Client_TrimsWhitespace_OnCreation()
        {
            // Arrange
            var fullName = "  Juan García  ";
            var phone = "  0972123456  ";
            var address = "  Calle Principal  ";

            // Act
            var client = new Client(fullName, phone, address);

            // Assert
            client.FullName.Should().Be("Juan García");
            client.Phone.Should().Be("0972123456");
            client.Address.Should().Be("Calle Principal");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Client_ThrowsException_WithInvalidFullName(string? invalidName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Client(invalidName ?? "", "0972123456", "Dirección")
            );
        }
    }
}
```

### Ejemplo 2: Tests de WorkOrder Entity

```csharp
// tests/BusinessManagementSystem.Domain.Tests/Entities/WorkOrderTests.cs

using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace BusinessManagementSystem.Domain.Tests.Entities
{
    public class WorkOrderTests
    {
        private readonly Client _testClient;
        private readonly Equipment _testEquipment;

        public WorkOrderTests()
        {
            _testClient = new Client("Test Client", "0972123456", "Test Address");
            _testEquipment = new Equipment("Motosierra", "Stihl", "MS200", "SN123");
        }

        [Fact]
        public void WorkOrder_CreatesSuccessfully()
        {
            // Arrange
            var workOrderNumber = "OT-2024-001";
            var description = "Cliente reporta que no enciende";

            // Act
            var workOrder = new WorkOrder(
                workOrderNumber,
                _testClient,
                _testEquipment,
                description
            );

            // Assert
            workOrder.Id.Should().NotBeEmpty();
            workOrder.WorkOrderNumber.Should().Be(workOrderNumber);
            workOrder.Client.Should().Be(_testClient);
            workOrder.Equipment.Should().Be(_testEquipment);
            workOrder.RequestedWorkDescription.Should().Be(description);
            workOrder.Status.Should().Be(WorkOrderStatus.Ingresada);
        }

        [Fact]
        public void WorkOrder_AssignsMechanic_Successfully()
        {
            // Arrange
            var workOrder = new WorkOrder("OT-001", _testClient, _testEquipment, "Falla");
            var mechanicId = Guid.NewGuid();

            // Act
            workOrder.AssignMechanic(mechanicId);

            // Assert
            workOrder.AssignedMechanicUserId.Should().Be(mechanicId);
            workOrder.Status.Should().Be(WorkOrderStatus.Asignada);
        }

        [Fact]
        public void WorkOrder_StartsDiagnosis_Successfully()
        {
            // Arrange
            var workOrder = new WorkOrder("OT-001", _testClient, _testEquipment, "Falla");
            workOrder.AssignMechanic(Guid.NewGuid());

            // Act
            workOrder.StartDiagnosis();

            // Assert
            workOrder.Status.Should().Be(WorkOrderStatus.EnDiagnostico);
        }

        [Fact]
        public void WorkOrder_SetsDiagnosis_Successfully()
        {
            // Arrange
            var workOrder = new WorkOrder("OT-001", _testClient, _testEquipment, "Falla");
            workOrder.AssignMechanic(Guid.NewGuid());
            workOrder.StartDiagnosis();
            var mechanicId = Guid.NewGuid();

            // Act
            workOrder.SetDiagnosis(
                "Bujía rota",
                "Cambiar bujía",
                "Equipo muy sucio",
                mechanicId
            );

            // Assert
            workOrder.Diagnosis.Should().NotBeNull();
            workOrder.Diagnosis!.Findings.Should().Be("Bujía rota");
        }

        [Fact]
        public void WorkOrder_CannotModify_AfterDelivery()
        {
            // Arrange
            var workOrder = new WorkOrder("OT-001", _testClient, _testEquipment, "Falla");
            workOrder.Status = WorkOrderStatus.Entregada; // Simular entrega (private set)

            // Act & Assert
            // Intentar modificar debería fallar
            var exception = Assert.Throws<InvalidOperationException>(
                () => workOrder.AddAccessory("Accesor", true, null)
            );
            exception.Message.Should().Contain("entregada");
        }

        [Fact]
        public void WorkOrder_CalculatesWarranty_Correctly()
        {
            // Arrange
            var workOrder = new WorkOrder("OT-001", _testClient, _testEquipment, "Falla");
            var deliveryDate = DateTime.Now.AddDays(-10);
            workOrder.SetWarrantyDays(30);

            // Act
            var isUnderWarranty = workOrder.IsUnderWarranty(DateTime.Now.AddDays(15));
            var isOutOfWarranty = workOrder.IsUnderWarranty(DateTime.Now.AddDays(35));

            // Assert
            isUnderWarranty.Should().BeTrue();
            isOutOfWarranty.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(366)]
        public void WorkOrder_RejectsInvalidWarrantyDays(int days)
        {
            // Arrange
            var workOrder = new WorkOrder("OT-001", _testClient, _testEquipment, "Falla");

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => workOrder.SetWarrantyDays(days)
            );
        }

        [Fact]
        public void WorkOrder_AddsPart_Successfully()
        {
            // Arrange
            var workOrder = new WorkOrder("OT-001", _testClient, _testEquipment, "Falla");
            var partName = "Bujía SP-12";
            var quantity = 2;

            // Act
            workOrder.AddPart(partName, quantity);

            // Assert
            workOrder.Parts.Should().HaveCount(1);
            workOrder.Parts.First().Name.Should().Be(partName);
            workOrder.Parts.First().Quantity.Should().Be(quantity);
        }
    }
}
```

---

## ?? Tests de Aplicación

### Ejemplo: Tests de CreateClient Use Case

```csharp
// tests/BusinessManagementSystem.Application.Tests/Clients/CreateClientTests.cs

using BusinessManagementSystem.Application.Clients;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace BusinessManagementSystem.Application.Tests.Clients
{
    public class CreateClientTests
    {
        private readonly Mock<IClientRepository> _repositoryMock;

        public CreateClientTests()
        {
            _repositoryMock = new Mock<IClientRepository>();
        }

        [Fact]
        public async Task CreateClient_HandlesSuccessfully_WithValidCommand()
        {
            // Arrange
            var command = new CreateClient.Command(
                fullName: "Juan García",
                phone: "0972123456",
                email: "juan@email.com",
                address: "Calle Principal 123"
            );

            // Act
            await CreateClient.HandleAsync(_repositoryMock.Object, command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateClient_CreatesClientWithCorrectData()
        {
            // Arrange
            var command = new CreateClient.Command(
                fullName: "María López",
                phone: "0973654321",
                email: "maria@email.com",
                address: "Avenida Principal"
            );

            Client? createdClient = null;

            _repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .Callback<Client, CancellationToken>((client, _) => createdClient = client)
                .Returns(Task.CompletedTask);

            // Act
            await CreateClient.HandleAsync(_repositoryMock.Object, command, CancellationToken.None);

            // Assert
            createdClient.Should().NotBeNull();
            createdClient!.FullName.Should().Be("María López");
            createdClient.Phone.Should().Be("0973654321");
            createdClient.Email.Should().Be("maria@email.com");
            createdClient.Address.Should().Be("Avenida Principal");
        }

        [Fact]
        public async Task CreateClient_ThrowsException_WithEmptyFullName()
        {
            // Arrange
            var command = new CreateClient.Command(
                fullName: "",
                phone: "0972123456",
                email: "juan@email.com",
                address: "Calle"
            );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => CreateClient.HandleAsync(_repositoryMock.Object, command, CancellationToken.None)
            );
        }
    }
}
```

---

## ?? Tests de Integración (Futuro)

### Ejemplo: Repository Integration Test

```csharp
// tests/BusinessManagementSystem.Infrastructure.Tests/Repositories/ClientRepositoryTests.cs

using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BusinessManagementSystem.Infrastructure.Tests.Repositories
{
    public class ClientRepositoryTests : IAsyncLifetime
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ClientRepository _repository;

        public ClientRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _repository = new ClientRepository(_dbContext);
        }

        public async Task InitializeAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.DisposeAsync();
        }

        [Fact]
        public async Task AddAsync_PersistsClient_InDatabase()
        {
            // Arrange
            var client = new Client("Test Client", "0972123456", "Test Address");

            // Act
            await _repository.AddAsync(client, CancellationToken.None);

            // Assert
            var retrieved = await _repository.GetByIdAsync(client.Id, CancellationToken.None);
            retrieved.Should().NotBeNull();
            retrieved!.FullName.Should().Be("Test Client");
        }

        [Fact]
        public async Task GetByPhoneAsync_ReturnsClient_WhenExists()
        {
            // Arrange
            var client = new Client("Test", "0972123456", "Address");
            await _repository.AddAsync(client, CancellationToken.None);

            // Act
            var retrieved = await _repository.GetByPhoneAsync("0972123456", CancellationToken.None);

            // Assert
            retrieved.Should().NotBeNull();
            retrieved!.Id.Should().Be(client.Id);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllClients()
        {
            // Arrange
            var client1 = new Client("Client 1", "0972111111", "Address 1");
            var client2 = new Client("Client 2", "0972222222", "Address 2");

            await _repository.AddAsync(client1, CancellationToken.None);
            await _repository.AddAsync(client2, CancellationToken.None);

            // Act
            var clients = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            clients.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesClientData()
        {
            // Arrange
            var client = new Client("Original", "0972123456", "Address");
            await _repository.AddAsync(client, CancellationToken.None);

            // Act
            client.UpdateInfo("Updated", "0973654321", "New Address");
            await _repository.UpdateAsync(client, CancellationToken.None);

            // Assert
            var updated = await _repository.GetByIdAsync(client.Id, CancellationToken.None);
            updated!.FullName.Should().Be("Updated");
        }
    }
}
```

---

## ?? Ejecución de Tests

### Ejecutar Todos los Tests

```powershell
# Desde la raíz del proyecto
dotnet test

# Resultado esperado:
# Test Run Successful.
# Total tests: 25
# Passed: 25
# Failed: 0
```

### Ejecutar Tests Específicos

```powershell
# Solo Domain tests
dotnet test tests/BusinessManagementSystem.Domain.Tests

# Solo Application tests
dotnet test tests/BusinessManagementSystem.Application.Tests

# Un test específico
dotnet test -k "Client_CreatesSuccessfully"
```

### Ejecutar con Verbosidad

```powershell
# Mostrar todos los detalles
dotnet test -v d

# Mostrar solo los tests que fallan
dotnet test --logger "console;verbosity=minimal"
```

### Ejecutar y Generar Reporte

```powershell
# Con salida en formato TRX (para integración)
dotnet test --logger "trx;LogFileName=test-results.trx"

# Ver resultado en Visual Studio Test Explorer
```

---

## ?? Cobertura de Código

### Instalar Coverlet

```powershell
cd tests/BusinessManagementSystem.Domain.Tests

# Instalar coverlet
dotnet add package coverlet.collector --version 6.0.0
```

### Generar Reporte de Cobertura

```powershell
# Generar reporte de cobertura
dotnet test /p:CollectCoverage=true /p:CoverageFormat=cobertura

# Resultado:
# Generating coverage report...
# | Module | Line | Branch | Method |
# | Domain | 95%  | 92%    | 98%    |
# | Appln  | 88%  | 85%    | 90%    |
```

### Objetivo de Cobertura

```
Domain Layer:     ? 95%+ (crítico - lógica de negocio)
Application Layer: ? 90%+ (importante - casos de uso)
Infrastructure:    ? 80%+ (después de implementar)
API:              ? 85%+ (después de implementar)

Métrica global: 90%+
```

---

## ?? Test Builders (Opcional pero Útil)

### Crear Builders para Tests

```csharp
// tests/BusinessManagementSystem.Domain.Tests/Builders/ClientBuilder.cs

using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Domain.Tests.Builders
{
    public class ClientBuilder
    {
        private string _fullName = "Test Client";
        private string _phone = "0972123456";
        private string _address = "Test Address";

        public ClientBuilder WithFullName(string fullName)
        {
            _fullName = fullName;
            return this;
        }

        public ClientBuilder WithPhone(string phone)
        {
            _phone = phone;
            return this;
        }

        public ClientBuilder WithAddress(string address)
        {
            _address = address;
            return this;
        }

        public Client Build()
        {
            return new Client(_fullName, _phone, _address);
        }
    }
}
```

### Usar en Tests

```csharp
[Fact]
public void Client_WithBuilder()
{
    // Arrange & Act
    var client = new ClientBuilder()
        .WithFullName("Juan García")
        .WithPhone("0973654321")
        .WithAddress("Calle Nueva")
        .Build();

    // Assert
    Assert.NotNull(client);
}
```

---

## ? Checklist de Testing

### Domain Layer Tests
- [ ] Client entity tests (7+ tests)
- [ ] Equipment entity tests
- [ ] User entity tests
- [ ] WorkOrder entity tests (15+ tests)
- [ ] WorkOrderAccessory tests
- [ ] WorkOrderPart tests
- [ ] Enum tests
- [ ] Value object tests

### Application Layer Tests
- [ ] CreateClient handler tests
- [ ] UpdateClient handler tests
- [ ] DeleteClient handler tests
- [ ] GetClient handler tests
- [ ] CreateUser handler tests
- [ ] StartRepair handler tests
- [ ] SetDiagnosis handler tests
- [ ] CreateQuote handler tests
- [ ] More use case tests...

### Integration Tests (Post-Infrastructure)
- [ ] ClientRepository tests
- [ ] WorkOrderRepository tests
- [ ] UserRepository tests
- [ ] Database transaction tests
- [ ] Migration tests

### E2E Tests (Post-API)
- [ ] Complete workflow tests
- [ ] API integration tests
- [ ] Authentication flow tests
- [ ] Warranty claim flow tests

---

## ?? Comando Rápido

```powershell
# Setup completo de tests
cd tests/BusinessManagementSystem.Domain.Tests
dotnet add package xunit FluentAssertions Moq xunit.runner.visualstudio
dotnet add reference "..\..\BusinessManagementSystem\BusinessManagementSystem.Domain\BusinessManagementSystem.Domain.csproj"

# Ejecutar tests
dotnet test

# Con cobertura
dotnet test /p:CollectCoverage=true
```

---

## ?? Ejemplo Completo: Test Suite Mínimo

Crear archivo: `tests/BusinessManagementSystem.Domain.Tests/Entities/ClientTests.cs`

```csharp
using BusinessManagementSystem.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BusinessManagementSystem.Domain.Tests.Entities
{
    public class ClientTests
    {
        [Fact]
        public void Client_CreatesSuccessfully() =>
            new Client("Juan", "0972123456", "Calle").FullName.Should().Be("Juan");

        [Fact]
        public void Client_ThrowsOnEmptyName() =>
            Assert.Throws<ArgumentException>(() => new Client("", "phone", "addr"));

        [Fact]
        public void Client_UpdatesPhone()
        {
            var client = new Client("Juan", "0972123456", "Calle");
            client.UpdatePhone("0973654321");
            client.Phone.Should().Be("0973654321");
        }
    }
}
```

Ejecutar:
```powershell
dotnet test

# Output:
# 3 passed
```

---

## ?? Recomendaciones

```
1. Empezar AHORA con Domain layer tests
   ? No necesitas infraestructura
   ? Puedes hacer 50+ tests hoy

2. Mantener tests simples y rápidos
   ? Tests de 1-5 líneas son ideales
   ? Tests complejos = mantenimiento difícil

3. Usar nombre descriptivos
   ? MethodUnderTest_Scenario_ExpectedResult
   ? ClientTests, WorkOrderTests, etc.

4. Una afirmación por test (cuando sea posible)
   ? Facilita identificar qué falla

5. Refactorizar tests como código
   ? Builders, fixtures, helpers
   ? Reutilizar en múltiples tests

6. CI/CD: Ejecutar tests antes de cada commit
   ? git hooks para tests automáticos
   ? Mantiene calidad
```

---

**¡Listo para testing! ??** 

Próximo paso: Crear carpeta `tests/` e implementar primeros tests.
