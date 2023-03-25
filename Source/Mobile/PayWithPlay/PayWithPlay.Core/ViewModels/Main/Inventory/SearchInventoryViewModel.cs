using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class SearchInventoryViewModel : BaseViewModel
    {
        private string? _search;

        public string Title => Resource.SearchInventory;
        public string SearchText => Resource.Search;
        public string SortByText => Resource.Search;
        public string StoreText => Resource.Store;

        public Action? OnBackAction { get; set; }

        public string? Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        public void OnBack()
        {
            OnBackAction?.Invoke();
        }
    }
}
