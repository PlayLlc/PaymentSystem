using Android.Views;
using MvvmCross.DroidX.Material;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;

namespace PayWithPlay.Droid.Fragments.BottomSheets
{
    public abstract class BaseBottomSheetFragment<TViewModel> : MvxBottomSheetDialogFragment<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public abstract int LayoutId { get; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _ = base.OnCreateView(inflater, container, savedInstanceState);

            var root = this.BindingInflate(LayoutId, container, false);

            return root;
        }
    }
}