using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Extensions;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Resources;
using static PayWithPlay.Core.ViewModels.BaseItemSelectionViewModel;

namespace PayWithPlay.Core.ViewModels
{
    public abstract class BaseItemSelectionViewModel : BaseViewModel<NavigationData>
    {
        public class NavigationData
        {
            public Action<List<ItemSelectionModel>>? ResultItemsAction { get; set; }

            public ItemSelectionType SelectionType { get; set; }
        }

        private bool _isLoading;
        private string? _search;

        private Action<List<ItemSelectionModel>>? _resultItemsAction;

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _resultItemsAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public override void Prepare(NavigationData parameter)
        {
            SelectionType = parameter.SelectionType;
            _resultItemsAction = parameter.ResultItemsAction;
        }

        public string SearchText => Resource.Search;
        public string NoResultsText => Resource.NoResults;
        public string CancelButtonText => Resource.Cancel;
        public string DoneButtonText => Resource.Done;

        public abstract string Title { get; }
        public ItemSelectionType SelectionType { get; set;  }

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
            set => SetProperty(ref _search, value, OnSearch);
        }

        public void OnItemSelection(ItemSelectionModel item)
        {
            if (SelectionType == ItemSelectionType.Single)
            {
                _resultItemsAction?.Invoke(new List<ItemSelectionModel> { item });
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
            _resultItemsAction?.Invoke(DisplayedItems.ToList());
            NavigationService.Close(this);
        }

        protected virtual void OnSearch() 
        {
            DisplayedItems.Clear();
            DisplayedItems.AddRange(Items.Where(item => item.Name.ComplexContains(_search)));
        }
    }
}
