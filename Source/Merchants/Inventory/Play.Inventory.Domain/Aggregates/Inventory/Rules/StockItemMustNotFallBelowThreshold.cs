using Play.Domain.Aggregates;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain
{
    public class StockItemMustNotFallBelowThreshold : BusinessRule<Inventory, string>
    {
        #region Instance Values

        private readonly bool _IsValid;
        private readonly string _StockItemId;
        private readonly IEnumerable<User> _Subscriptions;
        public override string Message => $"The {nameof(Item)} has fallen below the low inventory threshold";

        #endregion

        #region Constructor

        internal StockItemMustNotFallBelowThreshold(Item item, string stockItemId, int quantity)
        {
            _IsValid = item.IsLowInventoryAlertRequired(quantity, out IEnumerable<User>? subscriptions);
            _StockItemId = stockItemId;
            _Subscriptions = subscriptions ?? new List<User>();
        }

        #endregion

        #region Instance Members

        public override LowInventoryAlert CreateBusinessRuleViolationDomainEvent(Inventory inventory)
        {
            return new LowInventoryAlert(inventory, _StockItemId, _Subscriptions, this);
        }

        public override bool IsBroken()
        {
            return !_IsValid;
        }

        #endregion
    }
}