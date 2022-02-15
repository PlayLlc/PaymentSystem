using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataExchange;

public interface ITlvDatabase : IQueryTlvDatabase
{
    #region Instance Members

    /// <summary>
    ///     Clears the database of transient values and restores the persistent and default values of the database
    /// </summary>
    public void Clear();

    /// <summary>
    ///     Updates the database with the
    ///     <param name="value"></param>
    ///     if it is a recognized object and discards the value
    ///     if it is not recognized
    /// </summary>
    /// <param name="value"></param>
    public void Update(TagLengthValue value);

    /// <summary>
    ///     Updates the the database with all recognized
    ///     <param name="values" />
    ///     provided it is not recognized
    /// </summary>
    /// <param name="values"></param>
    public void UpdateRange(TagLengthValue[] values);

    /// <summary>
    ///     Initializes the data object specified by <see cref="tag" /> with a zero length. After initialization the data
    ///     object is present in the TLV Database.
    /// </summary>
    /// <param name="tag"></param>
    public void Initialize(Tag tag);

    #endregion
}