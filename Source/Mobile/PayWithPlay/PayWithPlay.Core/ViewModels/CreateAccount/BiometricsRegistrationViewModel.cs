using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public class BiometricsRegistrationViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<BiometricsRegistrationViewModel> _onContinue;
        private bool _biometricsEnabled;

        public BiometricsRegistrationViewModel(Action<BiometricsRegistrationViewModel> onContinue)
        {
            _onContinue = onContinue;
        }

        public string Title => Resource.RegistrationBiometricsTitle;
        public string BiometricsText => Resource.BiometricsSwitchText;
        public string ContinueText => Resource.Continue;

        public bool BiometricsEnabled 
        { 
            get => _biometricsEnabled;
            set => SetProperty(ref _biometricsEnabled, value);
        }

        public void OnContinue()
        {
            _onContinue?.Invoke(this);
        }
    }
}