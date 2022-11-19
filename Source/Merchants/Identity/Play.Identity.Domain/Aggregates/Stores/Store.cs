using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Domain.ValueObjects;
using Play.Identity.Contracts.Commands.Store;
using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Domain.Aggregates;

public class Store : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly string _MerchantId;
    private readonly Name _StoreName;
    private readonly Address _Address;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // For entity framework only
    private Store()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Store(string id, string merchantId, Name storeName, Address address)
    {
        Id = new SimpleStringId(id);
        _MerchantId = merchantId;
        _StoreName = storeName;
        _Address = address;
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public static Store CreateNewStore(CreateNewStore command)
    {
        // TODO: Create Business Rules to enforce
        Store newStore = new Store(GenerateSimpleStringId(), command.MerchantId, new Name(command.Name), new Address(command.Address));

        // TODO: Publish Domain Event that a new store has been created. This is how we will initialize the ID
        return newStore;
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override IDto AsDto()
    {
        return new StoreDto()
        {
            Id = Id,
            Address = _Address.AsDto(),
            StoreName = _StoreName
        };
    }

    #endregion
}