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
using Play.Core.Exceptions;
using Play.Emv.Ber.Exceptions;

using PlayEncodingId = Play.Ber.Codecs.PlayEncodingId;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class AlphaNumericCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly AlphaNumeric _AlphaNumeric = PlayEncoding.AlphaNumeric;
    public static readonly PlayEncodingId Identifier = GetEncodingId(typeof(AlphaNumericCodec));

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => _AlphaNumeric.IsValid(value);

    public override byte[] Encode<T>(T value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T, char>(ref value));

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public override byte[] Encode<T>(T value, int length)
    {
        CheckCore.ForRange(length, 1, 1, nameof(length));

        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T, char>(ref value));

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public static byte[] Encode(ReadOnlySpan<char> value) => _AlphaNumeric.GetBytes(value);

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        if (value.Length == length)
            return Encode(value);

        if (length > value.Length)
        {
            Span<byte> buffer = stackalloc byte[length];
            _AlphaNumeric.GetBytes(value).AsSpan().CopyTo(buffer);

            return buffer.ToArray();
        }

        return _AlphaNumeric.GetBytes(value)[..length];
    }

    public byte[] Encode(string value) => Encode(value.AsSpan());
    public override ushort GetByteCount<T>(T value) => 1;

    public override ushort GetByteCount<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="Play.Codecs.Exceptions.PlayEncodingException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_AlphaNumeric.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    #endregion

    #region Serialization

    public override DecodedResult<char[]> Decode(ReadOnlySpan<byte> value) => new(_AlphaNumeric.GetChars(value), value.Length);

    #endregion
}