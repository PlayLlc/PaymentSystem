using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PayWithPlay.Droid.ViewHolders;

namespace PayWithPlay.Droid.Adapters
{
    public class TransactionsAdapter : MvxRecyclerAdapter
    {
        public TransactionsAdapter(IMvxAndroidBindingContext? bindingContext) : base(bindingContext)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);
            ((TransactionViewHolder)holder).OnBind();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext!.LayoutInflaterHolder);
            var view = InflateViewForHolder(parent, viewType, itemBindingContext);

            return new TransactionViewHolder(view, itemBindingContext);
        }
    }
}
