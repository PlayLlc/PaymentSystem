using System;

using Play.Ber.Exceptions;
using Play.Ber.Lengths.Long;
using Play.Core.Extensions;

namespace Play.Ber.Lengths.Short;

/// <summary>
///     In the short form, the length octets shall consist of a single octet in which bit 8
///     is zero and bits 7 to 1 encode the number of octets in the contents octets (which may
///     be zero), as an unsigned binary integer with bit 7 as the most significant bit.
/// </summary>
/// <remarks>
///     <see cref="ITUT_X690" /> 8.1.3
/// </remarks>
internal static class ShortLength
{
    #region Static Metadata

    /// <summary>
    ///     In the short form, the length octets shall consist of a single octet
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> 8.1.3.4
    /// </remarks>
    public const byte ByteCount = 1;

    public const byte MaxValue = 127;

    #endregion

    #region Instance Members

    public static ushort GetContentLength(byte value)
    {
        const byte bitEight = (byte) Bits.Eight;

        return value.GetMaskedValue(bitEight);
    }

    public static bool IsValid(uint value) => value <= sbyte.MaxValue;
    public static bool IsValid(int value) => value <= sbyte.MaxValue;
    public static bool IsValid(byte value) => LongLengthBitIsNotSet(value);

    /// <summary>
    ///     The Short Length must have Bit Eight cleared and the content'_Stream Octets must be less than 127.
    ///     Bit Eight being cleared ensures both of these conditions are being met
    /// </summary>
    /// <remarks>
    ///     <see cref="ISO8825.Part1" /> 8.1.3.4
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    private static bool LongLengthBitIsNotSet(byte value) => (value == 0) || !value.AreBitsSet(LongLength.LongLengthFlag);

    public static bool TryGetContentLength(uint value, out ushort result)
    {
        result = (ushort) ((value <= byte.MaxValue) && IsValid(value) ? GetContentLength((byte) value) : 0);

        return result != 0;
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     Serialize
    /// </summary>
    /// <param name="contentOctets"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public static byte Serialize(ReadOnlySpan<byte> contentOctets)
    {
        if (contentOctets.Length > sbyte.MaxValue)
            throw new BerParsingException(new ArgumentOutOfRangeException());

        return (byte) contentOctets.Length;
    }

    /// <summary>
    ///     Serialize
    /// </summary>
    /// <param name="contentOctetLength"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public static byte Serialize(byte contentOctetLength)
    {
        if (contentOctetLength > sbyte.MaxValue)
            throw new BerParsingException(new ArgumentOutOfRangeException());

        return contentOctetLength;
    }

    #endregion
}