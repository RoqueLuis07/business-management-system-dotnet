using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;
using FluentAssertions;

namespace BusinessManagementSystem.Domain.Tests;

public class DomainTests
{
    [Fact]
    public void Creating_workorder_with_valid_data_should_initialize_properties()
    {
        // Arrange
        var client = new Client("Juan Pérez", "123456789", "Calle 1");
        var equipment = new Equipment("Laptop", "Dell", "XPS", "SN123");
        var requested = "Reparar pantalla";
        var number = "OT-001";

        // Act
        var wo = new WorkOrder(number, client, equipment, requested);

        // Assert
        wo.WorkOrderNumber.Should().Be(number);
        wo.Client.Should().BeSameAs(client);
        wo.Equipment.Should().BeSameAs(equipment);
        wo.RequestedWorkDescription.Should().Be(requested);
        wo.Status.Should().Be(WorkOrderStatus.Ingresada);
        wo.Accessories.Should().BeEmpty();
        wo.Parts.Should().BeEmpty();
    }

    [Fact]
    public void Creating_workorder_with_empty_number_should_throw()
    {
        // Arrange
        var client = new Client("Ana", "987654321", "Av 2");
        var equipment = new Equipment("Telefono", "Samsung", "S10", null);

        // Act
        Action act = () => new WorkOrder("  ", client, equipment, "Diagnóstico");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*talonario*");
    }

    [Fact]
    public void AddAccessory_should_add_entry_and_track_status()
    {
        var client = new Client("Luis", "111222333", "Calle 3");
        var equipment = new Equipment("Tablet", "Apple", "iPad", "");
        var wo = new WorkOrder("OT-002", client, equipment, "Reparar batería");

        wo.AddAccessory("Cargador", true, "Bueno");
        wo.Accessories.Should().HaveCount(1);
        wo.Accessories.First().Name.Should().Be("Cargador");
    }
}
