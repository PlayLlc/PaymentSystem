using Android.Views;
using AndroidX.AppCompat.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PayWithPlay.Core.Models.PointOfSale;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.ViewHolders
{
    public class TransactionViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatSeekBar _seekbar;
        private int _minSeekValue = 7;
        private int _maxSeekValue = 93;
        private int _threshold = 50;

        private TransactionItemModel _itemModel => BindingContext.DataContext as TransactionItemModel;

        public TransactionViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
            var seekBarContainer = itemView.FindViewById<FrameLayout>(Resource.Id.seekBar_container)!;

            seekBarContainer.SetBackground(Resource.Color.third_color, cornerRadius: 30f.ToFloatPx());

            _seekbar = itemView.FindViewById<AppCompatSeekBar>(Resource.Id.seekBar)!;

            itemView.Post(() =>
            {
                _seekbar.Min = 0;
                _seekbar.Max = itemView.Width;

                _minSeekValue = 24f.ToPx();
                _maxSeekValue = itemView.Width - 24f.ToPx();
                _threshold = (int)(0.75f * itemView.Width);

                SetSeekProgress();
            });

            _seekbar.ProgressChanged -= OnProgressChanged;
            _seekbar.ProgressChanged += OnProgressChanged;
            _seekbar.StopTrackingTouch -= OnStopTrackingTouch;
            _seekbar.StopTrackingTouch += OnStopTrackingTouch;
        }

        public void OnBind()
        {
            SetSeekProgress();
        }

        private void SetSeekProgress() 
        {
            if (BindingContext.DataContext is not TransactionItemModel transactionItemModel)
            {
                return;
            }

            if (transactionItemModel.SelectedToReturn)
            {
                _seekbar.Progress = _maxSeekValue;
            }
            else
            {
                _seekbar.Progress = _minSeekValue;
            }
        }

        private void OnProgressChanged(object? sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (_seekbar.Progress > _maxSeekValue)
            {
                _seekbar.Progress = _maxSeekValue;
            }
            else if (_seekbar.Progress < _minSeekValue)
            {
                _seekbar.Progress = _minSeekValue;
            }
        }

        private void OnStopTrackingTouch(object? sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            if (_seekbar.Progress > _threshold) 
            {
                _seekbar.SetProgress(_maxSeekValue, true);
                _itemModel?.SetSelectedToReturn(true);
            }
            else
            {
                _seekbar.SetProgress(_minSeekValue, true);
                _itemModel?.SetSelectedToReturn(false);
            }
        }
    }
}
