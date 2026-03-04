using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessManagementSystem.API;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BusinessManagementSystem.Api.Tests
{
    // simple in-memory repository stub for testing
    internal class InMemoryWorkOrderRepository : IWorkOrderRepository
    {
        private readonly List<WorkOrder> _store = new();
        public Task AddAsync(WorkOrder workOrder, CancellationToken ct) {
            _store.Add(workOrder);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(WorkOrder workOrder, CancellationToken ct) { return Task.CompletedTask; }
        public Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult(_store.FirstOrDefault(w => w.Id == id));
        public Task<WorkOrder?> GetByNumberAsync(string number, CancellationToken ct) => Task.FromResult(_store.FirstOrDefault(w => w.WorkOrderNumber == number));
        public Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken ct) => Task.FromResult<IEnumerable<WorkOrder>>(_store);
        public Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken ct) => Task.FromResult<IEnumerable<WorkOrder>>(_store.Where(w => w.Status.ToString() == status.ToString()));
        public Task<IEnumerable<WorkOrder>> GetByClientAsync(Guid clientId, CancellationToken ct) => Task.FromResult<IEnumerable<WorkOrder>>(_store.Where(w => w.ClientId == clientId));
        public Task<IEnumerable<WorkOrder>> GetByMechanicAsync(Guid mechanicUserId, CancellationToken ct) => Task.FromResult<IEnumerable<WorkOrder>>(_store.Where(w => w.AssignedMechanicUserId == mechanicUserId));
        public Task<IEnumerable<WorkOrder>> GetUnderWarrantyAsync(DateTime nowLocal, CancellationToken ct) => Task.FromResult<IEnumerable<WorkOrder>>(_store.Where(w => w.DeliveredAtLocal.HasValue && w.DeliveredAtLocal.Value.AddDays(w.WarrantyDays) >= nowLocal));
        public Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var wo = _store.FirstOrDefault(w => w.Id == id);
            if (wo != null) _store.Remove(wo);
            return Task.CompletedTask;
        }
    }

    // custom factory that injects test services
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // replace repository with in-memory stub
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IWorkOrderRepository));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddSingleton<IWorkOrderRepository, InMemoryWorkOrderRepository>();
            });
        }
    }

    public class WorkOrdersApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public WorkOrdersApiTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllWorkOrders_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/workorders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}