using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.Loyalty
{
    public class DiscountModel
    {
        public int Value { get; set; }

        public string DisplayValue => $"{Value}% {Resource.Off.ToLower()}";
    }
}
