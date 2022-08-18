using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber;

public interface IWriteTlvDatabase
{
    #region Instance Members

    /// <summary>
    ///     Updates the database with the
    ///     <param name="value"></param>
    ///     if it is a recognized object and discards the value if it is not recognized
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(PrimitiveValue value);

    /// <summary>
    ///     Updates the the database with all recognized
    ///     <param name="values" />
    ///     provided it is not recognized
    /// </summary>
    /// <param name="values"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(PrimitiveValue[] values);

    /// <summary>
    ///     Initializes the data object specified by <see cref="tag" /> with a zero length. After initialization the data
    ///     object is present in the TLV Database.
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Initialize(Tag tag);

    /// <exception cref="TerminalDataException"></exception>
    void Update(TerminalVerificationResult value);

    /// <summary>
    ///     Initialize
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    public void Initialize(params Tag[] tag);

    #endregion
}