using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleCustomTipDialogViewModel : DialogInputViewModel
    {
        public override InputTypeEnum InputType => InputTypeEnum.Decimal;
        public override string Title => Resource.CustomAmount;
        public override string HintText => "0";
        public override string DoneButtonText => Resource.Confirm;
        public override string Prefix => "$";
    }
}
