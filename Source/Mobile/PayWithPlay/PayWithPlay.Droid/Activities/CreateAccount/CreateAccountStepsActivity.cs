using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.ProgressIndicator;
using MvvmCross.DroidX.RecyclerView;
using PayWithPlay.Core.ViewModels.CreateAccount;
using PayWithPlay.Droid.Utils;
using PayWithPlay.Droid.Utils.Listeners;
using static Android.Views.ViewGroup;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace PayWithPlay.Droid.Activities.CreateAccount
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar")]
    public class CreateAccountStepsActivity : BaseActivity<CreateAccountStepsViewModel>
    {
        private bool scrolledToPage;
        private bool blockTouch;
        private MvxRecyclerView? _stepsRecyclerView;
        private LinearLayoutManagerWithScrollBoundery? _layaoutManager;
        private LinearProgressIndicator? _progressIndicator;

        protected override int LayoutId => Resource.Layout.activity_create_account_steps;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetRecyclerView();
            SetProgressIndicator();

            ViewModel.ScrollToPageAction = ScrollToPoage;
        }

        protected override void OnDestroy()
        {
            ViewModel.ScrollToPageAction = null;

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

        //protected override bool OnBackPressed()
        //{
        //    return false;
        //}

        private void SetRecyclerView()
        {
            _stepsRecyclerView = FindViewById<MvxRecyclerView>(Resource.Id.steps_rv)!;

            _layaoutManager = new LinearLayoutManagerWithScrollBoundery(this, LinearLayoutManager.Horizontal, false) { HorizontalScrollEnabled = false };
            _layaoutManager.OnScrollStateChangedAction = (state) =>
            {
                if (state == (int)ScrollState.Idle)
                {
                    _layaoutManager.HorizontalScrollEnabled = false;
                    blockTouch = false;
                    if (scrolledToPage)
                    {
                        scrolledToPage = false;
                        var view = _stepsRecyclerView!.FindViewHolderForAdapterPosition(ViewModel.CurrentPageIndex);
                        if (view != null)
                        {
                            new Handler(Looper.MainLooper).Post(() => view.ItemView.RequestLayout());
                        }
                    }
                }
            };
            _stepsRecyclerView.SetLayoutManager(_layaoutManager);

            var snapHelper = new PagerSnapHelper();
            snapHelper.AttachToRecyclerView(_stepsRecyclerView);
        }

        private void ScrollToPoage(int page)
        {
            App.Current!.ClearFocusAndHideKeyboard();
            blockTouch = true;
            scrolledToPage = true;
            _layaoutManager!.HorizontalScrollEnabled = true;
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