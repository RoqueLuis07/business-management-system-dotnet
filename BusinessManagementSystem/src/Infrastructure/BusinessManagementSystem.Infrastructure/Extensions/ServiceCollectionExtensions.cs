using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Infrastructure.Data;
using BusinessManagementSystem.Infrastructure.Repositories;

namespace BusinessManagementSystem.Infrastructure.Extensions
{
    /// <summary>
    /// Extensiones para registrar servicios de infraestructura en el contenedor de DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra todos los servicios de infraestructura incluyendo DbContext y Repositorios
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string no puede estar vacío", nameof(connectionString));

            // Registrar DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.CommandTimeout(30);
                    npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                });

                // Logging en desarrollo
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.LogTo(Console.WriteLine)
                        .EnableSensitiveDataLogging();
                }
            });

            // Registrar Repositorios
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPartCatalogRepository, PartCatalogRepository>();
            services.AddScoped<IWarrantyClaimRepository, WarrantyClaimRepository>();

            return services;
        }

        /// <summary>
        /// Ejecuta las migraciones pendientes en la base de datos
        /// Útil para inicializar la BD al arrancar la aplicación
        /// </summary>
        public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    Console.WriteLine($"?? Aplicando {pendingMigrations.Count()} migraciones pendientes...");
                    await dbContext.Database.MigrateAsync();
                    Console.WriteLine("? Migraciones aplicadas exitosamente");
                }
                else
                {
                    Console.WriteLine("? Base de datos está actualizada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al aplicar migraciones: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Verifica la conexión a la base de datos
        /// </summary>
        public static async Task<bool> VerifyDatabaseConnectionAsync(this IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.OpenConnectionAsync();
                await dbContext.Database.CloseConnectionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error de conexión a BD: {ex.Message}");
                return false;
            }
        }
    }
}
