using System.Runtime.CompilerServices;

using Exceptions;
using Exceptions;
using Exceptions;
using Exceptions;
using Exceptions;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Strings;

using PlayEncodingId = Play.Ber.Codecs.PlayEncodingId;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class AlphaNumericSpecialCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly AlphaNumericSpecial _AlphanumericSpecial = PlayEncoding.AlphaNumericSpecial;
    public static readonly PlayEncodingId Identifier = GetEncodingId(typeof(AlphaNumericSpecialCodec));

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => _AlphanumericSpecial.IsValid(value);

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_AlphanumericSpecial.IsValid(value))
            throw new EncodingException(EncodingException.CharacterArrayContainsInvalidValue + " - The offending value was value[i]");
    }

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new NotImplementedException();
    }

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new NotImplementedException();
    }

    public override byte[] Encode<T>(T value) => throw new NotImplementedException();
    public override byte[] Encode<T>(T value, int length) => throw new NotImplementedException();

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value) => _AlphanumericSpecial.GetBytes(value);

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == length)
            return Encode(value);

        if (length > value.Length)
        {
            Span<byte> buffer = stackalloc byte[length];
            _AlphanumericSpecial.GetBytes(value).AsSpan().CopyTo(buffer);

            return buffer.ToArray();
        }

        return _AlphanumericSpecial.GetBytes(value)[..length];
    }

    public byte[] Encode(string value) => _AlphanumericSpecial.GetBytes(value);
    public override ushort GetByteCount<T>(T value) => throw new NotImplementedException();

    public override ushort GetByteCount<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new NotImplementedException();
    }

    #endregion

    #region Serialization

    public override DecodedResult<char[]> Decode(ReadOnlySpan<byte> value)
    {
        char[] valueResult = _AlphanumericSpecial.GetChars(value);

        return new DecodedResult<char[]>(valueResult, valueResult.Length);
    }

    #endregion
}