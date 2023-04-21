using MvvmCross.ViewModels;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels
{
    public abstract class OptionsBottomSheetViewModel : BaseViewModel
    {
        public string CancelText => Resource.Cancel;

        public abstract string? Title { get; }

        public abstract MvxObservableCollection<BottomSheetItemModel> Items { get; }

        public abstract void OnItem(BottomSheetItemModel item);

        public void OnCancel()
        {
            NavigationService.Close(this);
        }
    }

    public abstract class BottomSheetItemsViewModel<TParameter> : OptionsBottomSheetViewModel, IMvxViewModel<TParameter>
    {
        public abstract void Prepare(TParameter parameter);
    }
}
