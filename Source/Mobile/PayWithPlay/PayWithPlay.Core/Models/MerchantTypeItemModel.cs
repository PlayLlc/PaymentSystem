using PayWithPlay.Core.Enums;

namespace PayWithPlay.Core.Models
{
    public class MerchantTypeItemModel
    {
        public MerchantType Type { get; set; }

        public string? Title { get; set; }

        public string? Subtitle { get; set; }
    }
}