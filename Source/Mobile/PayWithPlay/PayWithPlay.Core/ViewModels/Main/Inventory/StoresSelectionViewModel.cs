using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class StoresSelectionViewModel : BaseItemSelectionViewModel
    {
        public StoresSelectionViewModel()
        {
            for (int i = 0; i < 100; i++)
            {
                Items.Add(new Models.ItemSelectionModel { Id = i, Name = $"Store {i}" });
            }

            DisplayedItems.AddRange(Items);
        }

        public override string Title => Resource.SelectStores;
    }
}
