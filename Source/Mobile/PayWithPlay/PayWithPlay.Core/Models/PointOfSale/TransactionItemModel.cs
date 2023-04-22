using MvvmCross.ViewModels;

namespace PayWithPlay.Core.Models.PointOfSale
{
    public class TransactionItemModel : MvxNotifyPropertyChanged
    {
        private bool _selectedToReturn;

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string DisplayPrice => $"${Price:0.00}";

        public bool SelectedToReturn
        {
            get => _selectedToReturn;
            set => SetProperty(ref _selectedToReturn, value);
        }
    }
}
