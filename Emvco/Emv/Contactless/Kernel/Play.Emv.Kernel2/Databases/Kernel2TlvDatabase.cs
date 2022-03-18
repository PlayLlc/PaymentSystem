using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Kernel.Databases.Tlv;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2TlvDatabase : ITlvDatabase
{
    #region Instance Values

    private readonly SortedDictionary<Tag, DatabaseValue?> _Database;
    private readonly Kernel2PersistentValues _PersistentValues;

    #endregion

    #region Constructor

    /// <param name="persistentValues">
    ///     persistent values are configuration values that will continue to exist from transaction
    ///     to transaction. These values will not change
    /// </param>
    /// <param name="databaseSeeder">
    ///     Seeds the database with default values for any required configuration value that is not
    ///     provided
    /// </param>
    public Kernel2TlvDatabase(Kernel2PersistentValues persistentValues)
    {
        _Database = new SortedDictionary<Tag, DatabaseValue?>();
        _PersistentValues = persistentValues;

        foreach (KnownObjects knownObject in KnownObjects.GetEnumerator())
            _Database.Add(knownObject, null);

        SeedDatabase();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Clears the database of transient values and restores the persistent and default values of the database
    /// </summary>
    public void Clear()
    {
        foreach (KeyValuePair<Tag, DatabaseValue?> a in _Database)
            _Database[a.Key] = null;

        SeedDatabase();
    }

    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public TagLengthValue Get(Tag tag)
    {
        if (!_Database.TryGetValue(tag, out DatabaseValue? result))
            throw new InvalidOperationException($"The argument {nameof(tag)} provided does not exist in {nameof(Kernel2Database)}");

        return result!;
    }

    /// <summary>
    ///     Proprietary tags not listed in <see cref="KnownObjects" /> may be present in the database at the time of
    ///     Database Initialization if their values are non-zero
    /// </summary>
    /// <param name="values"></param>
    public void Initialize(TagLengthValue[] values)
    {
        foreach (TagLengthValue? value in values)
        {
            if (value.GetLength() == 0)
                continue;

            if (!_Database.ContainsKey(value.GetTag()))
            {
                _Database.Add(value.GetTag(), new DatabaseValue(value));

                continue;
            }

            _Database[value.GetTag()] = new DatabaseValue(value);
        }
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
    public bool IsPresentAndNotEmpty(Tag tag) => IsPresent(tag) && _Database[tag]!.IsNull();

    private void SeedDatabase()
    {
        foreach (Tag tag in _Database.Keys)
            _Database[tag] = null;

        foreach (DatabaseValue? persistentValue in _PersistentValues.GetPersistentValues())
            _Database[persistentValue.GetTag()] = persistentValue;
    }

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="DatabaseValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    public bool TryGet(Tag tag, out TagLengthValue? result)
    {
        if (!_Database.TryGetValue(tag, out DatabaseValue? databaseValue))
        {
            result = null;

            return false;
        }

        result = databaseValue;

        return true;
    }

    /// <summary>
    ///     Updates the database with the
    ///     <param name="value"></param>
    ///     if it is a recognized object and discards the value if
    ///     it is not recognized
    /// </summary>
    /// <param name="value"></param>
    public void Update(TagLengthValue value)
    {
        if (!IsKnown(value.GetTag()))
            return;

        if (!_Database.ContainsKey(value.GetTag()))
            _Database.Add(value.GetTag(), new DatabaseValue(value));

        _Database[value.GetTag()] = new DatabaseValue(value);
    }

    /// <summary>
    ///     Updates the the database with all recognized
    ///     <param name="values" />
    ///     provided it is not recognized
    /// </summary>
    /// <param name="values"></param>
    public void UpdateRange(TagLengthValue[] values)
    {
        for (int i = 0; i < values.Length; i++)
            Update(values[i]);
    }

    /// <summary>
    ///     Initializes the data object specified by <see cref="tag" /> with a zero length. After initialization the data
    ///     object is present in the TLV Database.
    /// </summary>
    /// <param name="tag"></param>
    public void Initialize(Tag tag)
    {
        if (!IsKnown(tag))
            return;

        Update(new DatabaseValue(new TagLengthValue(tag, Array.Empty<byte>())));
    }

    #endregion
}