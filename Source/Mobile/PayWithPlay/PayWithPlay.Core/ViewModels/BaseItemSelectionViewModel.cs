using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels
{
    public abstract class BaseItemSelectionViewModel : BaseViewModel
    {
        private bool _isLoading;
        private string? _search;

        public string SearchText => Resource.Search;
        public string NoResultsText => Resource.NoResults;
        public string CancelButtonText => Resource.Cancel;
        public string DoneButtonText => Resource.Done;

        public abstract string Title { get; }
        public abstract ItemSelectionType SelectionType { get; }

        public bool DisplayTopSelections => SelectedItems != null && SelectedItems.Any() && SelectionType == ItemSelectionType.Multiple;
        public bool DisplayBottomActions => SelectionType == ItemSelectionType.Multiple;
        public bool DisplayNoResults => DisplayedItems == null || !DisplayedItems.Any();

        public MvxObservableCollection<ItemSelectionModel> Items { get; set; } = new MvxObservableCollection<ItemSelectionModel>();
        public MvxObservableCollection<ChipModel> SelectedItems { get; set; } = new MvxObservableCollection<ChipModel>();
        public MvxObservableCollection<ItemSelectionModel> DisplayedItems { get; set; } = new MvxObservableCollection<ItemSelectionModel>();

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string? Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        public void OnItemSelection(ItemSelectionModel item)
        {
            if (SelectionType == ItemSelectionType.Single)
            {
                NavigationService.Close(this);
                return;
            }
            else
            {
                item.Selected = !item.Selected;

                var selectedItem = SelectedItems.FirstOrDefault(x => x.Id == item.Id);
                if (selectedItem != null)
                {
                    SelectedItems.Remove(selectedItem);
                }
                else
                {
                    SelectedItems.Insert(0, new ChipModel { Id = item.Id, Title = item.Name, Type = ChipType.ItemSelection });
                }

                RaisePropertyChanged(() => DisplayTopSelections);
            }
        }

        public void OnRemoveSelectedItem(int id)
        {
            var selectedItem = SelectedItems.FirstOrDefault(x => x.Id == id);
            if (selectedItem != null)
            {
                SelectedItems.Remove(selectedItem);
            }

            var item = Items.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Selected = false;
            }

            RaisePropertyChanged(() => DisplayTopSelections);
        }

        public void OnCancel()
        {
            NavigationService.Close(this);
        }

        public void OnDone()
        {
        }
    }
}
