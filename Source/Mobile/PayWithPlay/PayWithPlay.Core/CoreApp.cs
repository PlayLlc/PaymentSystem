using MvvmCross.ViewModels;
using PayWithPlay.Core.ViewModels.Welcome;

namespace PayWithPlay.Core
{
    public class CoreApp : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<WelcomeViewModel>();
        }
    }
}
