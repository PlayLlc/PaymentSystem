using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayWithPlay.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        public static readonly IMvxNavigationService NavigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>(); 
    }
}
