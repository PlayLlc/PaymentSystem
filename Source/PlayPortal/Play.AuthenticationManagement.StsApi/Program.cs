using Microsoft.EntityFrameworkCore;
using Play.AuthenticationManagement.Identity;
using Play.AuthenticationManagement.Identity.Models;
using Play.AuthenticationManagement.IdentityServer;
using Play.AuthenticationManagement.IdentityServer.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

await builder.Services.AddIdentityServices(builder.Configuration.GetConnectionString("Play.IdentityStore"));

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
.AddDeveloperSigningCredential() // we will use this only for dev. for production we need appropriate signing certificates for our tls.
.AddInMemoryIdentityResources(Config.IdentityResources)
.AddInMemoryApiScopes(Config.ApiScopes)
.AddInMemoryClients(Config.Clients)
.AddAspNetIdentity<User>();

builder.Services.AddAutoMapper(typeof(IdentityServerMapper));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthorization();

app.UseMvcWithDefaultRoute();

app.Run();
