namespace Play.Codecs;

public interface IEncodeStructs
{
    public ushort GetByteCount<_T>(_T value) where _T : struct;
    public ushort GetByteCount<_T>(_T[] value) where _T : struct;

    /// <summary>
    ///     Encodes the Value content according to this implementation's encoding rules
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content that is capable of being encoded by this implementation
    /// </typeparam>
    /// <param name="value">
    ///     The Value content sequence to be encoded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public byte[] Encode<_T>(_T value) where _T : struct;

    /// <summary>
    ///     Encodes the Value content according to this implementation's encoding rules
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content that is capable of being encoded by this implementation
    /// </typeparam>
    /// <param name="value">
    ///     The Value content to be encoded
    /// </param>
    /// <param name="length">
    ///     The length in bytes that the encoded result will return. If the length is smaller than the value provided
    ///     then the result will be truncated. If the length is larger then the result will be padded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public byte[] Encode<_T>(_T value, int length) where _T : struct;

    /// <summary>
    ///     Encodes the Value content according to this implementation's encoding rules
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content that is capable of being encoded by this implementation
    /// </typeparam>
    /// <param name="value">
    ///     The Value content sequence to be encoded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public byte[] Encode<_T>(_T[] value) where _T : struct;

    /// <summary>
    ///     Encodes the Value content according to this implementation's encoding rules
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content that is capable of being encoded by this implementation
    /// </typeparam>
    /// <param name="value">
    ///     The Value content sequence to be encoded
    /// </param>
    /// <param name="length">
    ///     The length in bytes that the encoded result will return. If the length is smaller than the value provided
    ///     then the result will be truncated. If the length is larger then the result will be padded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public byte[] Encode<_T>(_T[] value, int length) where _T : struct;
}