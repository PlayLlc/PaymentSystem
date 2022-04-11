using System;

using Play.Ber.Exceptions;
using Play.Codecs;
using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Ber.Identifiers.Long;

internal static partial class LongIdentifier
{
    /// <summary>
    ///     The Leading Octet is a one byte value that holds metadata relating to the Tag and
    ///     the BER-TLV Data Object as a whole.
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.2.4.1
    /// </remarks>
    private static class LeadingOctet
    {
        #region Instance Members

        /// <summary>
        ///     Returns the <see cref="ClassType" /> of the BER-TLV object
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.2.4.1 a
        /// </remarks>
        /// <param name="value"></param>
        /// <returns cref="ClassType">ClassType</returns>
        /// <exception cref="BerParsingException"></exception>
        public static ClassType GetClassType(byte value) => (ClassType) value.GetMaskedValue(ClassType.UnrelatedBits);

        /// <summary>
        ///     Returns the <see cref="DataObjectType" /> type of the BER-TLV Value field
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.2.4.1 b
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataObjectType GetDataObjectType(byte value) =>
            value.IsBitSet(Bits.Six) ? DataObjectType.Constructed : DataObjectType.Primitive;

        /// <summary>
        ///     Validates that the Long Identifier Flag is present for the byte
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.2.4.1 c
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>bool</returns>
        private static bool IsLongIdentifierFlagPresent(byte value) => value.AreBitsSet(LongIdentifierFlag);

        /// <summary>
        ///     Checks if the raw value is a valid Leading Octet
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(ReadOnlySpan<byte> value)
        {
            CheckCore.ForExactLength(value, 1, nameof(value));

            return IsValid(value[0]);
        }

        public static bool IsValid(byte value) => IsLongIdentifierFlagPresent(value);

        /// <exception cref="BerParsingException"></exception>
        public static void Validate(byte value)
        {
            if (!IsLongIdentifierFlagPresent(value))
            {
                throw new BerParsingException(
                    $"The {nameof(Tag)} could not be initialized because the argument {PlayCodec.BinaryCodec.DecodeToString(value)} contained an invalid format",
                    new ArgumentOutOfRangeException(nameof(value)));
            }
        }

        #endregion
    }
}