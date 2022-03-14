using System;

using Play.Ber.Exceptions;
using Play.Core.Extensions;

namespace Play.Ber.Identifiers.Long;

/// <summary>
///     For tags with a number greater than or equal to 31, the identifier shall comprise
///     a leading octet followed by one or more subsequent octets.
/// </summary>
/// ///
/// <remarks>
///     <see cref="ITUT_X690" /> 8.1.2.4
/// </remarks>
internal static partial class LongIdentifier
{
    #region Static Metadata

    /// <summary>
    ///     Leading Octet shall have BitCount 5 to 1 of the encoded as 11111.
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> 8.1.2.4.1 c
    /// </remarks>
    public const byte LongIdentifierFlag = (byte) (Bits.Five | Bits.Four | Bits.Three | Bits.Two | Bits.One);

    public const byte LongIdentifierFlagMask = (byte) (Bits.Eight | Bits.Seven | Bits.Six);

    /// <summary>
    ///     This is NOT a specification limitation. This is a limitation introduced purposefully by this code base.
    ///     This value may change in the future to support larger tags
    /// </summary>
    public const byte MaxLongIdentifierByteCount = 3;

    #endregion

    #region Instance Members

    public static byte GetByteCount(uint value) => value.GetMostSignificantByte();

    /// <exception cref="BerParsingException"></exception>
    public static byte GetByteCount(ReadOnlySpan<byte> value) =>

        // Adding 1 because Indexes are 0 based, and also to include the byte for the Initial Octet
        (byte) (SubsequentOctets.FindLastSubsequentOctetIndex(value) + 1);

    /// <summary>
    ///     The GetClassType of the Tag
    /// </summary>
    /// <remarks><see cref="ITUT_X690" /> Section 8.1.2.2 a</remarks>
    public static ClassType GetClassType(uint value) => LeadingOctet.GetClassType(GetLeadingOctet(value));

    /// <summary>
    ///     Represents the Data Object type in the Value field of the BER-TLV object
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.2.2 b
    /// </remarks>
    public static DataObjectType GetDataObject(uint value) => LeadingOctet.GetDataObjectType(GetLeadingOctet(value));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static byte GetLeadingOctet(uint value)
    {
        byte initialOctet = ShiftBitsToGetLeadingOctet(GetByteCount(value));

        LeadingOctet.Validate(initialOctet);

        return initialOctet;

        byte ShiftBitsToGetLeadingOctet(byte byteCount)
        {
            const byte octetBitCount = 8;

            try
            {
                return checked((byte) (value >> ((byteCount * octetBitCount) - octetBitCount)));
            }
            catch (OverflowException exception)
            {
                throw new
                    InvalidOperationException("Uh oh, something wasn't coded correctly. This code base only supports a Long Length object with 3 bytes or less",
                                              exception);
            }
        }
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    /// <exception cref="BerParsingException"></exception>
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
                throw new
                    BerParsingException($"This code base only supports a Long Length object with {MaxLongIdentifierByteCount} bytes or less",
                                        exception);
            }
        }
    }

    /// <summary>
    ///     The Tag Number of this long Identifier
    /// </summary>
    /// <remarks><see cref="ITUT_X690" /> Section 8.1.2.2 c</remarks>
    public static ushort GetTagNumber(uint value) => SubsequentOctets.GetTagNumber(GetSubsequentOctets(value));

    internal static bool IsValid(ReadOnlySpan<byte> value) => value[0].AreBitsSet(LongIdentifierFlag);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static bool IsValid(uint value) =>
        LeadingOctet.IsValid(GetLeadingOctet(value)) && SubsequentOctets.IsValid(GetSubsequentOctets(value));

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static void Validate(uint value)
    {
        LeadingOctet.Validate(GetLeadingOctet(value));
        SubsequentOctets.Validate(GetSubsequentOctets(value));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Core.Exceptions.PlayInternalException"></exception>
    public static void Validate(ReadOnlySpan<byte> value)
    {
        LeadingOctet.Validate(value[0]);
        SubsequentOctets.Validate(value[^SubsequentOctets.FindLastSubsequentOctetIndex(value)..]);
    }

    #endregion
}