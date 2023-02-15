using Android.Views;
using AndroidX.RecyclerView.Widget;
using PayWithPlay.Android.ViewHolders;
using PayWithPlay.Core.Utils;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace PayWithPlay.Android.Adapters
{
    public abstract class BaseRecyclerAdapter<TModel> : RecyclerView.Adapter, ISafeDisposable
    {
        private ICommand? _itemClick, _itemLongClick;
        private RecyclerView? _recyclerView;
        private List<RecyclerView.ViewHolder> _viewHolders = new();

        private ObservableCollection<TModel>? _items;

        public ObservableCollection<TModel>? Items
        {
            get => _items;
            set
            {
                if (_items != null)
                {
                    _items.CollectionChanged -= CollectionChanged;
                }

                _items = value;

                if (_items != null)
                {
                    _items.CollectionChanged += CollectionChanged;
                }

                NotifyDataSetChanged();
            }
        }

        public ICommand? ItemClick
        {
            get => _itemClick;
            set
            {
                if (!ReferenceEquals(_itemClick, value)) 
                {
                    _itemClick = value;

                }
            }
        }

        public ICommand? ItemLongClick
        {
            get => _itemLongClick;
            set
            {
                if (!ReferenceEquals(_itemLongClick, value))
                {
                    _itemLongClick = value;

                }
            }
        }

        public override int ItemCount => _items == null ? 0 : _items.Count;

        public abstract ViewHolder CreateItemViewHolder(ViewGroup parent, int viewType);

        public override void OnAttachedToRecyclerView(RecyclerView recyclerView)
        {
            base.OnAttachedToRecyclerView(recyclerView);

            _recyclerView = recyclerView;
        }

        public override void OnDetachedFromRecyclerView(RecyclerView recyclerView)
        {
            base.OnDetachedFromRecyclerView(recyclerView);

            SafeDispose();
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            AttachClickListeners(holder);
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var viewHolder = CreateItemViewHolder(parent, viewType);
            _viewHolders.Add(viewHolder);

            return viewHolder;
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            if (holder is ClickableViewHolder<TModel> viewHolder)
            {
                viewHolder.Click -= OnItemViewClick;
                viewHolder.LongClick -= OnItemViewLongClick;
                //viewHolder.OnViewRecycled();
            }
        }

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            if (holder is ClickableViewHolder<TModel> clickableViewHolder)
            {
                clickableViewHolder.OnAttachedToWindow();
            }
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            if(holder is ClickableViewHolder<TModel> clickableHolder)
            {
                clickableHolder.OnDetachedFromWindow();
            }

            base.OnViewDetachedFromWindow(holder);
        }

        public void SafeDispose()
        {
            Items = null;

            _viewHolders.ForEach(viewHolder => (viewHolder as ISafeDisposable)?.SafeDispose());
            _viewHolders.Clear();
            _itemClick = null;
            _itemLongClick = null;

            _recyclerView = null;
        }

        protected virtual void OnItemViewClick(object? sender, EventArgs? e)
        {
            if (sender is ClickableViewHolder<TModel> clickableHolder)
            {
                ExecuteCommandOnItem(ItemClick, clickableHolder.Model);
            }
        }

        protected virtual void OnItemViewLongClick(object? sender, EventArgs? e)
        {
            if (sender is ClickableViewHolder<TModel> clickableHolder)
            {
                ExecuteCommandOnItem(ItemLongClick, clickableHolder.Model);
            }
        }

        protected virtual void ExecuteCommandOnItem(ICommand? command, object? itemDataContext)
        {
            if (command?.CanExecute(itemDataContext) == true && itemDataContext != null)
            {
                command.Execute(itemDataContext);
            }
        }

        private void AttachClickListeners(ViewHolder viewHolder)
        {
            if (viewHolder is ClickableViewHolder<TModel> clickableViewHolder)
            {
                clickableViewHolder.Click -= OnItemViewClick;
                clickableViewHolder.LongClick -= OnItemViewLongClick;
                clickableViewHolder.Click += OnItemViewClick;
                clickableViewHolder.LongClick += OnItemViewLongClick;
            }
        }

        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add when e.NewItems != null:
                    NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Move when e.NewItems != null:
                    for (var i = 0; i < e.NewItems.Count; i++)
                        NotifyItemMoved(e.OldStartingIndex + i, e.NewStartingIndex + i);
                    break;
                case NotifyCollectionChangedAction.Replace when e.NewItems != null:
                    NotifyItemRangeChanged(e.NewStartingIndex, e.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Remove when e.OldItems != null:
                    NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    NotifyDataSetChanged();
                    break;
            }
        }

    }
}