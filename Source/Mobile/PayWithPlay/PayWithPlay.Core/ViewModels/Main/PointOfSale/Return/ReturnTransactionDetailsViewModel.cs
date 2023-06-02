using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.PointOfSale;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Return
{
    public class ReturnTransactionDetailsViewModel : BaseViewModel
    {
        private decimal _totalToReturn;

        public ReturnTransactionDetailsViewModel()
        {
            for (int i = 0; i < 10; i++)
            {
                Items.Add(new TransactionItemModel
                {
                    Name = MockDataUtils.RandomString(),
                    Price = MockDataUtils.RandomDecimal(),
                    SelectedToReturnAction = OnItemSelectedToReturn
                });
            }
        }

        public string Title => Resource.Return;
        public string TransactionDetailsText => Resource.TransactionDetails;
        public string ItemText => Resource.Item;
        public string PriceText => Resource.Price;
        public string TotalText => Resource.Total;
        public string ReturnButtonText => Resource.Return;

        public decimal TotalToReturn
        {
            get => _totalToReturn;
            set => SetProperty(ref _totalToReturn, value, () => RaisePropertyChanged(() => DisplayTotalToReturn));
        }

        public string DisplayTotalToReturn => $"${TotalToReturn:0.00}";

        public MvxObservableCollection<TransactionItemModel> Items { get; set; } = new MvxObservableCollection<TransactionItemModel>();
        public MvxObservableCollection<TransactionItemModel> ItemsToReturn { get; set; } = new MvxObservableCollection<TransactionItemModel>();

        public bool AnyItemsToReturn => ItemsToReturn.Any();

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnItemSelected(TransactionItemModel item)
        {
            item.Clicked = !item.Clicked;
        }

        public void OnItemSelectedToReturn(TransactionItemModel item)
        {
            if (item.SelectedToReturn)
            {
                if (!ItemsToReturn.Contains(item))
                {
                    TotalToReturn += item.Price;
                    ItemsToReturn.Add(item);
                }
            }
            else
            {
                if (ItemsToReturn.Remove(item))
                {
                    TotalToReturn -= item.Price;

                }
            }

            RaisePropertyChanged(() => AnyItemsToReturn);
        }

        public void OnReturn()
        {
            NavigationService.Navigate<PaymentViewModel>();
        }
    }
}
