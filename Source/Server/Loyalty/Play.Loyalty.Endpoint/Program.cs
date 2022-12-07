using Microsoft.Extensions.Hosting;

using NServiceBus;

using Play.Logging.Serilog;
using Play.Messaging.NServiceBus.Hosting;

namespace Play.Loyalty.Endpoint;

public class Program
{
    #region Instance Members

    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args).ConfigureNServiceBus(typeof(Program).Assembly).Build().RunAsync();
    }

    #endregion
}