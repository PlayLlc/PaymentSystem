using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Globalization.Currency;
using Play.Domain.Common.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entities
{
    public class InventoryItem : Entity<SimpleStringId>
    {
        #region Instance Values

        private readonly SimpleStringId _ItemId;
        private readonly MoneyValueObject _Price;

        public override SimpleStringId Id { get; }

        #endregion

        #region Constructor

        // Constructor for Entity Framework
        private InventoryItem()
        { }

        /// <exception cref="ValueObjectException"></exception>
        public InventoryItem(VariationDto dto)
        {
            Id = new SimpleStringId(dto.Id);
            _ItemId = new SimpleStringId(dto.ItemId);
            _Price = dto.Price.AsMoney();
        }

        /// <exception cref="ValueObjectException"></exception>
        public InventoryItem(string id, string itemId, Money price)
        {
            Id = new SimpleStringId(id);
            _ItemId = new SimpleStringId(itemId);
            _Price = price;
        }

        #endregion

        #region Instance Members

        internal bool IsLowerThanInventoryPrice(Money price) => price < _Price.Value;

        public override SimpleStringId GetId() => Id;

        public override VariationDto AsDto() =>
            new()
            {
                Id = Id,
                ItemId = _ItemId,
                Price = _Price.AsDto()
            };

        #endregion
    }
}