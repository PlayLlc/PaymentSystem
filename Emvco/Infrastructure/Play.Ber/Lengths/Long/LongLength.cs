using System;

using Play.Ber.Exceptions;
using Play.Core.Extensions;

namespace Play.Ber.Lengths.Long;

internal static partial class LongLength
{
    #region Static Metadata

    /// <summary>
    ///     Bit 8 shall be set
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.3.5 a
    /// </remarks>
    public const byte LongLengthFlag = (byte) Bits.Eight;

    /// <summary>
    ///     The maximum length supported in this code base
    /// </summary>
    public const ushort MaxLengthSupported = ushort.MaxValue;

    #endregion

    #region Instance Members

    public static byte GetByteCount(in uint value)
    {
        const byte octetBitCount = 8;
        byte mostSignificantBit = (byte) (value.GetMostSignificantBit() - octetBitCount);

        return mostSignificantBit.TryGetRemainder(octetBitCount, out byte resultWithoutRemainder) == 0
            ? resultWithoutRemainder
            : (byte) (resultWithoutRemainder + 1);
    }

    /// <exception cref="BerException"></exception>
    public static byte GetByteCount(ReadOnlySpan<byte> value) => (byte) (InitialOctet.GetSubsequentOctetCount(value[0]) + 1);

    public static ushort GetContentLength(in uint value) => GetSubsequentOctets(value);

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static byte GetInitialOctet(uint value)
    {
        byte initialOctet = ShiftBitsToGetInitialOctet(GetByteCount(value));

        InitialOctet.Validate(initialOctet);

        return initialOctet;

        byte ShiftBitsToGetInitialOctet(byte byteCount)
        {
            const byte octetBitCount = 8;

            try
            {
                return checked((byte) (value >> ((byteCount * octetBitCount) - octetBitCount)));
            }
            catch (OverflowException exception)
            {
                throw new BerInternalException(
                    "Uh oh, something wasn't coded correctly. This code base only supports a Long Length object with 3 bytes or less",
                    exception);
            }
        }
    }

    /// <summary>
    ///     returns the subsequent octets raw value as a ushort
    /// </summary>
    /// <param name="value"></param>
    /// <returns>The raw value of the Subsequent Octets as a <see cref="ushort" /></returns>
    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    private static ushort GetSubsequentOctets(uint value)
    {
        ushort subsequentOctets = (ushort) value.GetMaskedValue(GetSubsequentOctetMask(GetByteCount(value)));

        SubsequentOctets.Validate(subsequentOctets);

        return subsequentOctets;

        uint GetSubsequentOctetMask(byte byteCount)
        {
            const byte octetBitCount = 8;

            try
            {
                return checked((uint) (byte.MaxValue << ((byteCount * octetBitCount) - octetBitCount)));
            }
            catch (OverflowException exception)
            {
                throw new BerInternalException(
                    "Uh oh, something wasn't coded correctly. This code base only supports a Long Length object with 3 bytes or less",
                    exception);
            }
        }
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    public static void Validate(ReadOnlySpan<byte> value)
    {
        InitialOctet.Validate(value[..1]);
        SubsequentOctets.Validate(value[1..]);
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    public static void Validate(uint value)
    {
        InitialOctet.Validate(GetInitialOctet(value));
        SubsequentOctets.Validate(GetSubsequentOctets(value));
    }

    #endregion

    #region Serialization

    public static byte[] Serialize(ushort contentOctetsLength)
    {
        if (contentOctetsLength <= byte.MaxValue)
        {
            const byte initialOctetForSingleSubsequentOctet = 0b10000001;
            byte[] singleSubsequentOctetsResult = new byte[2];
            singleSubsequentOctetsResult[0] = initialOctetForSingleSubsequentOctet;
            singleSubsequentOctetsResult[1] = (byte) contentOctetsLength;

            return singleSubsequentOctetsResult;
        }

        const byte initialOctetForTwoSubsequentOctets = 0b10000010;
        byte[] twoSubsequentOctetsResult = new byte[3];
        twoSubsequentOctetsResult[0] = initialOctetForTwoSubsequentOctets;
        twoSubsequentOctetsResult[1] = (byte) contentOctetsLength;
        twoSubsequentOctetsResult[2] = (byte) (contentOctetsLength >> 8);

        return twoSubsequentOctetsResult;
    }

    public static byte[] Serialize(ReadOnlySpan<byte> contentOctets)
    {
        if (contentOctets.Length <= byte.MaxValue)
        {
            const byte initialOctetForSingleSubsequentOctet = 0b10000001;
            byte[] singleSubsequentOctetsResult = new byte[2];
            singleSubsequentOctetsResult[0] = initialOctetForSingleSubsequentOctet;
            singleSubsequentOctetsResult[1] = (byte) contentOctets.Length;

            return singleSubsequentOctetsResult;
        }

        const byte initialOctetForTwoSubsequentOctets = 0b10000010;
        byte[] twoSubsequentOctetsResult = new byte[3];
        twoSubsequentOctetsResult[0] = initialOctetForTwoSubsequentOctets;
        twoSubsequentOctetsResult[1] = (byte) contentOctets.Length;
        twoSubsequentOctetsResult[2] = (byte) (contentOctets.Length >> 8);

        return twoSubsequentOctetsResult;
    }

    #endregion
}