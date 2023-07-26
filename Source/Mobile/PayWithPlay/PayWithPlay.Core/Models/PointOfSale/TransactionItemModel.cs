using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.PointOfSale
{
    public class TransactionItemModel : MvxNotifyPropertyChanged
    {
        private bool _checked;

        public Action<TransactionItemModel>? SelectedToReturnAction { get; set; }

        public string ReturnText => Resource.Return;

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string DisplayPrice => $"${Price:0.00}";

        public bool Checked
        {
            get => _checked;
            set => SetProperty(ref _checked, value);
        }
    }
}
