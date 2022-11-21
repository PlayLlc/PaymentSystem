using Play.Core;
using Play.Domain;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;
using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entitiessss;

public class PercentageOff : Entity<SimpleStringId>
{
    #region Instance Values

    private Probability _Percentage;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private PercentageOff()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal PercentageOff(string id, byte percentage)
    {
        Id = new SimpleStringId(id);
        _Percentage = new Probability(percentage);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal PercentageOff(PercentageOffDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _Percentage = new Probability(dto.Percentage);
    }

    #endregion

    #region Instance Members

    public void UpdatePercentage(byte percentage)
    {
        _Percentage = new Probability(percentage);
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override PercentageOffDto AsDto()
    {
        return new PercentageOffDto()
        {
            Id = Id,
            Percentage = (byte) _Percentage
        };
    }

    #endregion
}