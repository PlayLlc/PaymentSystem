using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Entities
{
    /// <summary>
    ///     A <see cref="StockItem" /> is the record of the <see cref="Item" /> quantity that a given Store currently has
    /// </summary>
    public class StockItem : Entity<SimpleStringId>
    {
        #region Instance Values

        public readonly SimpleStringId ItemId;
        public readonly SimpleStringId VariationId;
        private int _Quantity;

        public override SimpleStringId Id { get; }

        #endregion

        #region Constructor

        /// <exception cref="ValueObjectException"></exception>
        public StockItem(StockItemDto dto)
        {
            Id = new SimpleStringId(dto.Id);
        }

        /// <exception cref="ValueObjectException"></exception>
        public StockItem(string id, string itemId, string variationId, int quantity)
        {
            Id = new SimpleStringId(id);
            ItemId = new SimpleStringId(itemId);
            VariationId = new SimpleStringId(variationId);
            _Quantity = quantity;
        }

        // Constructor for Entity Framework
        private StockItem()
        { }

        #endregion

        #region Instance Members

        public string GetItemId()
        {
            return ItemId;
        }

        internal void AddQuantity(ushort quantity)
        {
            _Quantity += quantity;
        }

        internal void RemoveQuantity(ushort quantity)
        {
            _Quantity -= quantity;
        }

        public override SimpleStringId GetId()
        {
            return Id;
        }

        public override StockItemDto AsDto()
        {
            return new StockItemDto()
            {
                Id = Id,
                ItemId = ItemId,
                VariationId = VariationId,
                Quantity = _Quantity
            };
        }

        #endregion
    }
}