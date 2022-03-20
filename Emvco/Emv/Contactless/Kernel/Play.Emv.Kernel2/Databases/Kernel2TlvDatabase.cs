using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2TlvDatabase : ITlvDatabase
{
    #region Instance Values

    private readonly SortedDictionary<Tag, PrimitiveValue?> _Database;
    private readonly Kernel2PersistentValues _PersistentValues;

    #endregion

    #region Constructor

    /// <param name="persistentValues">
    ///     persistent values are configuration values that will continue to exist from transaction
    ///     to transaction. These values will not change
    /// </param>
    public Kernel2TlvDatabase(Kernel2PersistentValues persistentValues)
    {
        _Database = new SortedDictionary<Tag, PrimitiveValue?>();
        _PersistentValues = persistentValues;

        SeedDatabase();
    }

    #endregion

    #region Instance Members

    private void SeedDatabase()
    {
        foreach (PrimitiveValue persistentValue in _PersistentValues.GetPersistentValues())
            _Database.Add(persistentValue.GetTag(), persistentValue);
    }

    /// <summary>
    ///     Clears the database of transient values and restores the persistent and default values of the database
    /// </summary>
    public void Clear()
    {
        foreach (KeyValuePair<Tag, PrimitiveValue?> a in _Database)
            _Database[a.Key] = null;

        SeedDatabase();
    }

    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="TerminalDataException">
    ///     This exception gets thrown internally because something was coded or incorrectly configured in our code base. An
    ///     assumption was made that the database value was present when it was not.
    /// </exception>
    /// <exception cref="TerminalDataException"></exception>
    public PrimitiveValue Get(Tag tag)
    {
        if (!_Database.TryGetValue(tag, out PrimitiveValue? result))
            throw new TerminalDataException($"The argument {nameof(tag)} provided does not exist in {nameof(Kernel2Database)}");

        return result!;
    }

    /// <summary>
    ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
    /// </summary>
    /// <param name="tag"></param>
    public bool IsKnown(Tag tag) => KnownObjects.Exists(tag);

    /// <summary>
    ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
    ///     zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool IsPresent(Tag tag) => _Database.ContainsKey(tag);

    /// <summary>
    ///     Returns TRUE if:
    ///     • The Database includes a data object with the provided <see cref="Tag" />
    ///     • The length of the data object is non-zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool IsPresentAndNotEmpty(Tag tag) => IsPresent(tag) && (_Database[tag] != null);

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="PrimitiveValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    public bool TryGet(Tag tag, out PrimitiveValue? result)
    {
        if (!_Database.TryGetValue(tag, out PrimitiveValue? databaseValue))
        {
            result = null;

            return false;
        }

        result = databaseValue;

        return true;
    }

    /// <summary>
    ///     Updates the database if it is a recognized object and discards the value if it is not recognized
    /// </summary>
    /// <param name="value"></param>
    public void Update(PrimitiveValue value)
    {
        if (!IsKnown(value.GetTag()))
            return;

        if (_Database.ContainsKey(value.GetTag()))
            _Database[value.GetTag()] = value;
        else
            _Database.Add(value.GetTag(), value);
    }

    /// <summary>
    ///     Updates the the database with all recognized
    ///     <param name="values" />
    ///     provided it is not recognized
    /// </summary>
    /// <param name="values"></param>
    public void UpdateRange(PrimitiveValue[] values)
    {
        for (int i = 0; i < values.Length; i++)
            Update(values[i]);
    }

    /// <summary>
    ///     Initializes the data object specified with a zero length. After initialization the data object is present in the
    ///     TLV Database.
    /// </summary>
    /// <param name="tag"></param>
    public void Initialize(Tag tag)
    {
        if (!IsKnown(tag))
            return;

        _Database.Add(tag, null);
    }

    public void Initialize(params Tag[] tag)
    {
        for (int i = 0; i < tag.Length; i++)
        {
            if (!IsKnown(tag[i]))
                continue;

            Initialize(tag[i]);
        }
    }

    #endregion
}