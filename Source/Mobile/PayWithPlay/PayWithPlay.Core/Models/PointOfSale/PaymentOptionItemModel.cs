using PayWithPlay.Core.Enums;

namespace PayWithPlay.Core.Models.PointOfSale
{
    public class PaymentOptionItemModel
    {
        public string? Title { get; set; }

        public PaymentOptionType Type { get; set; }
    }
}
