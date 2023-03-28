using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.Models.Inventory.CreateItem
{
    public class StoresModel : MvxNotifyPropertyChanged
    {
        private bool _allStoresChecked;
        private bool _storesExpanded;

        public string StoresText => Resource.Stores;
        public string AllStoresText => Resource.AllStores;

        public bool StoresExpanded
        {
            get => _storesExpanded;
            set => SetProperty(ref _storesExpanded, value);
        }

        public bool AllStoresChecked
        {
            get => _allStoresChecked;
            set => SetProperty(ref _allStoresChecked, value);
        }

        public ObservableCollection<StoreItemModel> Stores { get; private set; } = new ObservableCollection<StoreItemModel>();

        public void OnStores()
        {
            StoresExpanded = !StoresExpanded;
        }

        public void OnAllStores() 
        {
            AllStoresChecked = !AllStoresChecked;

            if ((_allStoresChecked && Stores.Any(t => !t.Checked)) ||
                (!_allStoresChecked && Stores.All(t => t.Checked)))
            {
                Stores.ToList().ForEach(t => t.Checked = _allStoresChecked);
            }
        }

        public void OnStoreItemChecked(StoreItemModel storeItemModel)
        {
            storeItemModel.Checked = !storeItemModel.Checked;

            if (Stores.All(t => t.Checked) && !AllStoresChecked)
            {
                AllStoresChecked = true;
            }
            else if (!storeItemModel.Checked && AllStoresChecked)
            {
                AllStoresChecked = false;
            }
        }
    }
}
