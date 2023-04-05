using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace PayWithPlay.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        public static readonly IMvxNavigationService NavigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>(); 
    }

    public abstract class BaseViewModel<TParameter> : BaseViewModel, IMvxViewModel<TParameter> 
    {
        public abstract void Prepare(TParameter parameter);
    }
}
