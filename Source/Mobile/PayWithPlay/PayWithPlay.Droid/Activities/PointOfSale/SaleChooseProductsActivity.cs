using Android.Content.PM;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.Button;
using MvvmCross.DroidX.RecyclerView;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Fragments.MainFragments.PointOfSale;
using PayWithPlay.Droid.Utils;
using PayWithPlay.Droid.Utils.Listeners;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class SaleChooseProductsActivity : BaseActivity<SaleChooseProductsViewModel>
    {
        private DraggableBottomSheetBehavior? _bottomSheetBehavior;
        private NonSwipebleViewPager? _viewPager;

        private View? _collapsedDetailsView;
        private View? _expandedDetailsView;

        protected override int LayoutId => Resource.Layout.activity_sale_choose_products;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitViews();

            if (savedInstanceState == null)
            {
                ViewModel.ShowInitialViewModelsCommand.Execute();
            }

            ViewModel.PageChangedAction = OnPageChanged;
            ViewModel.BottomSheetExpandedAction = OnBottomSheetExpanded;
            ViewModel.TotalPriceChangeAction = OnTotalPriceChanged;

            OnTotalPriceChanged();
        }

        private void OnPageChanged(SaleChooseProductsViewModel.PageType page)
        {
            _viewPager!.SetCurrentItem((int)page, true);

            var scanItemFragment = (SaleScanItemFragment)SupportFragmentManager.FindFragmentByTag(SaleScanItemFragment.FRAGMENT_TAG)!;
            if (page == 0)
            {
                scanItemFragment.StartScanning();
            }
            else
            {
                scanItemFragment.StopScanning();
            }
        }

        private void OnBottomSheetExpanded(bool expanded)
        {
            if (expanded)
            {
                if (_collapsedDetailsView!.Visibility != ViewStates.Gone)
                {
                    _collapsedDetailsView.Visibility = ViewStates.Gone;
                }
                if (_bottomSheetBehavior!.State != BottomSheetBehavior.StateExpanded)
                {
                    _bottomSheetBehavior.State = BottomSheetBehavior.StateExpanded;
                }
            }
            else
            {
                if (_bottomSheetBehavior!.State != BottomSheetBehavior.StateCollapsed)
                {
                    _bottomSheetBehavior.State = BottomSheetBehavior.StateCollapsed;
                }
                if (_collapsedDetailsView!.Visibility != ViewStates.Visible)
                {
                    _collapsedDetailsView.Visibility = ViewStates.Visible;
                }
            }
        }

        private void OnTotalPriceChanged()
        {
            _bottomSheetBehavior!.Draggable = ViewModel.Items.Count > 0;
        }

        private void InitViews()
        {
            _collapsedDetailsView = FindViewById<View>(Resource.Id.collapsed_details_container);
            _expandedDetailsView = FindViewById<View>(Resource.Id.expanded_details_container);

            _viewPager = FindViewById<NonSwipebleViewPager>(Resource.Id.viewpager)!;
            _viewPager.OffscreenPageLimit = 3;

            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.items_rv)!;
            recyclerView.SetItemAnimator(null);

            var actionsView = FindViewById<LinearLayoutCompat>(Resource.Id.actions_container);
            actionsView.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());

            var bottomSheetView = FindViewById<LinearLayoutCompat>(Resource.Id.bottomSheet)!;
            bottomSheetView.OutlineProvider = new TopCornerRadiusOutlineProvider(5f.ToFloatPx());
            bottomSheetView.ClipToOutline = true;

            _bottomSheetBehavior = (DraggableBottomSheetBehavior)BottomSheetBehavior.From(bottomSheetView);
            _bottomSheetBehavior.PreventDragView = FindViewById<View>(Resource.Id.list_container);
            _bottomSheetBehavior.AddBottomSheetCallback(new BottomSheetBehaviorListener
            {
                StateChangedAction = (view, state) =>
                {
                    switch (state)
                    {
                        case BottomSheetBehavior.StateCollapsed:
                            ViewModel.BottomSheetExpanded = false;
                            if (_collapsedDetailsView!.Visibility != ViewStates.Visible)
                            {
                                _collapsedDetailsView.Visibility = ViewStates.Visible;
                            }
                            break;
                        case BottomSheetBehavior.StateExpanded:
                            ViewModel.BottomSheetExpanded = true;
                            if (_collapsedDetailsView!.Visibility != ViewStates.Gone)
                            {
                                _collapsedDetailsView.Visibility = ViewStates.Gone;
                            }
                            break;
                        case BottomSheetBehavior.StateDragging:
                            if (_collapsedDetailsView!.Visibility != ViewStates.Gone)
                            {
                                _collapsedDetailsView.Visibility = ViewStates.Gone;
                            }
                            break;
                        case BottomSheetBehavior.StateSettling:
                            break;
                    }
                }
            });

            SetLayoutForNarrowSizes();
        }

        private void SetLayoutForNarrowSizes()
        {
            var width = Resources!.DisplayMetrics!.WidthPixels;
            var density = Resources!.DisplayMetrics!.Density;
            if (width / density < 340f)
            {
                var actionsView = FindViewById<LinearLayoutCompat>(Resource.Id.actions_container)!;
                actionsView.SetPadding(32f.ToPx(), actionsView.PaddingTop, 8f.ToPx(), actionsView.PaddingBottom);

                var scanBtn = FindViewById<MaterialButton>(Resource.Id.scan_btn)!;
                var selectItemBtn = FindViewById<MaterialButton>(Resource.Id.select_item_btn)!;
                var customAmountBtn = FindViewById<MaterialButton>(Resource.Id.custom_amount_btn)!;
                scanBtn.SetMargins(end: 3f.ToPx());
                selectItemBtn.SetMargins(start: 3f.ToPx(), end: 3f.ToPx());
                customAmountBtn.SetMargins(start: 3f.ToPx());

                var backBtn = FindViewById<MaterialButton>(Resource.Id.back_btn)!;
                backBtn.SetSize(36f.ToPx(), 36f.ToPx());
            }
        }
    }
}
