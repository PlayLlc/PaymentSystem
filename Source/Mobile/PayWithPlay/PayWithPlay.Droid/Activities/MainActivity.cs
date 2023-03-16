using Android.Content.PM;
using AndroidX.Navigation.Fragment;
using AndroidX.Navigation.UI;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Button;
using Google.Android.Material.Shape;
using PayWithPlay.Core.ViewModels.Main;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class MainActivity : BaseActivity<MainViewModel>
    {
        private AndroidX.Navigation.NavController? _navController;
        private AppBarConfiguration? _appBarConfiguration;

        protected override int LayoutId => Resource.Layout.activity_main;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetSupportActionBar(FindViewById<MaterialToolbar>(Resource.Id.toolbar));
            SetupBottomNavigationView();
        }

        public override bool OnSupportNavigateUp()
        {
            return NavControllerKt.NavigateUp(_navController, _appBarConfiguration);
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
            bottomNavigationView.ClipToOutline= true;
        }
    }
}
