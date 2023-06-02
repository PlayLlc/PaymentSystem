using PayWithPlay.Core.Extensions;

namespace PayWithPlay.Core.Models.Loyalty
{
    public class PurchaseModel
    {
        public DateTime? Date { get; set; }

        public decimal Amount { get; set; }

        public string Number { get; set; }

        public string DisplayAmount => $"${Amount:0.00}";

        public string DisplayDate => Date.DisplayFormat();
    }
}
