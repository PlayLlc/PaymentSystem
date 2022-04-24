using System.Runtime.CompilerServices;

using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Metadata;
using Play.Codecs.Strings;
using Play.Core.Exceptions;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

/// <summary>
///     An encoder for encoding and decoding alphabetic and numeric ASCII characters
/// </summary>
/// <remarks>
///     Strict parsing is enforced. Exceptions will be raised if invalid data is attempted to be parsed
/// </remarks>
public class AlphaNumericEmvCodec : IPlayCodec
{
    #region Static Metadata

    private static readonly AlphaNumeric _AlphaNumeric = PlayEncoding.AlphaNumeric;

    #endregion

    #region Instance Members

    public bool IsValid(ReadOnlySpan<byte> value) => _AlphaNumeric.IsValid(value);

    public byte[] Encode<T>(T value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return new byte[] {Encode(Unsafe.As<T, char>(ref value))};

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public byte[] Encode<T>(T value, int length) where T : struct
    {
        CheckCore.ForRange(length, 1, 1, nameof(length));

        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T, char>(ref value), length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode<T>(T[] value, int length) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T, char>(ref value), buffer, ref offset);
        else
            throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T, char>(ref value), buffer, ref offset);
        else
            throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), buffer, ref offset);
        else
            throw new InternalEmvEncodingException("The code should not reach this point");
    }

    public void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
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
    public ushort GetByteCount<T>(T value) where T : struct => 1;

    public ushort GetByteCount<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return checked((ushort) value.Length);

        throw new InternalEmvEncodingException("The code should not reach this point");
    }

    /// <exception cref="EncodingException"></exception>
    protected void Validate(ReadOnlySpan<byte> value)
    {
        if (!_AlphaNumeric.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    #endregion

    #region Serialization

    public DecodedMetadata Decode(ReadOnlySpan<byte> value) => new DecodedResult<char[]>(_AlphaNumeric.GetChars(value), value.Length);

    #endregion
}