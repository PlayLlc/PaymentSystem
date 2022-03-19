using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Emv.Ber;

public interface IWriteTlvDatabase
{
    /// <summary>
    ///     Updates the database with the
    ///     <param name="value"></param>
    ///     if it is a recognized object and discards the value
    ///     if it is not recognized
    /// </summary>
    /// <param name="value"></param>
    public void Update(PrimitiveValue value);

    /// <summary>
    ///     Updates the the database with all recognized
    ///     <param name="values" />
    ///     provided it is not recognized
    /// </summary>
    /// <param name="values"></param>
    public void UpdateRange(PrimitiveValue[] values);

    /// <summary>
    ///     Initializes the data object specified by <see cref="tag" /> with a zero length. After initialization the data
    ///     object is present in the TLV Database.
    /// </summary>
    /// <param name="tag"></param>
    public void Initialize(Tag tag);
}