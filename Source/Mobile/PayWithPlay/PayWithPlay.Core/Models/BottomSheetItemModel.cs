using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;

namespace PayWithPlay.Core.Models
{
    public class BottomSheetItemModel : MvxNotifyPropertyChanged
    {
        public BottomSheetIconType IconType { get; set; }

        public string? Title { get; set; }

    }
}
