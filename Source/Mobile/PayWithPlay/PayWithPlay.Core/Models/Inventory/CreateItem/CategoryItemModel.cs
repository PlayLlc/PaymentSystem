using MvvmCross.ViewModels;

namespace PayWithPlay.Core.Models.Inventory.CreateItem
{
    public class CategoryItemModel : MvxNotifyPropertyChanged
    {
        private bool _checked;

        public string? Title { get; set; }

        public bool Checked
        {
            get => _checked;
            set => SetProperty(ref _checked, value);
        }
    }
}