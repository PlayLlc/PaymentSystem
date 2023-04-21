using MvvmCross.ViewModels;

namespace PayWithPlay.Core.Models
{
    public class ItemSelectionModel : MvxNotifyPropertyChanged
    {
        private bool _selected;

        public int Id { get; set; }

        public string? Name { get; set; }

        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }
    }
}
