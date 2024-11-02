using static ClassesOrganizationSistem.Infrastructure.Persistence.PersistenceDI;
using static ClassesOrganizationSistem.Infrastructure.IdentityServer.IdentityDI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();
