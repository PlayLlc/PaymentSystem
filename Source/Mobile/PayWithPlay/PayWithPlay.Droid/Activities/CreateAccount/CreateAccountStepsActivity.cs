using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using PayWithPlay.Core.ViewModels.CreateAccount;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Activities.CreateAccount
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar")]
    public class CreateAccountStepsActivity : BaseActivity<CreateAccountStepsViewModel>
    {
        private bool blockTouch;
        private MvxRecyclerView? _stepsModelsReciclerView;
        private LinearLayoutManagerWithScrollBoundery? _layaoutManager;

        protected override int LayoutId => Resource.Layout.activity_create_account_steps;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetRecyclerView();

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

        private void SetRecyclerView()
        {
            _stepsModelsReciclerView = FindViewById<MvxRecyclerView>(Resource.Id.steps_rv)!;

            _layaoutManager = new LinearLayoutManagerWithScrollBoundery(this, LinearLayoutManager.Horizontal, false) { HorizontalScrollEnabled = false };
            _layaoutManager.OnScrollStateChangedAction = (state) =>
            {
                if (state == 0)
                {
                    _layaoutManager.HorizontalScrollEnabled = false;
                    blockTouch = false;
                }
            };
            _stepsModelsReciclerView.SetLayoutManager(_layaoutManager);

            var snapHelper = new PagerSnapHelper();
            snapHelper.AttachToRecyclerView(_stepsModelsReciclerView);
        }

        private void ScrollToPoage(int page)
        {
            blockTouch = true;
            _layaoutManager!.HorizontalScrollEnabled = true;
            _stepsModelsReciclerView!.SmoothScrollToPosition(page);
        }
    }
}