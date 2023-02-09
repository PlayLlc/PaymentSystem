using Microsoft.Extensions.DependencyInjection;
using PayWithPlay.Core.ViewModels.CreateAccount;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using PayWithPlay.Core.ViewModels.SignIn;
using PayWithPlay.Core.ViewModels.Welcome;

namespace PayWithPlay.Core
{
    public class ServicesProvider
    {
        public static readonly ServicesProvider Current = new();

        public IServiceProvider Provider { get; private set; } = null!;

        public void RegisterDependencies()
        {
            var services = new ServiceCollection();

            // Services

            // Viewmodels
            services.AddTransient<WelcomeViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<CreateAccountViewModel>();
            services.AddTransient<VerifyEmailViewModel>();
            services.AddTransient<VerifyPhoneNumberViewModel>();

            Provider = services.BuildServiceProvider();
        }
    }
}
