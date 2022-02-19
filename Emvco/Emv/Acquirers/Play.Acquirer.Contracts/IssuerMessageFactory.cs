namespace Play.Acquirer.Contracts;

public abstract class IssuerMessageFactory
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
    ///     Creates the <see cref="IssuerMessageRequest" /> needed to communicate with the Issuer
    /// </summary>
    /// <param name="messageCode"></param>
    /// <param name="tagLengthValues"></param>
    /// <returns></returns>
    public abstract IssuerMessageRequest Create(MessageTypeIndicator messageCode, TagLengthValue[] tagLengthValues);

    /// <summary>
    ///     Validates that the <see cref="TagLengthValue" /> objects that are required for this message type are present
    /// </summary>
    /// <param name="tagLengthValues"></param>
    protected abstract void Validate(TagLengthValue[] tagLengthValues);

    #endregion
}