using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class ManageStockViewModel : BaseViewModel
    {
        private readonly IWheelPicker _wheelPicker;
        private readonly string[] _reasonValues = new[] { Resource.Restock, Resource.Return, Resource.Sold };
        private string? _reason;
        private string? _quantity;

        public ManageStockViewModel(IWheelPicker wheelPicker)
        {
            _wheelPicker = wheelPicker;
        }

        public string Title => Resource.ManageStock;

        public string QuantityText => Resource.Quantity;
        public string LocationText => Resource.Location;
        public string ReasonText => Resource.Reason;
        public string SaveButtonText => Resource.Save;
        public string SelectText => Resource.Select;

        public string? Reason
        {
            get => _reason;
            set => SetProperty(ref _reason, value);
        }

        public string? Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public bool SaveButtonEnabled => true;

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnLocation()
        {
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
