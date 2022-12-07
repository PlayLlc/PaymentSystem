namespace Play.Codecs;

public record DecodedMetadata
{
    #region Instance Values

    public nint CharCount { get; init; }

    #endregion

    #region Constructor

    public DecodedMetadata(nint charCount)
    {
        CharCount = charCount;
    }

    #endregion
}