using MerchantPortal.Application;
using MerchantPortal.Infrastructure.Persistence;
using MerchantPortal.WebApi.Filters;
using MerchantPortal.WebApi.Mapping;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiFilterExceptionAttribute>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("sql");

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(connectionString);

builder.Services.AddAutoMapper(typeof(PersistenceMapperProfile));
builder.Services.AddAutoMapper(typeof(ProfileModelMapper));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
