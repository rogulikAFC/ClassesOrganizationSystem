using static ClassesOrganizationSystem.Infrastructure.Persistence.PersistenceDI;
using static ClassesOrganizationSystem.Infrastructure.IdentityServer.IdentityDI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddCustomIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();
