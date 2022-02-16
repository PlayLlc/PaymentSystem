using System;
using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Strings;

namespace Play.Ber.Codecs;

public sealed class IA5StringCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly StrictAscii _StrictAsciiCodec = PlayEncoding.ASCII;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(IA5StringCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => _StrictAsciiCodec.IsValid(value);

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_StrictAsciiCodec.IsValid(value))
            throw new BerFormatException(new ArgumentOutOfRangeException(nameof(value)));
    }

    // TODO: this is only in BER library so holding off on implementing this
    public override byte[] Encode<T>(T[] value, int length) => throw new NotImplementedException();
    public override byte[] Encode<T>(T value) => throw new NotImplementedException();
    public override byte[] Encode<T>(T value, int length) => throw new NotImplementedException();

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) != typeof(char))
            throw new BerInternalException($"The {nameof(IA5StringCodec)} could not handle encoding a type of {typeof(T).FullName}");

        return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value)
    {
        try
        {
            return _StrictAsciiCodec.GetBytes(value);
        }
        catch (EncodingException exception)
        {
            throw new BerFormatException(
                new ArgumentOutOfRangeException("The argument was out of range of acceptable values. The value could not be encoded",
                    exception));
        }
    }

    public override ushort GetByteCount<T>(T value) => throw new NotImplementedException();

    public override ushort GetByteCount<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return (ushort) value.Length;

        throw new NotImplementedException();
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="EncodingException"></exception>
    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        try
        {
            char[]? result = _StrictAsciiCodec.GetChars(value);

            return new DecodedResult<char[]>(_StrictAsciiCodec.GetChars(value), result.Length);
        }
        catch (EncodingException exception)
        {
            throw new BerFormatException("The argument was out of range of acceptable values. The value could not be decoded", exception);
        }
    }

    #endregion
}