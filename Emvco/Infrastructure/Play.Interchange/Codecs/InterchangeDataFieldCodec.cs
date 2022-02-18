namespace Play.Interchange.Codecs;

public abstract class InterchangeDataFieldCodec
{
    #region Instance Members

    protected static InterchangeEncodingId GetEncodingId(Type encoder) => new(encoder);

    // A TLV object's Length is a uint. That accounts for one byte for the metadata and 2 subsequent
    // octets for the actual byte count of the Value content
    public abstract ushort GetByteCount<T>(T value) where T : struct;
    public abstract ushort GetByteCount<T>(T[] value) where T : struct;

    /// <summary>
    ///     An method to get the Identifier for an instance of this class
    /// </summary>
    /// <returns></returns>
    public abstract InterchangeEncodingId GetIdentifier();

    /// <summary>
    ///     This is for external validation of a sequence and will not throw an exception
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public abstract bool IsValid(ReadOnlySpan<byte> value);

    /// <summary>
    ///     This method will be used to validate the stream when decoding. If the stream is not
    ///     valid then it will throw an exception as to why
    /// </summary>
    /// <param name="value"></param>
    protected abstract void Validate(ReadOnlySpan<byte> value);

    public abstract void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct;
    public abstract void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct;
    public abstract void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct;
    public abstract void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct;

    #endregion

    #region Serialization

    public abstract DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion
}