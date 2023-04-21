using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class CategoriesSelectionViewModel : BaseItemSelectionViewModel
    {
        public CategoriesSelectionViewModel()
        {
            for (int i = 0; i < 100; i++)
            {
                Items.Add(new Models.ItemSelectionModel { Id = i, Name = $"Category {i}" });
            }

            DisplayedItems.AddRange(Items);
        }

        public override string Title => Resource.SelectCategories;
        public override ItemSelectionType SelectionType => ItemSelectionType.Multiple;
    }
}
