using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.PIN
{
    public class PINViewModel : BaseViewModel, ICreateAccountStep
    {
        public const string PINSignIn = "PINSignIn";

        private readonly Action<PINViewModel>? _continueAction;
        private string? _inputValue;

        public PINViewModel()
        {
        }

        public PINViewModel(Action<PINViewModel> continueAction, PINPageType pageType)
        {
            _continueAction = continueAction;
            Type = pageType;
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            if (parameters == null || parameters.Data == null)
            {
                return;
            }

            if (parameters.Data.ContainsKey(PINSignIn))
            {
                Type = PINPageType.SignInConfirm;
            }
        }

        public PINPageType Type { get; set; }

        public string? InputValue
        {
            get => _inputValue;
            set => SetProperty(ref _inputValue, value);
        }

        public bool FingerprintEnabled => Type == PINPageType.SignInConfirm;

        public string ActionText => Type switch
        {
            PINPageType.SignInConfirm => Resource.ConfirmPINCode,
            PINPageType.Create => Resource.CreatePINCode,
            PINPageType.CreateConfirm => Resource.ConfirmPINCode,
            _ => string.Empty,
        };

        public string ContinueButtonText => Type switch
        {
            PINPageType.SignInConfirm => Resource.Confirm,
            PINPageType.Create => Resource.Continue,
            PINPageType.CreateConfirm => Resource.Confirm,
            _ => string.Empty,
        };

        public string SafeMessage => Resource.PINSafeMessage;

        public void OnContinue()
        {
            if (Type == PINPageType.SignInConfirm)
            {
            }
            else
            {
                _continueAction?.Invoke(this);
            }
        }

        public void OnKey(NumericKeyboardKeyType keyType)
        {
            if (keyType == NumericKeyboardKeyType.Remove)
            {
                InputValue = string.Empty;
            }
            else if (keyType != NumericKeyboardKeyType.FingerPrint)
            {
                InputValue += ((int)keyType).ToString();
            }
        }
    }
}