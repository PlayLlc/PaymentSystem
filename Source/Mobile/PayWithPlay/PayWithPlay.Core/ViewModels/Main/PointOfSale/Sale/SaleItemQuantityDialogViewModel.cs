using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleItemQuantityDialogViewModel : DialogInputViewModel
    {
        public override InputTypeEnum InputType => InputTypeEnum.Numeric;

        public override string Title => Resource.Quantity;
        public override string DoneButtonText => Resource.Add;
        public override string HintText => Resource.Quantity;
    }
}
