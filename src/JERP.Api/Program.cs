using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Microsoft.OpenApi.Models;
using JERP.Api.Middleware;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ---- Serilog (single configuration) ----
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.File("logs/jerp-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14, shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

// ---- Services ----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JERP 3.0 API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer token. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// Rate limiting (global fixed-window)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ip_fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
});

// CORS (reads Cors:AllowedOrigins from appsettings.json)
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

// ---- Database - SQL Server (JerpDbContext) ----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<JerpDbContext>(opts =>
    opts.UseSqlServer(connectionString, sql =>
    {
        sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
        sql.CommandTimeout(60);
        sql.MigrationsAssembly("JERP.Infrastructure");
    }));

// Optional: register other existing extension services (e.g., compliance)
if (Type.GetType("JERP.Application.ComplianceExtensions, JERP.Application") != null)
{
    // If you have an extension method AddComplianceServices, call it here.
    // builder.Services.AddComplianceServices();
}

var app = builder.Build();

// ---- Middleware pipeline (ordered) ----
app.UseSerilogRequestLogging();   // request logging

// Security headers middleware - make sure this file exists
app.UseSecurityHeaders();

app.UseRateLimiter();             // global rate limiter

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "JERP 3.0 API v1");
        options.RoutePrefix = string.Empty; // swagger at root
    });
}

// Map endpoints
app.MapControllers();
app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Json(new
{
    name = "JERP 3.0 API",
    version = "1.0.0",
    developer = "Julio Cesar Mendez Tobar Jr.",
    contact = "ichbincesartobar@yahoo.com",
    timestamp = DateTime.UtcNow
}));

try
{
    Log.Information("Starting JERP API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
