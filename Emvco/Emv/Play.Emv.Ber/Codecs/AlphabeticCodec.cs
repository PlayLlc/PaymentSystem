using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.Emv.Exceptions;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Strings;

namespace Play.Ber.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class AlphabeticCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly Alphabetic _Alphabetic = PlayEncoding.Alphabetic;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(AlphabeticCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier()
    {
        return Identifier;
    }

    /// <exception cref="EncodingException"></exception>
    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        return _Alphabetic.IsValid(value);
    }

    public override byte[] Encode<T>(T value)
    {
        throw new NotImplementedException();
    }

    public override byte[] Encode<T>(T value, int length)
    {
        throw new NotImplementedException();
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new NotImplementedException();
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new NotImplementedException();
    }

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value)
    {
        return _Alphabetic.GetBytes(value);
    }

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == length)
            return Encode(value);

        if (length > value.Length)
        {
            Span<byte> buffer = stackalloc byte[length];
            _Alphabetic.GetBytes(value).AsSpan().CopyTo(buffer);

            return buffer.ToArray();
        }

        return _Alphabetic.GetBytes(value)[..length];
    }

    public byte[] Encode(string value)
    {
        return Encode(value.AsSpan());
    }

    public override ushort GetByteCount<T>(T value)
    {
        throw new NotImplementedException();
    }

    public override ushort GetByteCount<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new InternalEmvEncodingException("This exception should never be reached");
    }

    /// <exception cref="EmvEncodingFormatException"></exception>
    /// <exception cref="EncodingException"></exception>
    private static void Validate(ReadOnlySpan<char> value)
    {
        if (!_Alphabetic.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    /// <exception cref="EmvEncodingFormatException"></exception>
    /// <exception cref="EncodingException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_Alphabetic.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    #endregion

    #region Serialization

    /// <exception cref="EncodingException"></exception>
    public override DecodedResult<char[]> Decode(ReadOnlySpan<byte> value)
    {
        return new(_Alphabetic.GetChars(value), value.Length);
    }

    #endregion
}