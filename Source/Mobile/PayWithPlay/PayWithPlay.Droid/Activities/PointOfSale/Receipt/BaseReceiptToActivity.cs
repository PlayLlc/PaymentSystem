using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.Activities.PointOfSale.Receipt
{
    public class BaseReceiptToActivity<T> : BaseActivity<T> where T : BaseReceiptToViewModel
    {
        protected override int LayoutId => Resource.Layout.activity_send_receipt_to;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var inputEt = FindViewById<EditTextWithValidation>(Resource.Id.inputEt)!;
            ViewModel!.SetInputValidator(inputEt);
        }
    }
}
