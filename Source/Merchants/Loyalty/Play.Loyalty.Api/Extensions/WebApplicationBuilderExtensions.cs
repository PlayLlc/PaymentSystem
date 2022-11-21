namespace Play.Loyalty.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        //string? connectionString = builder.Configuration.GetConnectionString(InventoryDbContext.DatabaseName);

        //builder.Services.AddDbContext<InventoryDbContext>(options =>
        //{
        //    options.UseSqlServer(connectionString!);
        //});

        return builder;
    }

    #endregion
}