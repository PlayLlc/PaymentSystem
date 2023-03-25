using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Navigation.Fragment;
using AndroidX.Navigation.UI;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomNavigation;
using PayWithPlay.Core.ViewModels.Main;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;
using PayWithPlay.Droid.Utils.Listeners;

namespace PayWithPlay.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : BaseActivity<MainViewModel>
    {
        private LinearLayoutCompat? _rootView;
        private BottomNavigationView? _navView;

        private AndroidX.Navigation.NavController? _navController;
        private AppBarConfiguration? _appBarConfiguration;

        protected override int LayoutId => Resource.Layout.activity_main;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitViews();
            SetSupportActionBar(FindViewById<MaterialToolbar>(Resource.Id.toolbar));
            SetupBottomNavigationView();
            InitHideNavViewWhenKeyboardAppear();
        }

        public override bool OnSupportNavigateUp()
        {
            return NavControllerKt.NavigateUp(_navController, _appBarConfiguration);
        }

        private void InitViews()
        {
            _rootView = FindViewById<LinearLayoutCompat>(Resource.Id.root_view)!;
            _navView = FindViewById<BottomNavigationView>(Resource.Id.bottom_nav)!;
        }

        private void SetupBottomNavigationView()
        {
            var navHostFragment = (NavHostFragment)SupportFragmentManager.FindFragmentById(Resource.Id.nav_host_container)!;
            var bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_nav)!;

            _navController = navHostFragment.NavController!;

            // Setup the bottom navigation view with navController
            BottomNavigationViewKt.SetupWithNavController(bottomNavigationView, _navController);

            _appBarConfiguration = new AppBarConfiguration(new int[] { Resource.Id.homeScreen, Resource.Id.pointOfSaleScreen, Resource.Id.loyaltyScreen, Resource.Id.inventoryScreen, Resource.Id.componentsScreen }, null, null, null);

            ActivityKt.SetupActionBarWithNavController(this, _navController, _appBarConfiguration);

            bottomNavigationView.OutlineProvider = new TopCornerRadiusOutlineProvider(5f.ToFloatPx());
            bottomNavigationView.ClipToOutline = true;
        }

        private void InitHideNavViewWhenKeyboardAppear()
        {
            _rootView!.ViewTreeObserver!.AddOnGlobalLayoutListener(new GlobalLayoutListener(() =>
            {
                Rect? outRect = new Rect();
                _rootView.GetWindowVisibleDisplayFrame(outRect);
                var keyboardHeight = Resources!.DisplayMetrics!.HeightPixels - outRect.Bottom;
                if (keyboardHeight > 0)
                {
                    if (_navView!.Visibility != ViewStates.Gone)
                    {
                        _navView.Visibility = ViewStates.Gone;
                    }
                }
                else
                {
                    if (_navView!.Visibility != ViewStates.Visible)
                    {
                        var handler = new Handler(Looper.MainLooper);
                        handler.PostDelayed(() =>
                        {
                            if (_navView!.Visibility != ViewStates.Visible)
                            {
                                _navView.Visibility = ViewStates.Visible;

                            }
                        }, 50);
                    }
                }
            }));
        }
    }
}
