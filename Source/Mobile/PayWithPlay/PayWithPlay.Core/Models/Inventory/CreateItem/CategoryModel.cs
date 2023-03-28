using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.Inventory.CreateItem
{
    public class CategoryModel : MvxNotifyPropertyChanged
    {
        private bool _categoryExpanded;
        private string? _search;
        private bool _showAddCategory;
        private string? _newCategory;

        public string CategoryText => Resource.Category;
        public string SearchText => Resource.Search;
        public string AddButtonText => Resource.Add;
        public string AddCategoryText => Resource.AddCategory;
        public string CreateCategoryText => Resource.CreateCategory;
        public string NoCategoryFoundText => Resource.NoCategoryFound;

        public bool CategoryExpanded
        {
            get => _categoryExpanded;
            set => SetProperty(ref _categoryExpanded, value);
        }

        public string? Search
        {
            get => _search;
            set
            {
                if (!SetProperty(ref _search, value) && !string.IsNullOrWhiteSpace(value)) 
                {
                    return;
                }

                DisplayedCategories.Clear();

                if (Categories != null && Categories.Count > 0)
                {
                    if (string.IsNullOrEmpty(_search))
                    {
                        DisplayedCategories.AddRange(Categories);
                    }
                    else
                    {
                        DisplayedCategories.AddRange(Categories.Where(t => t.Title!.Contains(_search, StringComparison.OrdinalIgnoreCase)));
                    }
                }

                RaisePropertyChanged(() => NoCategoryFound);
            }
        }

        public string? NewCategory
        {
            get => _newCategory;
            set => SetProperty(ref _newCategory, value);
        }

        public bool ShowAddCategory
        {
            get => _showAddCategory;
            set => SetProperty(ref _showAddCategory, value);
        }

        public bool NoCategoryFound => DisplayedCategories.Count == 0;

        public MvxObservableCollection<CategoryItemModel> DisplayedCategories { get; private set; } = new MvxObservableCollection<CategoryItemModel>();

        public List<CategoryItemModel>? Categories { get; set; }

        public void OnCategory()
        {
            CategoryExpanded = !CategoryExpanded;
        }

        public void OnCreate()
        {
            ShowAddCategory = true;
        }

        public void OnAdd()
        {
            Categories ??= new List<CategoryItemModel>();
            Categories!.Add(new CategoryItemModel { Title = NewCategory });

            Search = string.Empty;
            NewCategory = string.Empty;
            ShowAddCategory = false;

            RaisePropertyChanged(() => NoCategoryFound);
        }

        public void OnCategoryItemChecked(CategoryItemModel categoryItemModel) 
        {
            categoryItemModel.Checked = !categoryItemModel.Checked;
        }
    }
}
