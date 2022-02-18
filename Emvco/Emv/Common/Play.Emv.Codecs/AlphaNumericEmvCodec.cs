using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Strings;
using Play.Core.Exceptions;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class AlphaNumericEmvCodec : Codec
{
    #region Static Metadata

    private static readonly AlphaNumeric _AlphaNumeric = PlayEncoding.AlphaNumeric;

    #endregion

    #region Instance Members

    public override bool IsValid(ReadOnlySpan<byte> value) => _AlphaNumeric.IsValid(value);

    public override byte[] Encode<T>(T value)
    {
        if (typeof(T) == typeof(char))
            return new byte[] {Encode(Unsafe.As<T, char>(ref value))};

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public override byte[] Encode<T>(T value, int length)
    {
        CheckCore.ForRange(length, 1, 1, nameof(length));

        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T, char>(ref value), length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    public override byte[] Encode<T>(T[] value, int length)
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T, char>(ref value), buffer, ref offset);
        else
            throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T, char>(ref value), buffer, ref offset);
        else
            throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), buffer, ref offset);
        else
            throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length, buffer, ref offset);
        else
            throw new InternalEmvEncodingException("The code should not reach this point");
    }

    private byte Encode(char character) => _AlphaNumeric.GetByte(character);

    private void Encode(char character, Span<byte> buffer, ref int offset)
    {
        buffer[offset++] = _AlphaNumeric.GetByte(character);
    }

    /// <exception cref="EncodingException"></exception>
    public static byte[] Encode(ReadOnlySpan<char> value) => _AlphaNumeric.GetBytes(value);

    public static void Encode(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset) =>
        _AlphaNumeric.GetBytes(value, buffer, ref offset);

    /// <exception cref="EncodingException"></exception>
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

    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        if (value.Length == length)
        {
            Encode(value, buffer, ref offset);

            return;
        }

        if (length > value.Length)
        {
            _AlphaNumeric.GetBytes(value, buffer, ref offset);

            return;
        }

        _AlphaNumeric.GetBytes(value, buffer, ref offset);
    }

    public byte[] Encode(string value) => Encode(value.AsSpan());
    public override ushort GetByteCount<T>(T value) => 1;

    public override ushort GetByteCount<T>(T[] value)
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (!_AlphaNumeric.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    #endregion
}