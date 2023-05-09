using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.Core.Content.Resources;
using AndroidX.Core.Content;
using Google.Android.Material.Chip;
using MvvmCross.Binding;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.WeakSubscription;
using PayWithPlay.Core.Models;
using PayWithPlay.Droid.Extensions;
using System.Collections;
using System.Collections.Specialized;
using Android.Graphics;
using PayWithPlay.Droid.Utils.Listeners;
using System.Windows.Input;

namespace PayWithPlay.Droid.CustomViews
{
    public class MvxChipGroup : ChipGroup
    {
        private IEnumerable? _itemsSource;
        private ICommand? _closeClick;
        private IDisposable? _subscription;

        #region ctors

        public MvxChipGroup(Context? context) : base(context)
        {
        }

        public MvxChipGroup(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
        }

        public MvxChipGroup(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected MvxChipGroup(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        [MvxSetToNullAfterBinding]
        public IEnumerable? ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (ReferenceEquals(_itemsSource, value))
                {
                    return;
                }

                _subscription?.Dispose();
                _subscription = null;

                if (value != null && value is not IList)
                {
                    MvxBindingLog.Warning("Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
                }

                if (value is INotifyCollectionChanged newObservable)
                    _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);

                _itemsSource = value;
                ResetItems();
            }
        }

        [MvxSetToNullAfterBinding]
        public ICommand? CloseClick
        {
            get => _closeClick;
            set
            {
                if (ReferenceEquals(_closeClick, value))
                    return;

                _closeClick = value;
            }
        }

        private void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs? e)
        {
            if (_subscription == null || _itemsSource == null) //Object disposed
                return;

            if (Looper.MainLooper != Looper.MyLooper())
                return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddItems(e.NewItems, e.NewStartingIndex);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveViews(e.OldStartingIndex, e.OldItems.Count);
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                ReplaceItem((ChipModel)e.NewItems[0], e.NewStartingIndex);
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                MoveItem(e.OldStartingIndex, e.NewStartingIndex);
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ResetItems();
            }
        }

        private void AddItems(IEnumerable newItems, int position)
        {
            if (newItems == null || newItems.Count() == 0)
            {
                return;
            }

            for (var i = 0; i < newItems.Count(); i++)
            {
                AddView(CreateNewChip((ChipModel)newItems.ElementAt(i)), i + position);
            }
        }

        private void MoveItem(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= ChildCount ||
                newIndex < 0 || newIndex >= ChildCount)
            {
                return;
            }

            var view = GetChildAt(oldIndex);
            RemoveViewAt(oldIndex);
            AddView(view, newIndex);
        }

        private void ReplaceItem(ChipModel chipModel, int index)
        {
            if (index >= ChildCount)
            {
                return;
            }

            if (GetChildAt(index) is not Chip chip)
            {
                return;
            }

            chip.ChipText = chipModel.Title;
            chip.Tag = chipModel.Id;
        }

        private void ResetItems()
        {
            RemoveAllViews();
            AddItems(_itemsSource, 0);
        }

        private Chip CreateNewChip(ChipModel chipModel)
        {
            var chip = new Chip(Context);
            chip.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, 24f.ToPx());
            chip.SetHeight(24f.ToPx());
            chip.SetSingleLine(true);
            chip.SetLines(1);
            chip.SetEnsureMinTouchTargetSize(false);
            chip.SetPadding(0, 0, 0, 0);
            chip.SetTextSize(ComplexUnitType.Sp, 12);
            chip.TextStartPadding = 8f.ToPx();
            chip.TextEndPadding = 8f.ToPx();
            chip.ChipStartPadding = 0;
            chip.ChipEndPadding = 0;
            chip.ChipStrokeWidth = 0;
            chip.CloseIconEndPadding = 4f.ToPx();
            chip.Typeface = ResourcesCompat.GetFont(Context, Resource.Font.poppins_semibold);
            chip.ChipCornerRadius = 8f.ToPx();
            chip.ChipBackgroundColor = ColorStateList.ValueOf(new Color(ContextCompat.GetColor(Context, Resource.Color.secondary_text_color)));
            chip.SetTextColor(ColorStateList.ValueOf(new Color(ContextCompat.GetColor(Context, Resource.Color.black))));
            chip.CloseIconVisible = true;
            chip.Clickable = false;
            chip.SetOnCloseIconClickListener(new OnClickListener(OnCloseChip));
            chip.ChipText = chipModel.Title;
            chip.Tag = chipModel.Id;
            return chip;
        }

        private void OnCloseChip(View chip)
        {
            var chipId = (int)chip.Tag;
            if (_closeClick?.CanExecute(chipId) == true)
            {
                _closeClick!.Execute(chipId);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Clean(true);
            base.Dispose(disposing);
        }

        private void Clean(bool disposing)
        {
            if (disposing)
            {
                _subscription?.Dispose();
                _subscription = null;
                _closeClick = null;

                var childCount = this.ChildCount;

                for (int i = 0; i < childCount; i++)
                {
                    if (GetChildAt(i) is Chip chip)
                    {
                        chip.SetOnCloseIconClickListener(null);
                    }
                }
            }
        }
    }
}
