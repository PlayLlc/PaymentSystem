using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain;

/// <summary>
///     This partial takes care of the base class implementation and class constructors
/// </summary>
public partial class Item : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly HashSet<Category> _Categories = new();

    /// <summary>
    ///     The locations that carry this item in stock
    /// </summary>
    private readonly Locations _Locations;

    /// <summary>
    ///     Variations of this item, such as 'XSmall', 'Small', 'Medium'. Every time a variation is created a new child Item is
    ///     created
    /// </summary>
    private readonly HashSet<Variation> _Variations = new();

    /// <summary>
    ///     Inventory tracking is handled by these alerts. When an item is below threshold or empty, this object will determine
    ///     whether an alert needs to be sent
    /// </summary>
    private readonly Alerts _Alerts;

    private readonly Price _Price;

    private string _Description;
    private Name _Name;
    private Sku? _Sku;
    private int _Quantity;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Item()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Item(ItemDto value)
    {
        Id = new SimpleStringId(value.Id);
        _MerchantId = new SimpleStringId(value.Id);
        _Name = new Name(value.Name);
        _Price = new Price(value.Price);
        _Quantity = value.Quantity;
        _Description = value.Description;
        _Sku = value.Sku is null ? null : new Sku(value.Sku);
        _Alerts = new Alerts(value.Alerts);
        _Locations = new Locations(value.Locations);
        _Categories = new HashSet<Category>(value.Categories.Select(a => new Category(a)));
        _Variations = new HashSet<Variation>(value.Variations.Select(a => new Variation(a)));
    }

    /// <exception cref="ValueObjectException"></exception>
    public Item(
        string id, string merchantId, Name name, Price price, string description, Alerts alerts, Locations locations, Sku? sku = null, short quantity = 0,
        IEnumerable<Category>? categories = null, IEnumerable<Variation>? variations = null)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _Name = name;
        _Price = price;
        _Quantity = quantity;
        _Description = description;
        _Sku = sku;
        _Alerts = alerts;
        _Locations = locations;
        _Categories = categories?.ToHashSet() ?? new HashSet<Category>();
        _Variations = variations?.ToHashSet() ?? new HashSet<Variation>();
    }

    #endregion

    #region Instance Members

    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task RemoveItem(IRetrieveUsers userService, RemoveItem command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Publish(new ItemRemoved(this, user.GetId()));
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public static Item CreateNewItem(IRetrieveUsers userService, IRetrieveMerchants merchantService, CreateItem command)
    {
        User user = userService.GetById(command.UserId) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = merchantService.GetById(command.UserId) ?? throw new NotFoundException(typeof(User));

        Alerts alerts = new Alerts(GenerateSimpleStringId(), true, 0);
        Locations locations = new Locations(GenerateSimpleStringId(), true);
        Name name = new Name(command.Name);
        Sku? sku = command.Sku is null ? null : new Sku(command.Sku);
        IEnumerable<Category> categories = command.Categories.Select(a => new Category(a));
        Price price = new Price(GenerateSimpleStringId(), command.Price);

        Item item = new Item(GenerateSimpleStringId(), merchant.Id, name, price, command.Description ?? string.Empty, alerts, locations, sku, 0, categories);

        item.Enforce(new MerchantMustBeActiveToCreateAggregate<Item>(merchant));
        item.Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        item.Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(merchant.Id, user));
        item.Publish(new ItemCreated(item, user.Id));

        return item;
    }

    internal SimpleStringId GetMerchantId()
    {
        return _MerchantId;
    }

    internal string GetName()
    {
        return _Name.Value;
    }

    public int GetQuantityInStock()
    {
        return _Quantity;
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override ItemDto AsDto()
    {
        return new ItemDto
        {
            Id = Id,
            Name = _Name,
            Description = _Description,
            Categories = _Categories.Select(a => a.AsDto()),
            MerchantId = _MerchantId,
            Locations = _Locations.AsDto(),
            Price = _Price.AsDto(),
            Quantity = _Quantity,
            Sku = _Sku?.Value
        };
    }

    #endregion
}