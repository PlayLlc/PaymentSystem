using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Strings;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class AlphaNumericSpecialEmvCodec : Codec
{
    #region Static Metadata

    private static readonly AlphaNumericSpecial _AlphanumericSpecial = PlayEncoding.AlphaNumericSpecial;

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<byte> value) => _AlphanumericSpecial.IsValid(value);

    /// <exception cref="EncodingException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_AlphanumericSpecial.IsValid(value))
            throw new EncodingException(EncodingException.CharacterArrayContainsInvalidValue + " - The offending value was value[i]");
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new InternalEmvEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new InternalEmvEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        throw new InternalEmvEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        throw new InternalEmvEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), buffer, ref offset);
        else
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length, buffer, ref offset);
        else
        {
            throw new InternalEmvEncodingException(
                $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");
        }
    }

    public override byte[] Encode<T>(T value) =>
        throw new InternalEmvEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    public override byte[] Encode<T>(T value, int length) =>
        throw new InternalEmvEncodingException(
            $"The {nameof(AlphaNumericSpecialEmvCodec)} does not have the capability to {nameof(Encode)} the type: [{typeof(T)}]");

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value) => _AlphanumericSpecial.GetBytes(value);

    public void Encode(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset) =>
        _AlphanumericSpecial.GetBytes(value, buffer, ref offset);

    /// <exception cref="EncodingException"></exception>
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

    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        if (value.Length == length)
        {
            Encode(value, buffer, ref offset);

            return;
        }

        if (length > value.Length)
        {
            _AlphanumericSpecial.GetBytes(value, buffer, ref offset);

            offset += length - value.Length;
        }

        _AlphanumericSpecial.GetBytes(value[..length], buffer, ref offset);
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
        char[] valueResult = PlayEncoding.AlphaNumericSpecial.GetChars(value);

        return new DecodedResult<char[]>(valueResult, valueResult.Length);
    }

    #endregion
}