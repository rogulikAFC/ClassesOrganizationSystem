using static ClassesOrganizationSystem.Infrastructure.IdentityServer.IdentityDI;
using static ClassesOrganizationSystem.Infrastructure.Persistence.PersistenceDI;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddCustomIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();
