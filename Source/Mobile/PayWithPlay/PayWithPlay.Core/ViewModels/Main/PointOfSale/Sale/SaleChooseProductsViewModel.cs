using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Models.PointOfSale;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleChooseProductsViewModel : BaseViewModel
    {
        private PageType _currentPage = PageType.Scan;
        private decimal _totalPrice;
        private bool _bottomSheetExpanded;

        public SaleChooseProductsViewModel()
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            PageChangedAction = null;
            BottomSheetExpandedAction = null;
            TotalPriceChangeAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public enum PageType { Scan = 0, SelectItem = 1, CustomAmount = 2 }

        public Action<PageType>? PageChangedAction { get; set; }
        public Action<bool>? BottomSheetExpandedAction { get; set; }
        public Action? TotalPriceChangeAction { get; set; }

        public string ScanButtonText => Resource.Scan;
        public string SelectItemButtonText => Resource.SelectItem;
        public string CustomAmountButtonText => Resource.CustomAmount;
        public string TransactionDetailsText => Resource.TransactionDetails;
        public string ItemsText => Resource.Items;
        public string ItemText => Resource.Item;
        public string PriceText => Resource.Price;
        public String TaxText => Resource.Tax;
        public string TotalText => Resource.Total;
        public string ContinueButtonText => Resource.Continue;

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; }

        public PageType CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value, () =>
            {
                PageChangedAction?.Invoke(_currentPage);
                RaisePropertyChanged(() => ScanButtonSelected);
                RaisePropertyChanged(() => SelectItemButtonSelected);
                RaisePropertyChanged(() => CustomAmountButtonSelected);
            });
        }

        public decimal TaxValue => TotalPrice * 0.19m;

        public string TaxValueDispalyed => $"${TaxValue:0.00}";

        public string TotalPriceDisplayed => $"${TotalPrice:0.00}";

        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value, () =>
            {
                RaisePropertyChanged(() => TotalPriceDisplayed);
                RaisePropertyChanged(() => TaxValueDispalyed);
                RaisePropertyChanged(() => ItemsCount);
                RaisePropertyChanged(() => ItemsAdded);
                TotalPriceChangeAction?.Invoke();
            });
        }

        public bool BottomSheetExpanded
        {
            get => _bottomSheetExpanded;
            set => SetProperty(ref _bottomSheetExpanded, value, () => BottomSheetExpandedAction?.Invoke(_bottomSheetExpanded));
        }

        public bool ScanButtonSelected => CurrentPage == PageType.Scan;
        public bool SelectItemButtonSelected => CurrentPage == PageType.SelectItem;
        public bool CustomAmountButtonSelected => CurrentPage == PageType.CustomAmount;

        public MvxObservableCollection<ProductItemModel> Items { get; set; } = new MvxObservableCollection<ProductItemModel>();

        public string ItemsCount => $"{Items?.Count}";

        public bool ItemsAdded => Items.Count > 0;

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnScan()
        {
            BottomSheetExpanded = false;
            CurrentPage = PageType.Scan;
        }

        public void OnSelectItem()
        {
            BottomSheetExpanded = false;
            CurrentPage = PageType.SelectItem;
        }

        public void OnCustomAmount()
        {
            BottomSheetExpanded = false;
            CurrentPage = PageType.CustomAmount;
        }

        public void OnItemClick(ProductItemModel item)
        {
            var currentSelected = Items.FirstOrDefault(t => t.Selected);
            if (currentSelected != null && currentSelected != item)
            {
                currentSelected.Selected = false;
            }

            item.Selected = !item.Selected;
        }

        public void OnContinue()
        {
            if (!BottomSheetExpanded)
            {
                BottomSheetExpanded = true;
                return;
            }

            NavigationService.Navigate<SaleEnterLoyaltyMemberViewModel>();
        }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>
            {
                NavigationService.Navigate<SaleScanItemViewModel, Action<string>>(OnItemScanned),
                NavigationService.Navigate<SaleSelectItemViewModel, Action<InventoryItemModel>>(OnItemSelected),
                NavigationService.Navigate<SaleCustomAmountViewModel, Action<Tuple<string?, string?>>>(OnNewCustomAmount)
            };

            return Task.WhenAll(tasks);
        }

        private void OnItemScanned(string value)
        {
            var sameItem = Items.FirstOrDefault(t => string.Equals(t.Title, value));
            var productId = sameItem == null ? Guid.NewGuid() : sameItem.ProductItemId;

            var newProductItem = new ProductItemModel
            {
                ProductItemId = productId,
                Title = value,
                Price = 7.7m,
                AddAction = OnAddMoreProductItem,
                RemoveAction = OnRemoveProductItem
            };

            //Items.Insert(0, newProductItem);
            Items.Add(newProductItem);
            TotalPrice += newProductItem.Price;
        }

        private void OnItemSelected(InventoryItemModel inventoryItem)
        {
            var newProductItem = new ProductItemModel
            {
                ProductItemId = inventoryItem.Id,
                Title = inventoryItem.Name,
                Price = inventoryItem.Price,
                AddAction = OnAddMoreProductItem,
                RemoveAction = OnRemoveProductItem
            };

            //Items.Insert(0, newProductItem);
            Items.Add(newProductItem);
            TotalPrice += newProductItem.Price;
        }

        private void OnNewCustomAmount(Tuple<string?, string?> customAmountTuple)
        {
            var newProductItem = new ProductItemModel
            {
                Title = string.IsNullOrWhiteSpace(customAmountTuple.Item2) ? Resource.CustomAmount : customAmountTuple.Item2,
                Price = decimal.TryParse(customAmountTuple.Item1, out decimal value) ? value : 0.0m,
                AddAction = OnAddMoreProductItem,
                RemoveAction = OnRemoveProductItem
            };

            //Items.Insert(0, newProductItem);
            Items.Add(newProductItem);
            TotalPrice += newProductItem.Price;
        }

        private void OnAddMoreProductItem(ProductItemModel item)
        {
            var currentItemsCount = Items.Count(t => t.ProductItemId == item.ProductItemId);

            NavigationService.Navigate<SaleItemQuantityDialogViewModel, Tuple<string, Action<string>>>(Tuple.Create<string, Action<string>>(currentItemsCount.ToString(), (value) =>
            {
                if (!int.TryParse(value, out int quantity))
                {
                    return;
                }
                item.Selected = false;

                if (quantity == 0 || quantity < currentItemsCount)
                {
                    var itemsToBeRemoved = Items.Where(t => t.ProductItemId == item.ProductItemId).Take(currentItemsCount - quantity).ToList();
                    Items.RemoveItems(itemsToBeRemoved);
                    TotalPrice -= itemsToBeRemoved.Sum(t => t.Price);
                }
                else
                {
                    if (currentItemsCount == quantity)
                    {
                        return;
                    }

                    var itemIndex = Items.IndexOf(item);

                    for (int i = 0; i < quantity - currentItemsCount; i++)
                    {
                        Items.Insert(itemIndex, item.Clone());
                        TotalPrice += item.Price;
                    }
                }

                if (Items.Count == 0)
                {
                    BottomSheetExpanded = false;
                }
            }));
        }

        private void OnRemoveProductItem(ProductItemModel item)
        {
            Items.Remove(item);

            TotalPrice -= item.Price;

            if (Items.Count == 0)
            {
                BottomSheetExpanded = false;
            }
        }
    }
}
