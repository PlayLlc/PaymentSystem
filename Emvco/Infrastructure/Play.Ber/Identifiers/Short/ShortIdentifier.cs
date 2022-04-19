using System;

using Play.Ber.Exceptions;
using Play.Core.Extensions;

namespace Play.Ber.Identifiers.Short;

/// <summary>
///     The Short Identifier is a Tag with a number ranging from 0 to 30. Short	Identifiers
///     are comprised of a single byte
/// </summary>
/// <remarks>
///     <see cref="ITUT_X690" /> Section 8.1.2: Identifier Octets
/// </remarks>
internal static class ShortIdentifier
{
    #region Static Metadata

    /// <summary>
    ///     For tags with a number ranging from zero to 30 (inclusive), the identifier octets shall comprise a single octet
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> 8.1.2.2
    /// </remarks>
    public const byte ByteCount = 1;

    #endregion

    #region Instance Members

    /// <summary>
    ///     Returns the <see cref="ClassTypes" /> of the BER-TLV object
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.2.2 a
    /// </remarks>
    /// <param name="value"></param>
    /// <returns cref="ClassTypes">ClassType</returns>
    /// <exception cref="BerParsingException"></exception>
    public static ClassTypes GetClassType(byte value) => (ClassTypes) value.GetMaskedValue(ClassTypes.UnrelatedBits);

    /// <summary>
    ///     Returns the <see cref="DataObjectType" /> type of the BER-TLV Value field
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.2.2 b
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DataObjectType GetDataObject(byte value) => value.IsBitSet(Bits.Six) ? DataObjectType.Constructed : DataObjectType.Primitive;

    /// <summary>
    ///     The Tag Number of this Short Identifier. A number between 0-30
    /// </summary>
    /// <remarks>
    ///     BitCount 5 to 1 shall encode the number of the tag as a binary integer with bit 5 as the most significant bit.
    ///     <see cref="ITUT_X690" /> Section 8.1.2.2 c
    /// </remarks>
    /// <param name="value"></param>
    /// <returns>byte</returns>
    /// <exception cref="BerParsingException"></exception>
    public static byte GetTagNumber(byte value)
    {
        byte tagNumber = value.GetMaskedValue(TagNumber.UnrelatedBits);

        return tagNumber > TagNumber.MaxValue
            ? throw new BerParsingException($"The {nameof(TagNumber)} must be between 0 and 30 for a short Identifier {nameof(Tag)}")
            : tagNumber;
    }

    /// <summary>
    ///     Checks if the Tag Number is between 0 and 30
    /// </summary>
    /// <remarks>
    ///     BitCount 5 to 1 shall encode the number of the tag as a binary integer with bit 5 as the most significant bit
    ///     <see cref="ITUT_X690" /> 8.1.2.2
    /// </remarks>
    private static bool IsTagNumberInRange(byte value)
    {
        byte tagNumber = value.GetMaskedValue(TagNumber.UnrelatedBits);

        return tagNumber <= TagNumber.MaxValue;
    }

    /// <summary>
    ///     Checks if the raw value is a valid Leading Octet
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsValid(uint value)
    {
        if (value > byte.MaxValue)
            return false;

        return IsTagNumberInRange((byte) value);
    }

    public static bool IsValid(ReadOnlySpan<byte> value) => IsValid(value[0]);

    /// <summary>
    ///     Validates that the Tag Number is in the range of a Short Identifier
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="BerParsingException"></exception>
    public static void Validate(byte value)
    {
        if (!IsTagNumberInRange(value))
            throw new BerParsingException($"The {nameof(TagNumber)} must be between 0 and 30 for a short Identifier {nameof(Tag)}");
    }

    #endregion

    public static class TagNumber
    {
        #region Static Metadata

        //
        /// <summary>
        ///     Short Identifier tags will have a number ranging from zero to 30 (inclusive)
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.2.2
        /// </remarks>
        public const byte MaxValue = 30;

        /// <summary>
        ///     The bits that need to be cleared from a Short Identifier tag in order to get the TagNumber value
        /// </summary>
        /// <remarks>
        ///     BitCount 5 to 1 shall encode the number of the tag as a binary integer with bit 5 as the most significant bit
        ///     <see cref="ITUT_X690" /> 8.1.2.2
        /// </remarks>
        public const byte UnrelatedBits = (byte) (Bits.Eight | Bits.Seven | Bits.Six);

        #endregion
    }
}