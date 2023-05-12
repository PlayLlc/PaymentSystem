using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.PointOfSale
{
    public class TransactionItemModel : MvxNotifyPropertyChanged
    {
        private bool _clicked;
        private bool _selectedToReturn;

        public Action<TransactionItemModel>? SelectedToReturnAction { get; set; }

        public string ReturnText => Resource.Return;

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string DisplayPrice => $"${Price:0.00}";

        public bool SelectedToReturn
        {
            get => _selectedToReturn;
            set => SetProperty(ref _selectedToReturn, value, () => SelectedToReturnAction?.Invoke(this));
        }

        public bool Clicked
        {
            get => _clicked;
            set => SetProperty(ref _clicked, value);
        }

        public void SetSelectedToReturn(bool selected)
        {
            if (selected)
            {
                if (!SelectedToReturn)
                {
                    SelectedToReturn = true;
                    Task.Run(async () =>
                    {
                        await Task.Delay(200);

                        Clicked = false;
                    });
                }
            }
            else
            {
                if (SelectedToReturn)
                {
                    SelectedToReturn = false;
                    Task.Run(async () =>
                    {
                        await Task.Delay(200);

                        Clicked = false;
                    });
                }
            }

        }
    }
}
