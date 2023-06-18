using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Aggregates.Items.DomainEvents;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Aggregates;

/// <summary>
///     This partial takes care of the base class implementation and class constructors
/// </summary>
public partial class Item : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;
    private readonly HashSet<Category> _Categories = new();


    private MoneyValueObject _Price;
    private Name _Name;

    /// <summary>
    ///     The locations that carry this item in stock
    /// </summary>
    private readonly Locations _Locations;

    /// <summary>
    ///     Inventory tracking is handled by these alerts. When an item is below threshold or empty, this object will determine
    ///     whether an alert needs to be sent
    /// </summary>
    private readonly Alerts _Alerts;

    private string _Description;
    private Sku? _Sku;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Item()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Item(ItemDto value)
    {
        Id = new(value.Id);
        _MerchantId = new(value.Id);
        _Price = new(value.Price);
        _Name = new(value.Name);
        _Description = value.Description;
        _Alerts = new(value.Alerts);
        _Locations = new(value.Locations);
        _Categories = new(value.Categories.Select(a => new Category(a)));
    }

    /// <exception cref="ValueObjectException"></exception>
    public Item(
        string id, 
        string merchantId,
        MoneyValueObject price,
        Name name,
        string description,
        Alerts alerts,
        Locations locations, 
        Sku? sku = null,
        IEnumerable<Category>? categories = null)
    {
        Id = new(id);
        _MerchantId = new(merchantId);
        _Price = price;
        _Name = name;
        _Description = description;
        _Alerts = alerts;
        _Locations = locations;
        _Categories = categories?.ToHashSet() ?? new HashSet<Category>();
    }

    #endregion

    #region Instance Members

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static Item CreateNewItem(IRetrieveUsers userService, IRetrieveMerchants merchantService, CreateItem command)
    {
        User user = userService.GetById(command.UserId) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = merchantService.GetById(command.UserId) ?? throw new NotFoundException(typeof(User));

        Name name = new(command.Name);
        MoneyValueObject price = new(command.Price);

        Alerts alerts = command.Alerts is null ? new(GenerateSimpleStringId(), true, 0) : new(command.Alerts);
        string description = command.Description ?? string.Empty;
        Sku? sku = command.Sku is null ? null : new Sku(command.Sku);

        IEnumerable<Category> categories = command.Categories.Select(a => new Category(a));
        Locations locations = command.Locations is null ? new Locations(GenerateSimpleStringId(), true) : new(command.Locations);
        

        Item item = new(GenerateSimpleStringId(), 
            merchant.Id,
            price, 
            name,
            description, 
            alerts, 
            locations, 
            sku, 
            categories);

        item.Enforce(new MerchantMustBeActiveToCreateAggregate<Item>(merchant));
        item.Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        item.Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(merchant.Id, user));
        item.Publish(new ItemCreated(item, user.Id));

        return item;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task Remove(IRetrieveUsers userService, RemoveItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Publish(new ItemRemoved(this, user.GetId()));
    }
    





    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateSku(IRetrieveUsers userService, UpdateItemSku command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
         
        _Sku = new(command.Sku);

        Publish(new SkuUpdated(this, user.Id, _Sku.Value));
    }

    internal SimpleStringId GetMerchantId() => _MerchantId;

    public override SimpleStringId GetId() => Id;

    public override ItemDto AsDto()
    {
        return new()
        {
            Id = Id,
            Price = _Price,
            Name = _Name,
            Description = _Description,
            Categories = _Categories.Select(a => a.AsDto()),
            MerchantId = _MerchantId,
            Locations = _Locations.AsDto()
        };
    }

    #endregion
}