using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class InventoryViewModel : BaseViewModel
    {
        public InventoryViewModel()
        {
        }

        public string Title => Resource.Inventory;
        public string Subtitle => Resource.InventorySubtitle;
        public string SearchButtonText => Resource.Search;
        public string AddButtonText => Resource.Add;
        public string ScanButtonText => Resource.Scan;

        public Action? OnSearchAction { get; set; }
        public Action? OnAddAction { get; set; }

        public void OnSearch() 
        {
            OnSearchAction?.Invoke();
        }

        public void OnAdd()
        {
            OnAddAction?.Invoke();
        }

        public void OnScan() { }
    }
}
