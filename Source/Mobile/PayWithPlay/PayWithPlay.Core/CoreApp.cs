using MvvmCross.ViewModels;
using PayWithPlay.Core.ViewModels.Welcome;

namespace PayWithPlay.Core
{
    public class CoreApp : MvxApplication
    {
        public override void Initialize()
        {
            //Mvx.IoCProvider.RegisterSingleton(() => RestService.For<IApiService>("https://api-dev.cteleport.com"));

            RegisterAppStart<WelcomeViewModel>();
        }
    }
}
