using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.Loyalty
{
    public class DiscountModel
    {
        public decimal Value { get; set; }

        public DiscountType DiscountType { get; set; }

        public string DisplayValue => DiscountType == DiscountType.Amount ? $"{Value:0.00}" : $"{Value}% {Resource.Off.ToLower()}";
    }
}
