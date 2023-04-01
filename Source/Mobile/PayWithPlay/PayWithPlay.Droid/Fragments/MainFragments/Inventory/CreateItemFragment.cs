using Android.Views;
using AndroidX.Navigation.Fragment;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    public class CreateItemFragment : BaseFragment<CreateItemViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_create_inventory_item;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            SetAddImagePlaceholder(view);

            return view;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnBackAction = OnBack;
        }

        public override void OnDestroy()
        {
            ViewModel.OnBackAction = null;

            base.OnDestroy();
        }

        private void OnBack()
        {
            FragmentKt.FindNavController(this).NavigateUp();
        }

        private void SetAddImagePlaceholder(View root) 
        {
            var placeholderView = root.FindViewById<FrameLayout>(Resource.Id.add_image_placeholder_view)!;
            var cornerRadius = 15f.ToFloatPx();
            placeholderView.SetBackground(Resource.Color.third_color, 2f.ToFloatPx(), Resource.Color.accent_color, cornerRadius);
        }
    }
}
