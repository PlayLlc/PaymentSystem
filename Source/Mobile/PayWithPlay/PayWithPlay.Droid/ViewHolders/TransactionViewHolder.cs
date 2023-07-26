using Android.Views;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PayWithPlay.Core.Models.PointOfSale;

namespace PayWithPlay.Droid.ViewHolders
{
    public class TransactionViewHolder : MvxRecyclerViewHolder
    {
        private TransactionItemModel? _itemModel => BindingContext!.DataContext as TransactionItemModel;

        public TransactionViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
        }

        public void OnBind()
        {
        }
    }
}
