using MvvmCross.Navigation;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class ManageStockViewModel : BaseViewModel
    {
        private readonly IWheelPicker _wheelPicker;
        private readonly string[] _reasonValues = new[] { Resource.Restock, Resource.Return, Resource.Sold };
        private string? _store;
        private string? _reason;
        private string? _quantity;

        public ManageStockViewModel(IWheelPicker wheelPicker)
        {
            _wheelPicker = wheelPicker;
        }

        public string Title => Resource.ManageStock;

        public string QuantityText => Resource.Quantity;
        public string StoreText => Resource.Store;
        public string ReasonText => Resource.Reason;
        public string SaveButtonText => Resource.Save;
        public string SelectText => Resource.Select;

        public string? Store
        {
            get => _store;
            set => SetProperty(ref _store, value, ()=> RaisePropertyChanged(()=> SaveButtonEnabled));
        }

        public string? Reason
        {
            get => _reason;
            set => SetProperty(ref _reason, value, () => RaisePropertyChanged(() => SaveButtonEnabled));
        }

        public string? Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value, () => RaisePropertyChanged(() => SaveButtonEnabled));
        }

        public bool SaveButtonEnabled => !string.IsNullOrWhiteSpace(Store) &&
                                         !string.IsNullOrWhiteSpace(Reason) &&
                                         !string.IsNullOrWhiteSpace(Quantity);

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnStore()
        {
            NavigationService.Navigate<StoresSelectionViewModel, BaseItemSelectionViewModel.NavigationData>(new BaseItemSelectionViewModel.NavigationData 
            {
                ResultItemsAction = (items) => 
                {
                    Store = items?.FirstOrDefault()?.Name;
                },
                SelectionType = ItemSelectionType.Single
            });
        }

        public void OnReason()
        {
            var index = Array.IndexOf(_reasonValues, Reason);

            _wheelPicker.Show(_reasonValues, index == -1 ? 0 : index, Resource.SelectReason, Resource.Ok, Resource.Cancel, (index) =>
            {
                Reason = _reasonValues[index];
            });
        }

        public void OnSave()
        {
        }
    }
}
