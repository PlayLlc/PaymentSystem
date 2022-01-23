using System;

using Play.Ber.DataObjects;
using Play.Ber.InternalFactories;

namespace Play.Ber.Codecs;

public abstract class BerPrimitiveCodec
{
    #region Instance Members

    protected static BerEncodingId GetBerEncodingId(Type encoder) => new(encoder);

    // A TLV object's Length is a uint. That accounts for one byte for the metadata and 2 subsequent
    // octets for the actual byte count of the Value content
    public abstract ushort GetByteCount<T>(T value) where T : struct;
    public abstract ushort GetByteCount<T>(T[] value) where T : struct;

    /// <summary>
    ///     An method to get the Identifier for an instance of this class
    /// </summary>
    /// <returns></returns>
    public abstract BerEncodingId GetIdentifier();

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

    /// <summary>
    ///     Encodes the Value content from a <see cref="PrimitiveValue" />
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content <see cref="Type" /> for a <see cref="PrimitiveValue" />. The type must be unmanaged
    /// </typeparam>
    /// <param name="value">
    ///     The Value content sequence to be encoded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public abstract byte[] Encode<T>(T value) where T : struct;

    /// <summary>
    ///     Encodes the Value content from a <see cref="PrimitiveValue" />
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content <see cref="Type" /> for a <see cref="PrimitiveValue" />. The type must be unmanaged
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
    public abstract byte[] Encode<T>(T value, int length) where T : struct;

    /// <summary>
    ///     Encodes the Value content from a <see cref="PrimitiveValue" />
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content <see cref="Type" /> for a <see cref="PrimitiveValue" />. The type must be unmanaged
    /// </typeparam>
    /// <param name="value">
    ///     The Value content sequence to be encoded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public abstract byte[] Encode<T>(T[] value) where T : struct;

    /// <summary>
    ///     Encodes the Value content from a <see cref="PrimitiveValue" />
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content <see cref="Type" /> for a <see cref="PrimitiveValue" />. The type must be unmanaged
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
    public abstract byte[] Encode<T>(T[] value, int length) where T : struct;

    #endregion

    #region Serialization

    public abstract DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion
}