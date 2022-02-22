using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Strings;
using Play.Emv.Ber.Exceptions;

using PlayEncodingId = Play.Ber.Codecs.PlayEncodingId;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class AlphabeticBerCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly AlphabeticCodec _AlphabeticBer = PlayCodec.AlphabeticCodec;
    public static readonly PlayEncodingId Identifier = ()BerEncodingIdType.AlphabeticCodec;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => Identifier;

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public override bool IsValid(ReadOnlySpan<byte> value) => _AlphabeticBer.IsValid(value);

    public override byte[] Encode<T>(T value) => throw new NotImplementedException();
    public override byte[] Encode<T>(T value, int length) => throw new NotImplementedException();

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

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value) => _AlphabeticBer.GetBytes(value);

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == length)
            return Encode(value);

        if (length > value.Length)
        {
            Span<byte> buffer = stackalloc byte[length];
            _AlphabeticBer.GetBytes(value).AsSpan().CopyTo(buffer);

            return buffer.ToArray();
        }

        return _AlphabeticBer.GetBytes(value)[..length];
    }

    public byte[] Encode(string value) => Encode(value.AsSpan());
    public override ushort GetByteCount<T>(T value) => throw new NotImplementedException();

    public override ushort GetByteCount<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new InternalEmvEncodingException("This exception should never be reached");
    }

    /// <exception cref="EmvEncodingFormatException"></exception>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    private static void Validate(ReadOnlySpan<char> value)
    {
        if (!_AlphabeticBer.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    /// <exception cref="EmvEncodingFormatException"></exception>
    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_AlphabeticBer.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    #endregion

    #region Serialization

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public override DecodedResult<char[]> Decode(ReadOnlySpan<byte> value) => new(_AlphabeticBer.GetChars(value), value.Length);

    #endregion
}