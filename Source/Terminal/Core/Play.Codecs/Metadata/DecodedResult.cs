namespace Play.Codecs;

// TODO: This is a shitty implementation - you shouldn't have to expose metadata to the client and make them do a magic trick in order to decode something

public record DecodedResult<T> : DecodedMetadata
{
    #region Instance Values

    public T Value { get; }

    #endregion

    #region Constructor

    public DecodedResult(T value, nint charCount) : base(charCount)
    {
        Value = value;
    }

    #endregion
}