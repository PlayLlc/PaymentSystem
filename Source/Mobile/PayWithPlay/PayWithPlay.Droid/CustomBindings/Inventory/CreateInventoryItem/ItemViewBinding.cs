using Android.Views;
using MvvmCross.Platforms.Android.Binding.Target;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.CustomBindings.Inventory.CreateInventoryItem
{
    public class ItemViewBinding : MvxAndroidTargetBinding<View, ItemModel>
    {
        public const string Property = "CreateInventoryItemView";

        public ItemViewBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, ItemModel viewModel)
        {
            if (target == null || viewModel == null)
            {
                return;
            }

            Application.SynchronizationContext.Post(_ =>
            {
                var name = target.FindViewById<EditTextWithValidation>(Resource.Id.nameEt);
                var price = target.FindViewById<EditTextWithValidation>(Resource.Id.priceEt);

                viewModel.SetInputValidators(name, price);
            }, null);
        }
    }
}
