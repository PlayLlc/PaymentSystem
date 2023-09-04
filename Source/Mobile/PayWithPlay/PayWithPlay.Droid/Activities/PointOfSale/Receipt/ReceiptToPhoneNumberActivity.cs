using Android.Content.PM;
using Android.Text;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.Activities.PointOfSale.Receipt
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class ReceiptToPhoneNumberActivity : BaseReceiptToActivity<ReceiptToPhoneNumberViewModel>
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var inputEt = FindViewById<EditTextWithValidation>(Resource.Id.inputEt)!;
            inputEt.InputType |= InputTypes.ClassPhone;
        }
    }
}
