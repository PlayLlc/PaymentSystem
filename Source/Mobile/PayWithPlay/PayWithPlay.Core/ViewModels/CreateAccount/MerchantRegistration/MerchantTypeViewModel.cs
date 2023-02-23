using MvvmCross.ViewModels;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels.CreateAccount.MerchantRegistration
{
    public class MerchantTypeViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<MerchantTypeViewModel> _onContinueAction;

        public MerchantTypeViewModel(Action<MerchantTypeViewModel> onContinueAction)
        {
            _onContinueAction = onContinueAction;
        }

        public string Title => Resource.MerchantTypeTitle;

        public MerchantTypeItemModel? SelectedMerchantType { get; private set; }

        public ObservableCollection<MerchantTypeItemModel> Types { get; private set; } = new ObservableCollection<MerchantTypeItemModel>()
        {
            new MerchantTypeItemModel
            {
                Type = Enums.MerchantType.Business,
                Title= Resource.Business,
                Subtitle= Resource.MerchnatBusinessSubtitle,
            },
            new MerchantTypeItemModel
            {
                Type = Enums.MerchantType.NonProfit,
                Title= Resource.NonProfit,
                Subtitle= Resource.MerchnatNonProfitSubtitle,
            },
            new MerchantTypeItemModel
            {
                Type = Enums.MerchantType.Individual,
                Title= Resource.Individual,
                Subtitle= Resource.MerchnatIndividualSubtitle,
            }
        };

        public void OnMerchantType(MerchantTypeItemModel merchantTypeItem)
        {
            SelectedMerchantType = merchantTypeItem;
            _onContinueAction?.Invoke(this);
        }
    }
}