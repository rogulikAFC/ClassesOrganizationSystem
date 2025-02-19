using Duende.IdentityServer.Configuration;
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

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddCustomIdentity();

//builder.Services.AddIdentityServer(options =>
//{
//    options.Cors = new Duende.IdentityServer.Configuration.CorsOptions()
//    {
//        CorsPaths = { new PathString("http://localhost:5173/") },
//    };
//});

builder.Services.PostConfigure<IdentityServerOptions>(options =>
{
    options.Cors = new CorsOptions()
    {
        CorsPaths = { PathString.FromUriComponent(new Uri("http://localhost:5173/")) },
    };
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();
