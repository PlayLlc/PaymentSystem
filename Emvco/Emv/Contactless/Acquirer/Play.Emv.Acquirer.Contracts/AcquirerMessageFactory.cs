using Play.Ber.DataObjects;
using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Interchange.ValueTypes;

namespace Play.Emv.Acquirer.Contracts;

public abstract class AcquirerMessageFactory
{
    #region Instance Members

    /// <summary>
    ///     Creates a <see cref="DataNeeded" /> object so the terminal can query the Kernel for the
    ///     <see cref="TagLengthValue" />  objects that are required for this message
    /// </summary>
    /// <param name="messageCode"></param>
    /// <returns></returns>
    public abstract DataNeeded GetDataNeeded(MessageTypeIndicator messageCode);

    /// <summary>
    ///     Creates the <see cref="AcquirerRequestSignal" /> needed to communicate with the Issuer
    /// </summary>
    /// <param name="messageCode"></param>
    /// <param name="tagLengthValues"></param>
    /// <returns></returns>
    public abstract AcquirerRequestSignal Create(MessageTypeIndicator messageCode, TagLengthValue[] tagLengthValues);

    /// <summary>
    ///     Validates that the <see cref="TagLengthValue" /> objects that are required for this message type are present
    /// </summary>
    /// <param name="tagLengthValues"></param>
    protected abstract void Validate(TagLengthValue[] tagLengthValues);

    #endregion
}