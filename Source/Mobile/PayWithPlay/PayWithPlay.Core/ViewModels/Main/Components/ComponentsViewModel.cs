using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.Components;
using PayWithPlay.Core.Resources;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels.Main.Components
{
    public class ComponentsViewModel : BaseViewModel
    {
        public ComponentsViewModel()
        {
            Components.Add(new ComponentItemModel
            {
                Title = Resource.Add,
                Type = ComponentType.Add
            });
            Components.Add(new ComponentItemModel
            {
                Title = Resource.TeamManagement,
                Type = ComponentType.TeamManagement
            });
            Components.Add(new ComponentItemModel
            {
                Title = Resource.Chat,
                Type = ComponentType.Chat
            });
            Components.Add(new ComponentItemModel
            {
                Title = Resource.Invoices,
                Type = ComponentType.Invoices
            });
        }

        public string Title => Resource.Components;
        public string Subtitle => Resource.ComponentsSubtitle;

        public ObservableCollection<ComponentItemModel> Components { get; set; } = new();

        public void OnComponentItem(ComponentItemModel componentItem)
        {
        }
    }
}
