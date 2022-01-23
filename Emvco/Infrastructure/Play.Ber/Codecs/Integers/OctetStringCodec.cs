using System;
using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;

namespace Play.Ber.Codecs;

/// <summary>
/// </summary>
/// <remarks>
///     One octet string is equal to another if they are of the same length and are the same at each
///     octet position.
/// </remarks>
public sealed class OctetStringCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly Hexadecimal _HexadecimalCodec = PlayEncoding.Hexadecimal;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(OctetStringCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => true;

    protected override void Validate(ReadOnlySpan<byte> value)
    {
        // This is always going to be valid
    }

    public override byte[] Encode<T>(T value) => throw new NotImplementedException();
    public override byte[] Encode<T>(T value, int length) => throw new NotImplementedException();

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        {
            Type valueType = typeof(T);

            if (valueType == typeof(byte))
                return Encode(Unsafe.As<T[], byte[]>(ref value), length);
            if (valueType == typeof(char))
                return Encode(Unsafe.As<T[], char[]>(ref value), length);

            throw new
                BerInternalException($"The {nameof(OctetStringCodec)} does not implement the capability to encode the type {typeof(T).FullName}");
        }
    }

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    public override byte[] Encode<T>(T[] value)
    {
        Type valueType = typeof(T);

        if (valueType == typeof(byte))
            return Encode(Unsafe.As<T[], byte[]>(ref value));
        if (valueType == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value));

        throw new
            BerInternalException($"The {nameof(OctetStringCodec)} does not implement the capability to encode the type {typeof(T).FullName}");
    }

    public byte[] Encode(ReadOnlySpan<char> value) => _HexadecimalCodec.GetBytes(value);
    public byte[] Encode(ReadOnlySpan<char> value, int length) => _HexadecimalCodec.GetBytes(value)[..length];
    public byte[] Encode(ReadOnlySpan<byte> value) => value.ToArray();
    public byte[] Encode(ReadOnlySpan<byte> value, int length) => value[..length].ToArray();

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override ushort GetByteCount<T>(T value) => throw new NotImplementedException();

    public override ushort GetByteCount<T>(T[] value) => (ushort) (value.Length * Unsafe.SizeOf<T>());

    #endregion

    #region Serialization

    /// <remarks>
    ///     I don't see why we would need to decode this to a string. That would be really inefficient to
    ///     do that, so hopefully we can get away with byte arrays
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    public override DecodedMetadata Decode(ReadOnlySpan<byte> value) => new DecodedResult<byte[]>(value.ToArray(), value.Length);

    #endregion
}