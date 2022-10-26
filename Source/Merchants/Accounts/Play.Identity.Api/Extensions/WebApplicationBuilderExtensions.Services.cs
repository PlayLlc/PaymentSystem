using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Play.Accounts.Application.Services;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Repositories;
using Play.Domain.Repositories;
using Play.Identity.Api.Services;
using Play.Persistence.Sql;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Application.Services.Sms;
using Play.Accounts.Persistence.Sql.Persistence;
using Play.Telecom.Twilio.Email;
using Play.Telecom.Twilio.Sms;

namespace Play.Identity.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configurationManager = builder.Configuration;

        TwilioSmsConfiguration? twilioSmsConfiguration = configurationManager.GetSection(nameof(TwilioSmsConfiguration)).Get<TwilioSmsConfiguration>();
        SendGridConfiguration? sendGridConfiguration = configurationManager.GetSection(nameof(SendGridConfiguration)).Get<SendGridConfiguration>();

        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddScoped<IUrlHelper>(x =>
        {
            var actionContext = x.GetService<IActionContextAccessor>().ActionContext;

            return new UrlHelper(actionContext);
        });

        // Configuration
        builder.Services.AddScoped((a) => twilioSmsConfiguration);
        builder.Services.AddScoped((a) => sendGridConfiguration);
        builder.Services.AddScoped<ISendSmsMessages, SmsClient>();
        builder.Services.AddScoped<ISendEmail, EmailClient>();

        // Repositories
        builder.Services.AddScoped<DbContext, UserIdentityDbContext>();
        builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();

        builder.Services.AddScoped<IUserRegistrationRepository, UserRegistrationRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRepository<MerchantRegistration, string>, Repository<MerchantRegistration, string>>();
        builder.Services.AddScoped<IRepository<Merchant, string>, Repository<Merchant, string>>();

        builder.Services.AddScoped<IBuildLoginViewModel, LoginViewModelBuilder>();
        builder.Services.AddScoped<ILoginUsers, UserLoginService>();

        // Infrastructure Services
        builder.Services.AddScoped<ISendSmsMessages, SmsClient>();
        builder.Services.AddScoped<ISendEmail, EmailClient>();

        // Domain Services
        builder.Services.AddScoped<IEnsureUniqueEmails, UniqueEmailChecker>();
        builder.Services.AddScoped<IHashPasswords, PasswordHasher>();
        builder.Services.AddScoped<IUnderwriteMerchants, MerchantUnderwriter>();
        builder.Services.AddScoped<IVerifyEmailAccounts, EmailAccountVerifier>();
        builder.Services.AddScoped<IVerifyMobilePhones, MobilePhoneVerifier>();
        builder.Services.AddScoped<ICreateEmailVerificationReturnUrl, EmailVerificationReturnUrlGenerator>();

        // Identity Server
        builder.Services.AddScoped<IProfileService, ProfileService<UserIdentity>>();

        return builder;
    }

    #endregion
}