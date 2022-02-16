using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class CompressedNumericCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, char> _CharMap = new Dictionary<byte, char>
    {
        {48, '0'},
        {49, '1'},
        {50, '2'},
        {51, '3'},
        {52, '4'},
        {53, '5'},
        {54, '6'},
        {55, '7'},
        {56, '8'},
        {57, '9'},
        {70, 'F'}
    }.ToImmutableSortedDictionary();

    private static readonly ImmutableSortedDictionary<byte, char> _CharValueMap = new Dictionary<byte, char>
    {
        {48, '0'},
        {49, '1'},
        {50, '2'},
        {51, '3'},
        {52, '4'},
        {53, '5'},
        {54, '6'},
        {55, '7'},
        {56, '8'},
        {57, '9'}
    }.ToImmutableSortedDictionary();

    private static KeyValuePair<byte, char> _PaddingKey;
    public static BerEncodingId Identifier = GetBerEncodingId(typeof(CompressedNumericCodec));
    private const byte _PaddingByteKey = 70;
    private const char _PaddingCharKey = 'F';
    private const byte _LeftNibbleMask = (byte) (Bits.Eight | Bits.Seven | Bits.Six | Bits.Five);
    private const byte _PaddedNibble = 0xF;

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override bool IsValid(ReadOnlySpan<byte> value) => IsNumericEncodingValid(value[..^GetPaddingIndexFromEnd(value)]);
    public override byte[] Encode<T>(T[] value) => throw new NotImplementedException();
    public override byte[] Encode<T>(T[] value, int length) => throw new NotImplementedException();

    public override byte[] Encode<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteSize)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= Specs.Integer.UInt32.ByteSize)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (byteSize <= Specs.Integer.UInt64.ByteSize)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    public override byte[] Encode<T>(T value, int length)
    {
        if (length == Specs.Integer.UInt8.ByteSize)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (length == Specs.Integer.UInt16.ByteSize)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (length == 3)
            return Encode(Unsafe.As<T, uint>(ref value), length);
        if (length == Specs.Integer.UInt32.ByteSize)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (length < Specs.Integer.UInt64.ByteSize)
            return Encode(Unsafe.As<T, ulong>(ref value), length);
        if (length == Specs.Integer.UInt64.ByteSize)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value), length);
    }

    public byte[] Encode(byte value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt8.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(ushort value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt16.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(uint value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt32.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(uint value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        if (((numberOfDigits % 2) + (numberOfDigits / 2)) == length)
            return Encode(value);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(Specs.Integer.UInt32.CompressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;

        // space for padding
        if (length > ((numberOfDigits % 2) + (numberOfDigits / 2)))
        {
            for (int i = 0, j = (length * 2) - ((length * 2) - numberOfDigits) - 1; i < numberOfDigits; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (uint) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (uint) Math.Pow(10, j)) % 10);
            }
        }

        // truncate
        for (int i = 0, j = numberOfDigits - 1; i < (length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (uint) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (uint) Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(ulong value)
    {
        const byte compressedNumericByteSize = Specs.Integer.UInt64.CompressedNumericByteSize;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(compressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;
        byte numberOfDigits = value.GetNumberOfDigits();

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(ulong value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        if (((numberOfDigits % 2) + (numberOfDigits / 2)) == length)
            return Encode(value);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(Specs.Integer.UInt64.CompressedNumericByteSize);
        Span<byte> buffer = spanOwner.Span;

        // space for padding
        if (length > ((numberOfDigits % 2) + (numberOfDigits / 2)))
        {
            for (int i = 0, j = (length * 2) - ((length * 2) - numberOfDigits) - 1; i < numberOfDigits; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (ulong) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (ulong) Math.Pow(10, j)) % 10);
            }
        }

        // truncate
        for (int i = 0, j = numberOfDigits - 1; i < (length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (ulong) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (ulong) Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(BigInteger value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((numberOfDigits % 2) + (numberOfDigits / 2));
        Span<byte> buffer = spanOwner.Span;

        for (int i = 0, j = numberOfDigits - 1; i < numberOfDigits; i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public byte[] Encode(BigInteger value, int length)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        if (((numberOfDigits % 2) + (numberOfDigits / 2)) == length)
            return Encode(value);

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(length);
        Span<byte> buffer = spanOwner.Span;

        // space for padding
        if (length > ((numberOfDigits % 2) + (numberOfDigits / 2)))
        {
            for (int i = 0, j = (length * 2) - ((length * 2) - numberOfDigits) - 1; i < numberOfDigits; i++, j--)
            {
                if ((i % 2) == 0)
                    buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
                else
                    buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
            }
        }

        // truncate
        for (int i = 0, j = numberOfDigits - 1; i < (length * 2); i++, j--)
        {
            if ((i % 2) == 0)
                buffer[i / 2] = (byte) ((byte) ((value / (BigInteger) Math.Pow(10, j)) % 10) << 4);
            else
                buffer[i / 2] |= (byte) ((value / (BigInteger) Math.Pow(10, j)) % 10);
        }

        if ((numberOfDigits % 2) != 0)
        {
            buffer[numberOfDigits / 2] |= 0x0F;
            buffer[((numberOfDigits / 2) + 1)..].Fill(0xFF);

            return buffer.ToArray();
        }

        buffer[(numberOfDigits / 2)..].Fill(0xFF);

        return buffer.ToArray();
    }

    public override ushort GetByteCount<T>(T value) => checked((ushort) Unsafe.SizeOf<T>());
    public override ushort GetByteCount<T>(T[] value) => throw new NotImplementedException();

    public BigInteger DecodeBigInteger(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
        }

        BigInteger result = 0;

        return BuildInteger(result, value);
    }

    public byte DecodeByte(byte value)
    {
        if (value.GetMaskedValue(_LeftNibbleMask) == _PaddedNibble)
            return (byte) (value >> 4);

        byte result = (byte) (value >> 4);
        result *= 10;
        result += value.GetMaskedValue(_LeftNibbleMask);

        return result;
    }

    public ushort DecodeUInt16(ReadOnlySpan<byte> value)
    {
        ushort result = 0;

        return BuildInteger(result, value);
    }

    public uint DecodeUInt32(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
        }

        uint result = 0;

        return BuildInteger(result, value);
    }

    public ulong DecodeUInt64(ReadOnlySpan<byte> value)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            if (!IsValid(value))
                throw new EmvEncodingFormatException(new ArgumentOutOfRangeException(nameof(value)));
        }

        ulong result = 0;

        return BuildInteger(result, value);
    }

    /// <summary>
    ///     Validates that the left justified numeric values are encoded correctly
    /// </summary>
    /// <exception cref="EmvEncodingFormatException"></exception>
    private bool ValidateNumericEncoding(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!_CharMap.ContainsKey(value[i]))
            {
                throw new EmvEncodingFormatException(
                    $"The argument could not be parsed. The argument contained the value: [{value[i]}], which is an invalid {nameof(CompressedNumeric)} encoding");
            }
        }

        return true;
    }

    /// <summary>
    ///     Validates that the left justified numeric values are encoded correctly
    /// </summary>
    /// <exception cref="EmvEncodingFormatException"></exception>
    private bool IsNumericEncodingValid(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (!_CharMap.ContainsKey(value[i]))
                return false;
        }

        return true;
    }

    private int GetPaddingIndexFromEnd(ReadOnlySpan<byte> value)
    {
        for (int i = value.Length; i > 0; i--)
        {
            if (value[i] != _PaddingByteKey)
                return i;
        }

        return 0;
    }

    private dynamic BuildInteger(dynamic resultBuffer, ReadOnlySpan<byte> value)
    {
        if (resultBuffer != byte.MinValue)
            resultBuffer = 0;

        for (byte i = 0; i < value.Length; i++)
        {
            resultBuffer += DecodeByte(value[i]);
            resultBuffer <<= 8;
        }

        return resultBuffer;
    }

    private static byte[] TrimTrailingBytes(ReadOnlySpan<byte> value)
    {
        int padding = 0;

        for (int i = value.Length; i > 0; i--)
        {
            if (((value[i] >> 4) == _PaddedNibble) && (value[i].GetMaskedValue(0xF0) == _PaddedNibble))
            {
                padding++;

                continue;
            }

            break;
        }

        byte[] result = value[..^padding].ToArray();
        if (result[^1].GetMaskedValue(0xF0) == _PaddedNibble)
            result[^1] = result[^1].GetMaskedValue(0x0F);

        return result;
    }

    protected override void Validate(ReadOnlySpan<byte> value)
    {
        ValidateNumericEncoding(value[..^GetPaddingIndexFromEnd(value)]);
    }

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        Validate(value);

        ReadOnlySpan<byte> trimmedValue = TrimTrailingBytes(value);
        BigInteger maximumIntegerResult = (BigInteger) Math.Pow(2, value.Length * 8);

        if (maximumIntegerResult <= byte.MaxValue)
        {
            byte byteResult = DecodeByte(trimmedValue[0]);

            return new DecodedResult<byte>(byteResult, PlayEncoding.UnsignedInteger.GetCharCount(byteResult));
        }

        if (maximumIntegerResult <= ushort.MaxValue)
        {
            ushort shortResult = DecodeUInt16(trimmedValue);

            return new DecodedResult<ushort>(shortResult, PlayEncoding.UnsignedInteger.GetCharCount(shortResult));
        }

        if (maximumIntegerResult <= uint.MaxValue)
        {
            uint intResult = DecodeUInt32(trimmedValue);

            return new DecodedResult<uint>(intResult, PlayEncoding.UnsignedInteger.GetCharCount(intResult));
        }

        if (maximumIntegerResult <= ulong.MaxValue)
        {
            ulong longResult = DecodeUInt64(trimmedValue);

            return new DecodedResult<ulong>(longResult, PlayEncoding.UnsignedInteger.GetCharCount(longResult));
        }

        BigInteger bigIntegerResult = DecodeBigInteger(trimmedValue);

        return new DecodedResult<BigInteger>(bigIntegerResult, PlayEncoding.UnsignedInteger.GetCharCount(bigIntegerResult));
    }

    #endregion
}