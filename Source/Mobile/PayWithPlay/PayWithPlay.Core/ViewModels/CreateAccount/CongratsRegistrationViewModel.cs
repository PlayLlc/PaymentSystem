using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public class CongratsRegistrationViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<CongratsRegistrationViewModel> _onContinue;

        public CongratsRegistrationViewModel(Action<CongratsRegistrationViewModel> onContinue)
        {
            _onContinue = onContinue;
        }

        public string Title => Resource.CongratsRegsitrationTitle;
        public string Message => Resource.CongratsRegistrationMessage;
        public string GetStartedText => Resource.GetStarted;

        public void OnGetStarted()
        {
            _onContinue?.Invoke(this);
        }
    }
}
