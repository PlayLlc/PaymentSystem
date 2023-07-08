namespace PayWithPlay.Core.Models.Home
{
    public class SellerModel
    {
        public string? SellerName { get; set; }

        public string? StoreName { get; set; }

        public decimal? Amount { get; set; }

        public decimal PrevAmount { get; set; }

        public string DisplayedAmount => $"${Amount}";

        public string DisplayedPrevAmount => $"{(PrevIsNegative ? "-" : "+")}${Math.Abs(PrevAmount)}";

        public bool PrevIsNegative => PrevAmount < 0;

        public bool Active { get; set; }
    }
}
