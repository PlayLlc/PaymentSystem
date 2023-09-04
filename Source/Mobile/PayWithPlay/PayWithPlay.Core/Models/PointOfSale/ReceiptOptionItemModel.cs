using PayWithPlay.Core.Enums;

namespace PayWithPlay.Core.Models.PointOfSale
{
    public class ReceiptOptionItemModel
    {
        public string? Title { get; set; }

        public ReceiptOptionType Type { get; set; }
    }
}
