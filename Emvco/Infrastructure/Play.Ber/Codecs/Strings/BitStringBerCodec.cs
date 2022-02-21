using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Integers;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Ber.Codecs;

/// <summary>
///     The encoding of a bit string value shall be either primitive or constructed at the option of the sender.
///     NOTE – Where it is necessary to transfer part of a bit string before the entire bit string is available, the
///     constructed encoding is
///     used.
/// </summary>
/// <remarks>
///     X.690-0270 Section 8.6
/// </remarks>
public sealed class BitStringBerCodec : BerPrimitiveCodec
{
    #region Metadata

    private static readonly UnsignedInteger _UnsignedIntegerCodec = PlayEncoding.UnsignedInteger;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(BitStringBerCodec));

    //public override DecodedResult<T[]> DecodeArray<T>(ReadOnlySpan<byte> value)
    //{
    //    throw new NotImplementedException();
    //}

    //private DecodedResult<byte> Decode(ReadOnlySpan<byte> value)
    //{
    //    Validate(value); 

    //    return new DecodedResult<byte>(value[0], _UnsignedIntegerCodec.GetCharCount(value[0]));
    //}
    public const byte MinByteCount = 0x01;
    public const byte LeadingOctetMinValue = 0x01;
    public const byte LeadingOctetMaxValue = 0x07;

    #endregion

    #region Instance Values

    private readonly byte[] _EmptyBitString = {0x00};

    #endregion

    #region Count

    public override ushort GetByteCount<T>(T[] value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == 1)
            return (ushort) Unsafe.As<T[], byte[]>(ref value).AsSpan().Length;

        throw new NotImplementedException();
    }

    public override ushort GetByteCount<T>(T value) => (ushort) Unsafe.SizeOf<T>();

    #endregion

    #region Validation

    public override bool IsValid(ReadOnlySpan<byte> value) =>
        IsBitStringLengthValid(value) && IsLeadingOctetInRange(value) && AreUnusedBitsCorrect(value);

    protected override void Validate(ReadOnlySpan<byte> value)
    {
        if (value.Length == 1)
            return;

        if (!IsBitStringLengthValid(value))
        {
            throw new BerFormatException(new ArgumentOutOfRangeException(nameof(value),
                "If the Bit String has only one byte then that byte must be zero"));
        }

        if (!IsLeadingOctetInRange(value))
        {
            throw new BerFormatException(new ArgumentOutOfRangeException($"The argument {nameof(value)} was out of range. "
                + $"The initial octet must be greater than {LeadingOctetMinValue} and less than {LeadingOctetMaxValue}"));
        }

        if (!AreUnusedBitsCorrect(value))
        {
            throw new BerFormatException(new ArgumentOutOfRangeException(nameof(value),
                "The raw value does not conform to the BitStringTag encoding rules. The leading octet must indicate the number of "
                + "trailing unset bits in the last subsequent octet"));
        }
    }

    /// <summary>
    ///     If the BitStringTag is empty, there shall be no subsequent octets, and the initial octet shall be zero.
    /// </summary>
    /// <remarks>
    ///     X.690-0270 Section 8.6.2.3
    /// </remarks>
    /// <param name="value"></param>
    private static bool IsBitStringLengthValid(ReadOnlySpan<byte> value)
    {
        if (value.Length > MinByteCount)
            return true;

        if (value.Length < MinByteCount)
            return false;

        if ((value.Length == MinByteCount) && (value[0] != 0x00))
            return false;

        return true;
    }

    #endregion

    #region Encode

    public override byte[] Encode<T>(T[] value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == 1)
            return Encode(Unsafe.As<T[], byte[]>(ref value).AsSpan());

        throw new NotImplementedException();
    }

    public override byte[] Encode<T>(T[] value, int length)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == 1)
            return Encode(Unsafe.As<T[], byte[]>(ref value).AsSpan(), length);

        throw new NotImplementedException();
    }

    public override byte[] Encode<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == 1)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (byteSize == 2)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (byteSize <= 4)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (byteSize <= 8)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    public override byte[] Encode<T>(T value, int length)
    {
        if (length == 1)
            return Encode(Unsafe.As<T, byte>(ref value));
        if (length == 2)
            return Encode(Unsafe.As<T, ushort>(ref value));
        if (length == 3)
            return Encode((ushort) Unsafe.As<T, uint>(ref value), length);
        if (length == 4)
            return Encode(Unsafe.As<T, uint>(ref value));
        if (length < 8)
            return Encode(Unsafe.As<T, ulong>(ref value), length);
        if (length == 8)
            return Encode(Unsafe.As<T, ulong>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value), length);
    }

    public byte[] Encode(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            return _EmptyBitString;

        int byteCount = value.Length;
        byte unsetBitCount = 0;

        for (int i = 0; i < byteCount; i++)
        {
            byte unsetBits = value[i].RightPaddedUnsetBitCount();

            unsetBitCount += unsetBits;

            if (unsetBits < 8)
                break;
        }

        byte[] result = new byte[byteCount + 1];

        result[0] = unsetBitCount;

        for (int i = 0; i < byteCount; i++)
            result[i + 1] = value[i];

        return result;
    }

    public byte[] Encode(ReadOnlySpan<byte> value, int length)
    {
        if (value.IsEmpty)
            return _EmptyBitString;

        int byteCount = length;
        byte unsetBitCount = 0;

        for (int i = 0; i < byteCount; i++)
        {
            byte unsetBits = value[i].RightPaddedUnsetBitCount();

            unsetBitCount += unsetBits;

            if (unsetBits < 8)
                break;
        }

        byte[] result = new byte[byteCount + 1];

        result[0] = unsetBitCount;

        for (int i = 0; i < byteCount; i++)
            result[i + 1] = value[i];

        return result;
    }

    public byte[] Encode(byte value)
    {
        if (value == 0)
            return _EmptyBitString;

        return new[] {value.RightPaddedUnsetBitCount(), value};
    }

    public byte[] Encode(ushort value)
    {
        if (value == 0)
            return _EmptyBitString;

        byte[] result = new byte[Specs.Integer.UInt16.BitStringByteSize];
        result[0] = value.RightPaddedUnsetBitCount();
        new Span<byte>(_UnsignedIntegerCodec.GetBytes(value)).CopyTo(result[1..]);

        return result;
    }

    public byte[] Encode(uint value)
    {
        if (value == 0)
            return _EmptyBitString;

        byte[] result = new byte[Specs.Integer.UInt32.BitStringByteSize];
        result[0] = value.RightPaddedUnsetBitCount();
        new Span<byte>(_UnsignedIntegerCodec.GetBytes(value)).CopyTo(result[1..]);

        return result;
    }

    public byte[] Encode(uint value, int length)
    {
        if (value == 0)
            return _EmptyBitString;

        byte[] result = new byte[Specs.Integer.UInt32.BitStringByteSize];
        result[0] = value.RightPaddedUnsetBitCount();
        new Span<byte>(_UnsignedIntegerCodec.GetBytes(value))[((Specs.Integer.UInt32.ByteCount - length) + 1)..].CopyTo(result[1..]);

        return result;
    }

    public byte[] Encode(ulong value)
    {
        if (value == 0)
            return _EmptyBitString;

        byte[] result = new byte[Specs.Integer.UInt64.BitStringByteSize];
        result[0] = value.RightPaddedUnsetBitCount();
        new Span<byte>(_UnsignedIntegerCodec.GetBytes(value)).CopyTo(result[1..]);

        return result;
    }

    public byte[] Encode(ulong value, int length)
    {
        if (value == 0)
            return _EmptyBitString;

        byte[] result = new byte[Specs.Integer.UInt64.BitStringByteSize];
        result[0] = value.RightPaddedUnsetBitCount();
        new Span<byte>(_UnsignedIntegerCodec.GetBytes(value))[((Specs.Integer.UInt64.ByteCount - length) + 1)..].CopyTo(result[1..]);

        return result;
    }

    // TODO: THIS IS INCORRECT - the length can be either larger or smaller than the value so we need to account
    // TODO: for truncating and padding
    public byte[] Encode(BigInteger value, int length)
    {
        if (value == 0)
            return _EmptyBitString;

        byte[] result = new byte[value.GetMostSignificantByte() + 1];
        result[0] = value.RightPaddedUnsetBitCount();
        new Span<byte>(_UnsignedIntegerCodec.GetBytes(value))[((value.GetMostSignificantByte() - length) + 1)..].CopyTo(result[1..]);

        return result;
    }

    public byte[] Encode(BigInteger value)
    {
        if (value == 0)
            return _EmptyBitString;

        byte[] result = new byte[value.GetMostSignificantByte() + 1];
        result[0] = value.RightPaddedUnsetBitCount();
        new Span<byte>(_UnsignedIntegerCodec.GetBytes(value)).CopyTo(result[1..]);

        return result;
    }

    // BUG: This whole Encode section looks super sketchy. Check the specs again and make sure this is correct
    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <param name="bitFlags"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public byte[] Encode(params byte[] bitFlags)
    {
        if (bitFlags.Length > 8)
        {
            throw new BerFormatException(
                new InvalidOperationException(
                    $"The argument {nameof(bitFlags)} had more flags passed in than there are bits in a {typeof(byte)}"));
        }

        byte bitStringValue = 0;

        for (byte i = 0; i < bitFlags.Length; i++)
            bitStringValue |= bitFlags[i];

        return Encode(bitStringValue);
    }

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <param name="bitFlags"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public byte[] Encode(params ushort[] bitFlags)
    {
        if (bitFlags.Length > 16)
        {
            throw new BerFormatException(new InvalidOperationException(
                $"The argument {nameof(bitFlags)} had more flags passed in than there are bits in a {typeof(ushort)}"));
        }

        ushort bitStringValue = 0;

        for (byte i = 0; i < bitFlags.Length; i++)
            bitStringValue |= bitFlags[i];

        return Encode(bitStringValue);
    }

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <param name="bitFlags"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public byte[] Encode(params uint[] bitFlags)
    {
        if (bitFlags.Length > 32)
        {
            throw new BerFormatException(new InvalidOperationException(
                $"The argument {nameof(bitFlags)} had more flags passed in than there are bits in a {typeof(ushort)}"));
        }

        uint bitStringValue = 0;

        for (byte i = 0; i < bitFlags.Length; i++)
            bitStringValue |= bitFlags[i];

        return Encode(bitStringValue);
    }

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <param name="bitFlags"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public byte[] Encode(params ulong[] bitFlags)
    {
        if (bitFlags.Length > 64)
        {
            throw new BerFormatException(new InvalidOperationException(
                $"The argument {nameof(bitFlags)} had more flags passed in than there are bits in a {typeof(ushort)}"));
        }

        ulong bitStringValue = 0;

        for (byte i = 0; i < bitFlags.Length; i++)
            bitStringValue |= bitFlags[i];

        return Encode(bitStringValue);
    }

    public override void Encode<T>(T value, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Decode To DecodedMetadata

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        Validate(value);

        if (value.Length == 1)
        {
            byte byteResult = value[0];

            return new DecodedResult<byte>(byteResult, _UnsignedIntegerCodec.GetCharCount(byteResult));
        }

        if (value.Length == 2)
        {
            ushort shortResult = _UnsignedIntegerCodec.GetUInt16(value);

            return new DecodedResult<ushort>(shortResult, _UnsignedIntegerCodec.GetCharCount(shortResult));
        }

        if (value.Length <= 4)
        {
            uint intResult = _UnsignedIntegerCodec.GetUInt32(value);

            return new DecodedResult<uint>(intResult, _UnsignedIntegerCodec.GetCharCount(intResult));
        }

        if (value.Length <= 8)
        {
            ulong longResult = _UnsignedIntegerCodec.GetUInt64(value);

            return new DecodedResult<ulong>(longResult, _UnsignedIntegerCodec.GetCharCount(longResult));
        }

        BigInteger bigIntegerResult = _UnsignedIntegerCodec.GetBigInteger(value);

        return new DecodedResult<BigInteger>(bigIntegerResult, _UnsignedIntegerCodec.GetCharCount(bigIntegerResult));
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    private static bool AreUnusedBitsCorrect(ReadOnlySpan<byte> value) => value[^1].RightPaddedUnsetBitCount() == value[0];

    private static bool IsLeadingOctetInRange(ReadOnlySpan<byte> value)
    {
        if (value[0] < LeadingOctetMinValue)
            return false;

        if (value[0] > LeadingOctetMaxValue)
            return false;

        return true;
    }

    #endregion
}