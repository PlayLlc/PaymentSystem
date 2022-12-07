using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Reader;

public interface IReaderTransactionDatabase
{
    #region Instance Members

    /// <summary>
    ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
    ///     zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsPresent(Tag tag);

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="PrimitiveValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    /// <exception cref="TerminalDataException"></exception>
    public bool TryGet(Tag tag, out PrimitiveValue? result);

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="PrimitiveValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    /// <exception cref="TerminalDataException"></exception>
    bool TryGet<T>(Tag tag, out T? result) where T : PrimitiveValue;

    #endregion
}