using Android.Views;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.Button;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils.Listeners;

namespace PayWithPlay.Droid.ViewHolders
{
    public class InventoryItemViewHolder : MvxRecyclerViewHolder
    {
        private LinearLayoutCompat? _buttonsActionsContainer;

        private FrameLayout? _deletingViewContainer;
        private View? _deletingBackgroundView;
        private MaterialButton? _slidingDeleteButton;

        private InventoryItemModel? _itemModel => BindingContext.DataContext as InventoryItemModel;

        public InventoryItemViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
            InitViews(itemView);
        }

        public void OnBind()
        {
            if (BindingContext.DataContext is not InventoryItemModel inventoryItemModel)
            {
                return;
            }
        }

        private void InitViews(View itemView)
        {
            _buttonsActionsContainer = itemView.FindViewById<LinearLayoutCompat>(Resource.Id.buttons_actions_container)!;
            _buttonsActionsContainer.SetBackground(Resource.Color.third_color, cornerRadius: 5f.ToFloatPx());

            _deletingViewContainer = itemView.FindViewById<FrameLayout>(Resource.Id.fl_deleting_container)!;
            _deletingBackgroundView = itemView.FindViewById<View>(Resource.Id.backgroundView)!;
            _slidingDeleteButton = itemView.FindViewById<MaterialButton>(Resource.Id.btn_delete_sliding)!;

            _deletingBackgroundView.SetBackground(Resource.Color.negative_color, cornerRadius: 4f.ToFloatPx());

            _slidingDeleteButton.Touch += OnTouchSlidingDeleteButton;
            _slidingDeleteButton.ViewTreeObserver!.AddOnGlobalLayoutListener(new GlobalLayoutListener(() => 
            {
                UpdateDeletingBackgroundViewWidth((int)_slidingDeleteButton.GetX());
            }));
        }

        private float dX;
        private bool _isSliding;

        private void OnTouchSlidingDeleteButton(object? sender, View.TouchEventArgs e)
        {
            var ev = e.Event!;

            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    dX = ev.RawX - _slidingDeleteButton!.GetX();
                    _isSliding = true;
                    break;
                case MotionEventActions.Move:
                    if (_slidingDeleteButton!.Parent != null)
                    {
                        _slidingDeleteButton.Parent.RequestDisallowInterceptTouchEvent(true);
                    }
                    var newX = Math.Min(Math.Max(ev.RawX - dX, _deletingViewContainer!.PaddingStart), _deletingViewContainer.Width - _slidingDeleteButton.Width - _deletingViewContainer.PaddingEnd);

                    _slidingDeleteButton.SetX(newX);
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    if (_isSliding)
                    {
                        if ((float)_deletingBackgroundView!.Width / (_deletingViewContainer!.Width - _slidingDeleteButton!.Width - _deletingViewContainer.PaddingStart - _deletingViewContainer.PaddingEnd) > 0.7f)
                        {
                            _slidingDeleteButton!.Animate()!.X(_deletingViewContainer.Width - _slidingDeleteButton.Width - _deletingViewContainer.PaddingEnd).SetDuration(300).Start();
                            _itemModel?.OnSwipeToDelete();
                        }
                        else
                        {
                            _slidingDeleteButton!.Animate()!.X(_deletingViewContainer!.PaddingStart).SetDuration(300).Start();
                        }
                    }
                    _isSliding = false;
                    break;
            }
        }

        private void UpdateDeletingBackgroundViewWidth(int newWidth)
        {
            var lp = _deletingBackgroundView!.LayoutParameters!;
            lp.Width = newWidth;
            _deletingBackgroundView.LayoutParameters = lp;
        }
    }
}
