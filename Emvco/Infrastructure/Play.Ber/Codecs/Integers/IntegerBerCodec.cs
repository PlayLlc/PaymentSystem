using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Integers;
using Play.Codecs.Metadata;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Core.Specifications;

namespace Play.Ber.Codecs;

/// <summary>
///     Signed Integer
/// </summary>
/// <remarks>
///     [ITU-T X.690] Section 8.3
/// </remarks>
public sealed class IntegerBerCodec : BerPrimitiveCodec
{
    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;

    #endregion

    #region Serialization

    #region Decode To DecodedMetadata

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerException"></exception>
    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        Validate(value);

        if (value.Length == 1)
        {
            sbyte byteResult = _SignedIntegerCodec.GetByte(value);

            return new DecodedResult<sbyte>(byteResult, _SignedIntegerCodec.GetCharCount(byteResult));
        }

        if (value.Length == 2)
        {
            short shortResult = _SignedIntegerCodec.DecodeToInt16(value);

            return new DecodedResult<short>(shortResult, _SignedIntegerCodec.GetCharCount(shortResult));
        }

        if (value.Length <= 4)
        {
            int intResult = _SignedIntegerCodec.DecodeToInt32(value);

            return new DecodedResult<int>(intResult, _SignedIntegerCodec.GetCharCount(intResult));
        }

        if (value.Length <= 8)
        {
            long longResult = _SignedIntegerCodec.DecodeToInt64(value);

            return new DecodedResult<long>(longResult, _SignedIntegerCodec.GetCharCount(longResult));
        }

        BigInteger bigIntegerResult = _SignedIntegerCodec.DecodeToBigInteger(value);

        return new DecodedResult<BigInteger>(bigIntegerResult, _SignedIntegerCodec.GetCharCount(bigIntegerResult));
    }

    #endregion

    #endregion

    #region Metadata

    private static readonly SignedInteger _SignedIntegerCodec = PlayEncoding.SignedInteger;
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(IntegerBerCodec));

    //if (length == Integer.UInt8.ByteSize)
    //    return EncodeTagLengthValue(Unsafe.As<T, sbyte>(ref value));
    //if (length == Integer.UInt16.ByteSize)
    //    return EncodeTagLengthValue(Unsafe.As<T, short>(ref value));
    //if (length == 3)
    //    return EncodeTagLengthValue(Unsafe.As<T, int>(ref value), length);
    //if (length == Integer.UInt32.ByteSize)
    //    return EncodeTagLengthValue(Unsafe.As<T, int>(ref value));
    //if (length < Integer.UInt64.ByteSize)
    //    return EncodeTagLengthValue(Unsafe.As<T, long>(ref value), length);
    //if (length == Integer.UInt64.ByteSize)
    //    return EncodeTagLengthValue(Unsafe.As<T, long>(ref value));
    //return EncodeTagLengthValue(Unsafe.As<T, BigInteger>(ref value), length);
    private const byte _MinimumByteLength = 1;

    #endregion

    #region Count

    public override ushort GetByteCount<T>(T value) => throw new NotImplementedException();
    public override ushort GetByteCount<T>(T[] value) => throw new NotImplementedException();

    #endregion

    #region Validation

    public override bool IsValid(ReadOnlySpan<byte> value) => IsMinimumLengthValid(value) && AreFirstNineBitsValid(value);

    /// <param name="value"></param>
    /// <exception cref="BerException"></exception>
    /// <remarks></remarks>
    protected override void Validate(ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (value.Length == 1)
            return;

        if (!AreFirstNineBitsValid(value))
        {
            throw new BerFormatException(new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(IntegerBerCodec)} failed because the argument must not have the first 9 most significant bits all set or all cleared"));
        }

        if (!IsMinimumLengthValid(value))
        {
            throw new BerFormatException(new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(IntegerBerCodec)} failed. A minimum length of {_MinimumByteLength} is required"));
        }
    }

    /// <summary>
    ///     If the contents of an integer value encoding consist of more than one octet, then the bits of the first octet
    ///     and bit 8 of the second octet:
    ///     a) shall not all be set
    ///     b) shall not all be zero
    /// </summary>
    /// <remarks>
    ///     [ITU-T X.690] Section 8.3.2
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    private static bool AreFirstNineBitsValid(ReadOnlySpan<byte> value)
    {
        if ((value[0] == 0xFF) && value[1].IsBitSet(Bits.Eight))
            return false;

        if ((value[0] == 0x00) && !value[1].IsBitSet(Bits.Eight))
            return false;

        return true;
    }

    /// <summary>
    ///     The contents octets shall consist of one or more octets.
    /// </summary>
    /// <remarks>
    ///     [ITU-T X.690] Section 8.3.1
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    private static bool IsMinimumLengthValid(ReadOnlySpan<byte> value)
    {
        if (value.Length < _MinimumByteLength)
            return false;

        return true;
    }

    #endregion

    #region Encode

    public override byte[] Encode<T>(T[] value) => throw new NotImplementedException();
    public override byte[] Encode<T>(T[] value, int length) => throw new NotImplementedException();

    public override byte[] Encode<T>(T value)
    {
        nint byteSize = Unsafe.SizeOf<T>();

        if (byteSize == Specs.Integer.UInt8.ByteCount)
            return Encode(Unsafe.As<T, sbyte>(ref value));
        if (byteSize == Specs.Integer.UInt16.ByteCount)
            return Encode(Unsafe.As<T, short>(ref value));
        if (byteSize == Specs.Integer.UInt32.ByteCount)
            return Encode(Unsafe.As<T, int>(ref value));
        if (byteSize == Specs.Integer.UInt64.ByteCount)
            return Encode(Unsafe.As<T, long>(ref value));

        return Encode(Unsafe.As<T, BigInteger>(ref value));
    }

    // TODO: this is only in BER library so holding off on implementing this
    public override byte[] Encode<T>(T value, int length) => throw new NotImplementedException();

    public byte[] Encode(sbyte value)
    {
        return new[] {(byte) value};
    }

    public byte[] Encode(short value) => BitConverter.GetBytes(value);
    public byte[] Encode(int value) => BitConverter.GetBytes(value);
    public byte[] Encode(long value) => BitConverter.GetBytes(value);
    public byte[] Encode(BigInteger value) => value.ToByteArray();

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
}