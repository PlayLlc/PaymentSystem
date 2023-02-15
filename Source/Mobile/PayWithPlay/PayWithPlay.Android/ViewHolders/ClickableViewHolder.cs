using Android.Views;
using AndroidX.RecyclerView.Widget;
using CommunityToolkit.Mvvm.Bindings.Platforms.Android;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Android.ViewHolders
{
    public class ClickableViewHolder<T> : RecyclerView.ViewHolder, ISafeDisposable
    {
        private IDisposable? _itemViewClickSubscription, _itemViewLongClickSubscription;

        public ClickableViewHolder(View itemView) : base(itemView)
        {
        }

        public event EventHandler<EventArgs>? Click;

        public event EventHandler<EventArgs>? LongClick;

        public T? Model { get; set; }

        public virtual void SafeDispose()
        {
            _itemViewClickSubscription?.Dispose();
            _itemViewClickSubscription = null;
            _itemViewLongClickSubscription?.Dispose();
            _itemViewLongClickSubscription = null;
        }

        public virtual void OnAttachedToWindow()
        {
            _itemViewClickSubscription ??= ItemView.WeakSubscribe(nameof(View.Click), OnItemViewClick);

            _itemViewLongClickSubscription ??= ItemView.WeakSubscribe<View, View.LongClickEventArgs>(nameof(View.LongClick), OnItemViewLongClick);
        }

        public virtual void OnDetachedFromWindow()
        {
            _itemViewClickSubscription?.Dispose();
            _itemViewClickSubscription = null;
            _itemViewLongClickSubscription?.Dispose();
            _itemViewLongClickSubscription = null;
        }

        protected virtual void OnItemViewClick(object? sender, EventArgs e)
        {
            Click?.Invoke(this, e);
        }

        protected virtual void OnItemViewLongClick(object? sender, EventArgs e)
        {
            LongClick?.Invoke(this, e);
        }

        protected override void Dispose(bool disposing)
        {
            _itemViewClickSubscription?.Dispose();
            _itemViewClickSubscription = null;
            _itemViewLongClickSubscription?.Dispose();
            _itemViewLongClickSubscription = null;

            base.Dispose(disposing);
        }
    }
}