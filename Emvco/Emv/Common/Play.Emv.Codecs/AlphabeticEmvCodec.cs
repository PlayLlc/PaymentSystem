using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Codecs.Strings;
using Play.Emv.Codecs.Exceptions;

namespace Play.Emv.Codecs;

/// <summary>
///     An encoder for encoding and decoding alphabetic ASCII characters
/// </summary>
/// <remarks>
///     Strict parsing is enforced. Exceptions will be raised if invalid data is attempted to be parsed
/// </remarks>
public class AlphabeticEmvCodec : IPlayCodec
{
    #region Static Metadata

    private static readonly Alphabetic _Alphabetic = PlayEncoding.Alphabetic;

    #endregion

    #region Instance Members

    /// <exception cref="EncodingException"></exception>
    public bool IsValid(ReadOnlySpan<byte> value) => _Alphabetic.IsValid(value);

    public byte[] Encode<T>(T value) where T : struct => throw new NotImplementedException();
    public byte[] Encode<T>(T value, int length) where T : struct => throw new NotImplementedException();

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode<T>(T[] value) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan());

        throw new NotImplementedException();
    }

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode<T>(T[] value, int length) where T : struct
    {
        if (typeof(T) == typeof(char))
            return Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length);

        throw new NotImplementedException();
    }

    public void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        throw new NotImplementedException();
    }

    public void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), buffer, ref offset);

        throw new NotImplementedException();
    }

    public void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct
    {
        if (typeof(T) == typeof(char))
            Encode(Unsafe.As<T[], char[]>(ref value).AsSpan(), length, buffer, ref offset);
    }

    /// <exception cref="EncodingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value) => _Alphabetic.GetBytes(value);

    public void Encode(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset) => _Alphabetic.GetBytes(value, buffer, ref offset);

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

    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        if (value.Length == length)
        {
            Encode(value, buffer, ref offset);

            return;
        }

        if (length > value.Length)
        {
            _Alphabetic.GetBytes(value, buffer, ref offset);

            return;
        }

        _Alphabetic.GetBytes(value, length, buffer, ref offset);
    }

    public byte[] Encode(string value) => Encode(value.AsSpan());
    public ushort GetByteCount<T>(T value) where T : struct => throw new NotImplementedException();

    public ushort GetByteCount<T>(T[] value) where T : struct
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
    protected void Validate(ReadOnlySpan<byte> value)
    {
        if (!_Alphabetic.IsValid(value))
            throw new EmvEncodingFormatException(EncodingException.CharacterArrayContainsInvalidValue);
    }

    #endregion

    #region Serialization

    /// <exception cref="EncodingException"></exception>
    public DecodedMetadata Decode(ReadOnlySpan<byte> value) =>
        new DecodedResult<char[]>(PlayEncoding.Alphabetic.GetChars(value), value.Length);

    #endregion
}