using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Codecs;

public class SpecialCodec : PlayCodec
{
    #region Instance Members

    #region Decode To Integers

    /// <summary>
    ///     GetByte
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public byte DecodeToByte(char value)
    {
        Validate(value);

        return _ByteMapper[value];
    }

    #endregion

    #endregion

    #region Serialization

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value) => throw new NotImplementedException();

    #endregion

    #endregion

    #region Metadata

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public static readonly PlayEncodingId EncodingId = new(typeof(SpecialCodec));

    // 32 - 126
    private static readonly ImmutableSortedDictionary<char, byte> _ByteMapper = Enumerable.Range(32, 47 - 32)
        .Concat(Enumerable.Range(58, 64 - 58)).Concat(Enumerable.Range(91, 96 - 91)).Concat(Enumerable.Range(123, 126 - 123))
        .ToImmutableSortedDictionary(a => (char) a, b => (byte) b);

    private static readonly ImmutableSortedDictionary<byte, char> _CharMapper = Enumerable.Range(32, 47 - 32)
        .Concat(Enumerable.Range(58, 64 - 58)).Concat(Enumerable.Range(91, 96 - 91)).Concat(Enumerable.Range(123, 126 - 123))
        .ToImmutableSortedDictionary(a => (byte) a, b => (char) b);

    #endregion

    #region Count

    public int GetByteCount(char[] chars, int index, int count) => count;
    public int GetMaxByteCount(int charCount) => charCount;
    public override ushort GetByteCount<_T>(_T value) => (ushort) Unsafe.SizeOf<_T>();

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions._Temp.CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override ushort GetByteCount<_T>(_T[] value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        return (ushort) Unsafe.As<_T[], char[]>(ref value).Length;
    }

    public int GetCharCount(byte[] bytes, int index, int count) => count;
    public int GetMaxCharCount(int byteCount) => byteCount;

    #endregion

    #region Validation

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public bool IsValid(ReadOnlySpan<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!IsValid(value[i]))
                return false;
        }

        return true;
    }

    public bool IsValid(byte value) => _CharMapper.ContainsKey(value);
    public bool IsValid(char value) => _ByteMapper.ContainsKey(value);

    /// <exception cref="CodecParsingException"></exception>
    private void Validate(byte value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
    }

    /// <exception cref="CodecParsingException"></exception>
    private void Validate(char value)
    {
        if (!IsValid(value))
            throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
    }

    /// <exception cref="CodecParsingException"></exception>
    protected void Validate(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        for (int i = 0; i <= (value.Length - 1); i++)
        {
            if (!IsValid(value[i]))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
        }
    }

    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private void Validate(ReadOnlySpan<char> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        for (int i = 0; i <= (value.Length - 1); i++)
        {
            if (!IsValid(value[i]))
                throw new CodecParsingException(CodecParsingException.CharacterArrayContainsInvalidValue);
        }
    }

    #endregion

    #region Encode

    /// <exception cref="CodecParsingException"></exception>
    public bool TryEncoding(ReadOnlySpan<char> value, out byte[] result)
    {
        if (IsValid(value))
        {
            result = Encode(value);

            return true;
        }

        result = Array.Empty<byte>();

        return false;
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        return new byte[] {DecodeToByte(Unsafe.As<_T, char>(ref value))};
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T value, int length)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        return new byte[] {DecodeToByte(Unsafe.As<_T, char>(ref value))};
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T[] value)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        return Encode(Unsafe.As<_T[], char[]>(ref value));
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] Encode<_T>(_T[] value, int length)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        return Encode(Unsafe.As<_T[], char[]>(ref value), length);
    }

    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value)
    {
        byte[] byteArray = new byte[value.Length];

        for (int i = 0; i < value.Length; i++)
        {
            Validate(value[i]);
            byteArray[i] = _ByteMapper[value[i]];
        }

        return byteArray;
    }

    /// <exception cref="CodecParsingException"></exception>
    public byte[] Encode(ReadOnlySpan<char> value, int length)
    {
        byte[] byteArray = new byte[length];

        for (int i = 0; i < length; i++)
        {
            Validate(value[i]);
            byteArray[i] = _ByteMapper[value[i]];
        }

        return byteArray;
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="chars"></param>
    /// <param name="charIndex"></param>
    /// <param name="charCount"></param>
    /// <param name="bytes"></param>
    /// <param name="byteIndex"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions._Temp.CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public int Encode(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        if (charIndex > chars.Length)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if (byteIndex > bytes.Length)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if ((byteIndex + charCount) > bytes.Length)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if (charIndex > chars.Length)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if ((byteIndex + charCount) > (bytes.Length - byteIndex))
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if ((bytes.Length - byteIndex) < charCount)
            throw new CodecParsingException("The byte array buffer provided was smaller than expected");

        for (int i = charIndex, j = byteIndex; j < charCount; i++, j++)
        {
            Validate(chars[j]);
            bytes[j] = _ByteMapper[chars[i]];
        }

        return charCount;
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public void Encode(ReadOnlySpan<char> value, Span<byte> buffer, ref int offset)
    {
        for (int j = 0; j < value.Length; offset++)
        {
            Validate(value[j]);

            buffer[offset] = _ByteMapper[value[j]];
        }
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public void Encode(ReadOnlySpan<char> value, int length, Span<byte> buffer, ref int offset)
    {
        for (int j = 0; j < value.Length; offset++)
        {
            Validate(value[j]);

            buffer[offset] = _ByteMapper[value[j]];
        }
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T value, Span<byte> buffer, ref int offset)
    {
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        buffer[offset++] = _ByteMapper[Unsafe.As<_T, char>(ref value)];
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T value, int length, Span<byte> buffer, ref int offset)
    {
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        buffer[offset++] = _ByteMapper[Unsafe.As<_T, char>(ref value)];
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset)
    {
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        Encode(Unsafe.As<_T[], char[]>(ref value), buffer, ref offset);
    }

    /// <summary>
    ///     Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset)
    {
        // TODO: this is inefficient it's using reflection. Let's try and optimize this somehow
        Type type = typeof(_T);

        if (!type.IsChar())
            throw new CodecParsingException(this, type);

        Encode(Unsafe.As<_T[], char[]>(ref value), length, buffer, ref offset);
    }

    #endregion

    #region Decode To Chars

    /// <summary>
    ///     DecodeToChars
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="byteIndex"></param>
    /// <param name="byteCount"></param>
    /// <param name="chars"></param>
    /// <param name="charIndex"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public int DecodeToChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        if (byteIndex > chars.Length)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if (charIndex > chars.Length)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if ((charIndex + byteCount) > bytes.Length)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if (byteIndex > bytes.Length)
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if ((charIndex + byteCount) > (chars.Length - charIndex))
            throw new CodecParsingException(new ArgumentOutOfRangeException(nameof(chars)));

        if ((chars.Length - charIndex) < byteCount)
            throw new CodecParsingException("The byte array buffer provided was smaller than expected");

        for (int i = byteIndex, j = charIndex; j < byteCount; i++, j++)
            bytes[j] = _ByteMapper[chars[i]];

        return byteCount;
    }

    public char[] DecodeToChars(ReadOnlySpan<byte> value)
    {
        char[] result = new char[value.Length];
        for (int i = 0; i < value.Length; i++)
            result[i] = _CharMapper[value[i]];

        return result;
    }

    /// <summary>
    ///     GetChar
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="CodecParsingException"></exception>
    public char DecodeToChar(byte value)
    {
        Validate(value);

        return _CharMapper[value];
    }

    #endregion

    #region Decode To String

    /// <exception cref="CodecParsingException"></exception>
    public string DecodeToString(ReadOnlySpan<byte> value)
    {
        Validate(value);

        if (value.Length >= Specs.ByteArray.StackAllocateCeiling)
        {
            using SpanOwner<char> owner = SpanOwner<char>.Allocate(value.Length);
            Span<char> buffer = owner.Span;

            for (int i = 0; i <= (value.Length - 1); i++)
                buffer[i] = _CharMapper[value[i]];

            return new string(buffer);
        }
        else
        {
            Span<char> buffer = stackalloc char[value.Length];
            for (int i = 0; i <= (value.Length - 1); i++)
                buffer[i] = _CharMapper[value[i]];

            return new string(buffer);
        }
    }

    /// <exception cref="CodecParsingException"></exception>
    public bool TryDecodingToString(ReadOnlySpan<byte> value, out string result)
    {
        result = string.Empty;

        if (IsValid(value))
        {
            result = DecodeToString(value);

            return true;
        }

        return false;
    }

    #endregion
}