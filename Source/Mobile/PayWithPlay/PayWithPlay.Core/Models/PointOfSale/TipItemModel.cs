using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.PointOfSale
{
    public class TipItemModel
    {
        public TipType Type { get; set; }

        public int PercentageValue { get; set; }
        public string PercentageDisplayValue => Type == TipType.Custom ? Resource.Custom : $"{PercentageValue}%";

        public string? TipValue { get; set; }
    }
}
