using Microsoft.OpenApi.Models;
using BusinessManagementSystem.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "A Y R Servicio T�cnico - API REST",
        Version = "v1.0.0",
        Description = "Sistema de gesti�n de �rdenes de trabajo para talleres de reparaci�n",
        Contact = new OpenApiContact
        {
            Name = "A Y R Servicio T�cnico",
            Email = "info@ayrservicio.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT"
        }
    });

    // Agregar seguridad JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Comentarios XML en documentaci�n
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// Infrastructure setup
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured.");

builder.Services.AddInfrastructure(connectionString);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "A Y R API v1");
        c.RoutePrefix = string.Empty; // Swagger en la ra�z
    });
}

// Aplicar migraciones autom�ticamente
try
{
    await app.Services.ApplyMigrationsAsync();
    Console.WriteLine("? Migraciones aplicadas exitosamente");
}
catch (Exception ex)
{
    Console.WriteLine($"? Error al aplicar migraciones: {ex.Message}");
}

// Verificar conexi�n a BD
try
{
    var connected = await app.Services.VerifyDatabaseConnectionAsync();
    if (connected)
        Console.WriteLine("? Conectado a PostgreSQL correctamente");
}
catch (Exception ex)
{
    Console.WriteLine($"?? Error de conexi�n a BD: {ex.Message}");
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
