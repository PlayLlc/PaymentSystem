using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;

using Play.Accounts.Application.Services.Emails;
using Play.Accounts.Application.Services;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Repositories;
using Play.Domain.Repositories;
using Play.Identity.Api.Services;
using Play.Persistence.Sql;
using Play.Telecom.SendGrid.Email;
using Play.Telecom.SendGrid.Sms;
using Play.Identity.Api.Identity;

namespace Play.Identity.Api.Extensions
{
    public static partial class WebApplicationBuilderExtensions
    {
        #region Instance Members

        internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var configurationManager = builder.Configuration;

            TwilioSmsConfiguration? twilioSmsConfiguration = configurationManager.GetSection(nameof(TwilioSmsConfiguration)).Get<TwilioSmsConfiguration>();
            SendGridConfiguration? sendGridConfiguration = configurationManager.GetSection(nameof(SendGridConfiguration)).Get<SendGridConfiguration>();

            // Configuration
            builder.Services.AddScoped((a) => twilioSmsConfiguration);
            builder.Services.AddScoped((a) => sendGridConfiguration);
            builder.Services.AddScoped<ISendSmsMessages, SmsClient>();
            builder.Services.AddScoped<ISendEmail, EmailClient>();

            //  builder.Services.AddTransient<IRegisterUsers, UserRegistrationService>();
            builder.Services.AddScoped<IBuildLoginViewModel, LoginViewModelBuilder>();

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

            // Repositories
            builder.Services.AddScoped<IUserRegistrationRepository, UserRegistrationRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRepository<MerchantRegistration, string>, Repository<MerchantRegistration, string>>();
            builder.Services.AddScoped<IRepository<Merchant, string>, Repository<Merchant, string>>();

            // Identity Server
            builder.Services.AddScoped<IProfileService, ProfileService<UserIdentity>>();

            return builder;
        }

        #endregion
    }
}