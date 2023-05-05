using Android.Content.PM;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.BottomSheet;
using MvvmCross.DroidX.RecyclerView;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;
using PayWithPlay.Droid.Utils.Listeners;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, WindowSoftInputMode = SoftInput.AdjustPan)]
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
        }
    }
}
