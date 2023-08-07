using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.ProgressIndicator;
using MvvmCross.DroidX.RecyclerView;
using PayWithPlay.Core.ViewModels.CreateAccount;
using PayWithPlay.Droid.Utils;
using static Android.Views.ViewGroup;

namespace PayWithPlay.Droid.Activities.CreateAccount
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CreateAccountStepsActivity : BaseActivity<CreateAccountStepsViewModel>
    {
        private bool scrolledToPage;
        private bool blockTouch;
        private MvxRecyclerView? _stepsRecyclerView;
        private LinearLayoutManagerWithScrollBoundery? _layoutManager;
        private LinearProgressIndicator? _progressIndicator;

        protected override int LayoutId => Resource.Layout.activity_create_account_steps;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetRecyclerView();
            SetProgressIndicator();

            ViewModel!.ScrollToPageAction = ScrollToPage;
        }

        protected override void OnDestroy()
        {
            ViewModel!.ScrollToPageAction = null;

            base.OnDestroy();
        }

        public override bool DispatchTouchEvent(MotionEvent? ev)
        {
            if (!blockTouch)
            {
                base.DispatchTouchEvent(ev);
            }
            return blockTouch;
        }

        protected override bool OnBackPressed()
        {
            return ViewModel!.OnBackPressed();
        }

        private void SetRecyclerView()
        {
            _stepsRecyclerView = FindViewById<MvxRecyclerView>(Resource.Id.steps_rv)!;

            _layoutManager = new LinearLayoutManagerWithScrollBoundery(this, LinearLayoutManager.Horizontal, false) { HorizontalScrollEnabled = false };
            _layoutManager.OnScrollStateChangedAction = (state) =>
            {
                if (state == (int)ScrollState.Idle)
                {
                    _layoutManager.HorizontalScrollEnabled = false;
                    blockTouch = false;
                    if (scrolledToPage)
                    {
                        scrolledToPage = false;
                        var view = _stepsRecyclerView!.FindViewHolderForAdapterPosition(ViewModel!.CurrentPageIndex);
                        if (view != null)
                        {
                            new Handler(Looper.MainLooper).Post(() => view.ItemView.RequestLayout());
                        }
                    }
                }
            };
            _stepsRecyclerView.SetLayoutManager(_layoutManager);

            var snapHelper = new PagerSnapHelper();
            snapHelper.AttachToRecyclerView(_stepsRecyclerView);
        }

        private void ScrollToPage(int page)
        {
            App.Current!.ClearFocusAndHideKeyboard();
            blockTouch = true;
            scrolledToPage = true;
            _layoutManager!.HorizontalScrollEnabled = true;
            _stepsRecyclerView!.SmoothScrollToPosition(page);
        }

        private void SetProgressIndicator()
        {
            _progressIndicator = FindViewById<LinearProgressIndicator>(Resource.Id.progress_indicatorView)!;

            var lp = _progressIndicator.LayoutParameters as MarginLayoutParams;
            lp!.Width = (int)(Resources!.DisplayMetrics!.WidthPixels * 0.384);
            _progressIndicator.LayoutParameters = lp;

            _progressIndicator.LayoutParameters = lp;
        }
    }
}