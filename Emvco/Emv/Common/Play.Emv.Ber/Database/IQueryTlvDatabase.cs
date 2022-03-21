using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Emv.Ber;

public interface IQueryTlvDatabase
{
    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public PrimitiveValue Get(Tag tag);

    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public T Get<T>(Tag tag) where T : PrimitiveValue;

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="PrimitiveValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    public bool TryGet(Tag tag, out PrimitiveValue? result);

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="PrimitiveValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    bool TryGet<T>(Tag tag, out T? result) where T : PrimitiveValue;

    /// <summary>
    ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
    /// </summary>
    /// <param name="tag"></param>
    public bool IsKnown(Tag tag);

    /// <summary>
    ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
    ///     zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool IsPresent(Tag tag);

    /// <summary>
    ///     Returns TRUE if:
    ///     • The Database includes a data object with the provided <see cref="Tag" />
    ///     • The length of the data object is non-zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool IsPresentAndNotEmpty(Tag tag);
}