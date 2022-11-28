using System.Text.Json;

using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Identity.Api.Services;
using Play.Identity.Application.Services;
using Play.Identity.Application.Services.Sms;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Repositories;
using Play.Identity.Domain.Services;
using Play.Identity.Persistence.Sql.Entities;
using Play.Identity.Persistence.Sql.Persistence;
using Play.Identity.Persistence.Sql.Repositories;
using Play.Persistence.Sql;
using Play.Telecom.Twilio.Email;
using Play.Telecom.Twilio.Sms;

namespace Play.Identity.Api.Extensions;

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
            ActionContext? actionContext = x.GetService<IActionContextAccessor>().ActionContext;

            return new UrlHelper(actionContext);
        });

        // Configuration
        builder.Services.AddScoped(a => twilioSmsConfiguration);
        builder.Services.AddScoped(a => sendGridConfiguration);
        builder.Services.AddScoped<ISendSmsMessages, SmsClient>();
        builder.Services.AddScoped<ISendEmail, EmailClient>();
        builder.Services.Configure<JsonSerializerOptions>(_ => new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        // Repositories
        builder.Services.AddScoped<DbContext, UserIdentityDbContext>();
        builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
        builder.Services.AddScoped<IUserRegistrationRepository, UserRegistrationRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRepository<MerchantRegistration, SimpleStringId>, MerchantRegistrationRepository>();
        builder.Services.AddScoped<IRepository<Merchant, SimpleStringId>, Repository<Merchant, SimpleStringId>>();
        builder.Services.AddScoped<ILoginUsers, UserLoginService>();

        // Infrastructure Services 
        builder.Services.AddScoped<ISendSmsMessages, SmsClient>();
        builder.Services.AddScoped<ISendEmail, EmailClient>();

        // Domain Services
        builder.Services.AddScoped<IEnsureUniqueEmails, UniqueEmailChecker>();
        builder.Services.AddScoped<IHashPasswords, PasswordHasher>();
        builder.Services.AddHttpClient<IUnderwriteMerchants, MerchantUnderwriter>(options =>
        {
            options.BaseAddress = new Uri(builder.Configuration["UnderwritingServiceBaseAddress"]);
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
        //builder.Services.AddSingleton<UserRegistrationHandler>();

        // Identity Server
        builder.Services.AddScoped<IProfileService, ProfileService<UserIdentity>>();

        return builder;
    }

    #endregion
}