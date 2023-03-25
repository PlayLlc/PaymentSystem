using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments
{
    public abstract class BaseFragment<TViewModel> : MvxFragment<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public abstract int LayoutId { get; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _ = base.OnCreateView(inflater, container, savedInstanceState);

            var root = this.BindingInflate(LayoutId, container, false);

            SetBackAndTitleView(root);

            return root;
        }

        private void SetBackAndTitleView(View root)
        {
            var backAndTitleView = root.FindViewById<View>(Resource.Id.back_and_title_view);
            if (backAndTitleView == null)
            {
                return;
            }

            backAndTitleView.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());
        }
    }
}
