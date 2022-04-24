namespace Play.Codecs;

public interface IDecodeToMetadata
{
    #region Instance Members

    /// <summary>
    ///     This is for external validation of a sequence and will not throw an exception
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsValid(ReadOnlySpan<byte> value);

    #endregion

    #region Serialization

    public DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion
}