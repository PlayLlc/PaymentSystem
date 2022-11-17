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
        _Stores = new HashSet<Store>();
    }

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    internal Locations(string id, bool allLocations, params string[] storeIds)
    {
        Id = new SimpleStringId(id);
        _AllLocations = allLocations;
        _Stores = new HashSet<Store>(storeIds.Select(a => new Store(new SimpleStringId(a))));
    }

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    internal Locations(LocationsDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _AllLocations = dto.AllLocations;
        _Stores = new HashSet<Store>(dto.StoreIds.Select(a => new Store(new SimpleStringId(a))));
    }

    #endregion

    #region Instance Members

    internal bool IsAllLocationsSet()
    {
        return _AllLocations;
    }

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
            locationsAdded += _Stores.Add(new Store(id)) ? 1 : 0;

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
            locationsRemoved += _Stores.Remove(new Store(id)) ? 1 : 0;

        return locationsRemoved;
    }

    public void SetAllLocations()
    {
        _AllLocations = true;

        _Stores.Clear();
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override LocationsDto AsDto()
    {
        return new LocationsDto()
        {
            Id = Id,
            AllLocations = _AllLocations,
            StoreIds = _Stores.Select(a => a.Id).AsEnumerable()
        };
    }

    #endregion
}