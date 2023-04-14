using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class LoyaltyRewardsViewModel : BaseViewModel
    {
        private readonly List<ITextInputValidator> _inputValidators = new();

        private bool _rewardsEnabled;
        private string? _pointsPerDollar;
        private string? _pointsRequired;
        private string? _reward;

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _inputValidators.Clear();
        }

        public string Title => Resource.Rewards;
        public string Subtitle => Resource.RewardsSubtitle;
        public string PointsPerDollarText => Resource.PointPerDollar;
        public string PointsRequiredText => Resource.PointsRequired;
        public string RewardText => Resource.Reward;
        public string SaveButtonText => Resource.Save;

        public bool RewardsEnabled
        {
            get => _rewardsEnabled;
            set => SetProperty(ref _rewardsEnabled, value, () => RaisePropertyChanged(() => SaveButtonEnabled));
        }

        public string? PointsPerDollar
        {
            get => _pointsPerDollar;
            set => SetProperty(ref _pointsPerDollar, value, () => RaisePropertyChanged(() => SaveButtonEnabled));
        }

        public string? PointsRequired
        {
            get => _pointsRequired;
            set => SetProperty(ref _pointsRequired, value, () => RaisePropertyChanged(() => SaveButtonEnabled));
        }

        public string? Reward
        {
            get => _reward;
            set => SetProperty(ref _reward, value, () => RaisePropertyChanged(() => SaveButtonEnabled));
        }

        public bool SaveButtonEnabled => !RewardsEnabled || ValidationHelper.AreInputsValid(_inputValidators, false);

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnSave()
        {
        }

        public void SetInputValidators(ITextInputValidator pointsPerDollar, ITextInputValidator pointsRequired, ITextInputValidator reward) 
        {
            _inputValidators.Clear();

            _inputValidators.Add(pointsPerDollar);
            _inputValidators.Add(pointsRequired);
            _inputValidators.Add(reward);

            RaisePropertyChanged(() => SaveButtonEnabled);
        }
    }
}
