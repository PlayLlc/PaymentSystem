using System;

using Play.Ber.Exceptions;
using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Ber.Identifiers.Long;

internal static partial class LongIdentifier
{
    /// <summary>
    ///     For tags with a number greater than or equal to 31, the identifier shall comprise a leading
    ///     octet followed by one or more subsequent octets.
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> 8.1.2.4.2
    /// </remarks>
    private static class SubsequentOctets
    {
        #region Static Metadata

        /// <summary>
        ///     This is NOT a specification limitation. This is a limitation introduced purposefully by this code base.
        ///     This value may change in the future to support larger tags
        /// </summary>
        private const byte _MaxBytesAllowedToInitialize = 2;

        #endregion

        #region Instance Members

        /// <summary>
        ///     bit 8 of each octet shall be set to one unless it is the last octet of the identifier octets
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.2.4.2 a
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="BerFormatException"></exception>
        private static bool BitsAreSetCorrectly(ReadOnlySpan<byte> value)
        {
            const Bits bitEight = Bits.Eight;

            if (value.Length > _MaxBytesAllowedToInitialize)
            {
                throw new BerFormatException(new ArgumentOutOfRangeException(nameof(value),
                    $"The argument supplied surpassed the maximum amount of subsequent octets allowed of {_MaxBytesAllowedToInitialize}"));
            }

            for (int i = 0; i < (value.Length - 1); i++)
            {
                if (!value[i].IsBitSet(bitEight))
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     bit 8 of each octet shall be set to one unless it is the last octet of the identifier octets
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.2.4.2 a
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool BitsAreSetCorrectly(ushort value)
        {
            ushort tempValue = value;

            if (((byte) tempValue).IsBitSet(Bits.Eight))
                return false;

            tempValue >>= 8;

            while (tempValue > 0)
            {
                if (!((byte) tempValue).IsBitSet(Bits.Eight))
                    return false;

                tempValue >>= 8;
            }

            return true;
        }

        /// <summary>
        ///     Finds the first instance of a Last Subsequent Octet in the supplied sequence
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="BerException"></exception>
        public static byte FindLastSubsequentOctetIndex(ReadOnlySpan<byte> value)
        {
            if (value.Length == 1)
            {
                return (byte) (value[0].IsBitSet(Bits.Eight)
                    ? throw new BerException("The last subsequent octet must have bit eight cleared",
                        new ArgumentOutOfRangeException(nameof(value)))
                    : 0);
            }

            // The _MaxBytesAllowedToInitialize is not compliant with ISO8825-1 but if a Tag needs more than
            // 255 bytes... then that tag is a selfish son of a bitch, and that'_Stream how I'm #weedingThingsOutOfMyLife
            for (int i = 0; (i < value.Length) || (i > _MaxBytesAllowedToInitialize); i++)
            {
                if ((value[i].GetMaskedValue(LongIdentifierFlagMask) != LongIdentifierFlag) && !value[i].IsBitSet(Bits.Eight))
                    return (byte) i;
            }

            throw new BerFormatException(
                "Could not find the last Subsequent Octet. Either the raw value was in an invalid format or the tag number is too large for this code base");
        }

        /// <summary>
        ///     The Tag Number of this long Identifier
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> Section 8.1.2.2 c
        ///     This method is agnostic to the argument type so we can still use this when we expand the amount of
        ///     bytes we allow the tag to hold in this code base
        /// </remarks>
        /// <returns></returns>
        public static ushort GetTagNumber(ushort value)
        {
            const byte bitsToMask = (byte) Bits.Eight;
            ushort tempValue = value;
            ushort result = (byte) tempValue;
            tempValue >>= 8;
            int bitShiftValue = 8;

            while (tempValue > 0)
            {
                byte currentByte = (byte) tempValue;
                result &= (ushort) (currentByte.GetMaskedValue(bitsToMask) << bitShiftValue);
                tempValue >>= 8;
                bitShiftValue += 7;
            }

            return result;
        }

        public static bool IsValid(ushort value)
        {
            if (!TagByteCountIsInSupportedRange(value))
                return false;

            if (!LastOctetIsClearedCorrectly(value))
                return false;

            if (!BitsAreSetCorrectly(value))
                return false;

            return true;
        }

        /// <summary>
        ///     The most significant bit of the last Subsequent Octet must not be set
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.2.4.2 a
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool LastOctetIsClearedCorrectly(ReadOnlySpan<byte> value) => !value[^1].IsBitSet(Bits.Eight);

        /// <summary>
        ///     The most significant bit of the last Subsequent Octet must not be set
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.2.4.2 a
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool LastOctetIsClearedCorrectly(ushort value) => !((byte) value).AreBitsSet(BitLookup.GetBit(Bits.Eight));

        /// <summary>
        ///     This is not consistent with <see cref="ITUT_X690" /> specifications. <see cref="ITUT_X690" /> doesn't
        ///     specify a maximum byte count for a Tag, but for now this code base only supports a Tag with 3 bytes. So that
        ///     means the max amount of Subsequent Octets would be 2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool TagByteCountIsInSupportedRange(ReadOnlySpan<byte> value) => value.Length <= _MaxBytesAllowedToInitialize;

        private static bool TagByteCountIsInSupportedRange(ushort value) => true; // a ushort is always going to have 2 bytes

        /// <summary>
        ///     Validate
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="BerInternalException"></exception>
        /// <exception cref="BerFormatException"></exception>
        public static void Validate(ReadOnlySpan<byte> value)
        {
            CheckCore.ForEmptySequence(value, nameof(value));

            if (!TagByteCountIsInSupportedRange(value))
            {
                throw new BerInternalException(
                    $"This code base only support a Subsequent Octet with a byte count of {_MaxBytesAllowedToInitialize}");
            }

            if (!LastOctetIsClearedCorrectly(value))
            {
                throw new BerFormatException(new ArgumentOutOfRangeException(
                    $"The {nameof(SubsequentOctets)} could not be validated. The most significant bit of the last Subsequent Octet must not be set"));
            }

            if (!BitsAreSetCorrectly(value))
            {
                throw new BerFormatException(new ArgumentOutOfRangeException(
                    $"The {nameof(SubsequentOctets)} could not be validated. The Subsequent Octets of a Long Identifier must not have bit 8 of the first Subsequent Octet set"));
            }
        }

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="PlayInternalException">Ignore.</exception>
        /// <exception cref="BerFormatException"></exception>
        public static void Validate(ushort value)
        {
            if (!TagByteCountIsInSupportedRange(value))
            {
                throw new BerFormatException(new PlayInternalException(
                    $"This code base only support a Subsequent Octet with a byte count of {_MaxBytesAllowedToInitialize}"));
            }

            if (!LastOctetIsClearedCorrectly(value))
            {
                throw new BerFormatException(new ArgumentOutOfRangeException(
                    $"The {nameof(SubsequentOctets)} could not be validated. The most significant bit of the last Subsequent Octet must not be set"));
            }

            if (!BitsAreSetCorrectly(value))
            {
                throw new ArgumentOutOfRangeException(
                    $"The {nameof(SubsequentOctets)} could not be validated. The Subsequent Octets of a Long Identifier must not have bit 8 of the first Subsequent Octet set");
            }
        }

        #endregion
    }
}