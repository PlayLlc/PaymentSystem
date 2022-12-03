using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Entities;

public class Locations : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly HashSet<Store> _Stores;
    private bool _AllLocations;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Locations()
    {
        _Stores = new();
    }

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    internal Locations(string id, bool allLocations, params string[] storeIds)
    {
        Id = new(id);
        _AllLocations = allLocations;
        _Stores = new(storeIds.Select(a => new Store(new SimpleStringId(a))));
    }

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    internal Locations(LocationsDto dto)
    {
        Id = new(dto.Id);
        _AllLocations = dto.AllLocations;
        _Stores = new(dto.StoreIds.Select(a => new Store(new SimpleStringId(a))));
    }

    #endregion

    #region Instance Members

    public bool DoesLocationExist(string storeId)
    {
        return _Stores.Any(a => a.Id == new SimpleStringId(storeId));
    }

    internal bool IsAllLocationsSet() => _AllLocations;

    /// <summary>
    /// </summary>
    /// <param name="storeIds"></param>
    /// <returns>The number of locations added</returns>
    internal int AddLocations(IEnumerable<SimpleStringId> storeIds)
    {
        if (_AllLocations)
            _AllLocations = false;

        int locationsAdded = 0;
        foreach (SimpleStringId id in storeIds)
            locationsAdded += _Stores.Add(new(id)) ? 1 : 0;

        return locationsAdded;
    }

    /// <param name="storeIds"></param>
    /// <returns>
    ///     <returns>The number of locations added</returns>
    /// </returns>
    /// <exception cref="BusinessRuleValidationException"></exception>
    internal int RemoveLocations(IEnumerable<SimpleStringId> storeIds)
    {
        if (_AllLocations)
            throw new BusinessRuleValidationException(
                $"The {nameof(Locations)} object cannot {nameof(RemoveLocations)} because there are no specific stores specified");

        int locationsRemoved = 0;

        foreach (SimpleStringId id in storeIds)
            locationsRemoved += _Stores.Remove(new(id)) ? 1 : 0;

        return locationsRemoved;
    }

    public void SetAllLocations()
    {
        _AllLocations = true;

        _Stores.Clear();
    }

    public override SimpleStringId GetId() => Id;

    public override LocationsDto AsDto()
    {
        return new()
        {
            Id = Id,
            AllLocations = _AllLocations,
            StoreIds = _Stores.Select(a => a.Id).AsEnumerable()
        };
    }

    #endregion
}