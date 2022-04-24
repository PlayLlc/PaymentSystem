using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber;

public interface IReadTlvDatabase
{
    #region Instance Members

    /// <summary>
    ///     Gets the <see cref="PrimitiveValue" /> associated with the <see cref="Tag" /> in the arg
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="TerminalDataException">
    ///     This exception gets thrown internally because something was coded or incorrectly configured in our code base. An
    ///     assumption was made that the database value was present when it was not.
    /// </exception>
    /// <exception cref="TerminalDataException"></exception>
    public PrimitiveValue Get(Tag tag);

    /// <summary>
    ///     Gets the <see cref="PrimitiveValue" /> associated with the <see cref="Tag" /> in the arg
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException">
    ///     This exception gets thrown internally because something was coded or incorrectly configured in our code base. An
    ///     assumption was made that the database value was present when it was not.
    /// </exception>
    public T Get<T>(Tag tag) where T : PrimitiveValue;

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
    /// <exception cref="TerminalDataException"></exception>
    public bool IsPresent(Tag tag);

    /// <summary>
    ///     Returns TRUE if:
    ///     • The Database includes a data object with the provided <see cref="Tag" />
    ///     • The length of the data object is non-zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsPresentAndNotEmpty(Tag tag);

    #endregion
}