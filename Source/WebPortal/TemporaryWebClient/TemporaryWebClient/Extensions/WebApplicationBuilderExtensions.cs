using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

using Play.Telecom.Twilio.Email;
using Play.Telecom.Twilio.Sms;

using TemporaryWebClient.Services;

namespace Play.Identity.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ConfigurationManager configurationManager = builder.Configuration;

        SendGridConfiguration? sendGridConfiguration = configurationManager.GetSection(nameof(SendGridConfiguration)).Get<SendGridConfiguration>();

        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        // Configuration 

        builder.Services.AddScoped<SendGridConfiguration>(a => sendGridConfiguration);
        builder.Services.AddScoped<ISendEmail, EmailClient>();
        builder.Services.AddScoped<IEmailContactUsMessages, ContactUsEmailer>();
        var a = builder.Services.BuildServiceProvider().GetService<IEmailContactUsMessages>();

        return builder;
    }

    #endregion
}