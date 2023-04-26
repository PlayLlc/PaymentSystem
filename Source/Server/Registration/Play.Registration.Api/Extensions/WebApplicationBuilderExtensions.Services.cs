using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Registration.Application.Handlers.UserRegistration;
using Play.Registration.Application.Services;
using Play.Registration.Application.Services.Emails;
using Play.Registration.Application.Services.Sms;
using Play.Registration.Domain.Aggregates.MerchantRegistration;
using Play.Registration.Domain.Repositories;
using Play.Registration.Domain.Services;
using Play.Registration.Persistence.Sql.Persistence;
using Play.Registration.Persistence.Sql.Repositories;
using Play.Telecom.Twilio.Email;
using Play.Telecom.Twilio.Sms;

namespace Play.Registration.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ConfigurationManager configurationManager = builder.Configuration;

        TwilioSmsConfiguration? twilioSmsConfiguration = configurationManager.GetSection(nameof(TwilioSmsConfiguration)).Get<TwilioSmsConfiguration>();
        SendGridConfiguration? sendGridConfiguration = configurationManager.GetSection(nameof(SendGridConfiguration)).Get<SendGridConfiguration>();

        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddScoped<IUrlHelper>(x =>
        {
            var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            var factory = x.GetRequiredService<IUrlHelperFactory>();

            return factory.GetUrlHelper(actionContext);
        });

        //builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        //builder.Services.AddScoped<IUrlHelper>(x =>
        //{
        //    ActionContext? actionContext = x.GetService<IActionContextAccessor>().ActionContext;

        //    return new UrlHelper(actionContext);
        //});

        // Configuration
        builder.Services.AddScoped(a => twilioSmsConfiguration);
        builder.Services.AddScoped(a => sendGridConfiguration);
        builder.Services.AddScoped<ISendSmsMessages, SmsClient>();
        builder.Services.AddScoped<ISendEmail, EmailClient>();
        builder.Services.Configure<JsonSerializerOptions>(_ => new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        // Repositories
        builder.Services.AddScoped<DbContext, RegistrationDbContext>();
        builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
        builder.Services.AddScoped<IUserRegistrationRepository, UserRegistrationRepository>();
        builder.Services.AddScoped<IRepository<MerchantRegistration, SimpleStringId>, MerchantRegistrationRepository>();

        // Infrastructure Services 
        builder.Services.AddScoped<ISendSmsMessages, SmsClient>();
        builder.Services.AddScoped<ISendEmail, EmailClient>();

        // Domain Services
        builder.Services.AddScoped<IEnsureUniqueEmails, UniqueEmailChecker>();
        builder.Services.AddHttpClient<IUnderwriteMerchants, MerchantUnderwriter>(options =>
        {
            // HACK: This should be resolved when underwriting is complete
            //options.BaseAddress = new(builder.Configuration["UnderwritingServiceBaseAddress"]);
        });
        builder.Services.AddScoped<IVerifyEmailAccounts, EmailAccountVerifier>();
        builder.Services.AddScoped<IVerifyMobilePhones, MobilePhoneVerifier>();
        builder.Services.AddScoped<ICreateEmailVerificationReturnUrl, EmailVerificationReturnUrlGenerator>();

        // HACK: Should we make these scoped per request? That would mean that we would have to add them to EVERY controller.
        // HACK: Will this introduce race conditions? There are only Write methods so we're not tracking entity changes
        // HACK: so there shouldn't be any entities that are out of sync. Need to test and validate that singleton is the
        // HACK: right move here
        // Application Handlers
        //builder.Services.AddSingleton<MerchantHandler>();
        //builder.Services.AddSingleton<MerchantRegistrationHandler>();
        //builder.Services.AddSingleton<UserHandler>();
        builder.Services.AddScoped<UserRegistrationHandler>();

        // Identity Server
        builder.Services.AddScoped<IProfileService, ProfileService<UserIdentity>>();

        return builder;
    }

    #endregion
}