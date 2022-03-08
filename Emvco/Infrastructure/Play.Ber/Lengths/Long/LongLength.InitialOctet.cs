using System;

using Play.Ber.Exceptions;
using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Ber.Lengths.Long;

internal static partial class LongLength
{
    /// <summary>
    ///     The Initial Octet is a one byte value that holds metadata relating to the Length
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> Section 8.1.3.5
    /// </remarks>
    private static class InitialOctet
    {
        #region Static Metadata

        /// <summary>
        ///     The Byte Count length required for the Initial Octet
        /// </summary>
        /// <remarks>
        ///     In the long form, the length octets shall consist of an initial octet
        ///     <see cref="ITUT_X690" /> Section 8.1.3.5
        /// </remarks>
        public const byte ByteCount = 1;

        /// <summary>
        ///     The value 0xFF shall not be used.
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> Section 8.1.3.5 c
        /// </remarks>
        public const byte InvalidRawValue = 0xFF;

        #endregion

        #region Instance Members

        public static byte GetSubsequentOctetCount(in byte value) => (byte) (value & ~LongLengthFlag);

        /// <summary>
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.3.5 a
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool InitialOctetHasBitEightSet(byte value) => value.AreBitsSet(LongLengthFlag);

        /// <summary>
        ///     The value 0xFF shall not be used.
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.3.5 c
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool RawValueDoesNotHaveAllBitsSet(byte value) => value != InvalidRawValue;

        /// <summary>
        ///     A long Length shall consist of one or more subsequent octets
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> 8.1.3.5 c
        /// </remarks>
        private static bool SubsequentOctetCountIsNotZero(byte value) => value.GetMaskedValue(Bits.Eight) >= SubsequentOctets.MinByteCount;

        /// <summary>
        ///     This is NOT a specification limitation. This is a limitation introduced purposefully by this code base.
        ///     This value may change in the future to support larger tags
        /// </summary>
        private static bool SubsequentOctetCountIsSupportedByThisCodeBase(byte value) =>
            value.GetMaskedValue(Bits.Eight) <= SubsequentOctets.MaxByteCount;

        /// <summary>
        ///     Validate
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="BerException"></exception>
        public static void Validate(ReadOnlySpan<byte> value)
        {
            CheckCore.ForExactLength(value, 1, nameof(value));

            Validate(value[0]);
        }

        /// <exception cref="BerException"></exception>
        /// <exception cref="BerInternalException">Ignore.</exception>
        public static void Validate(byte value)
        {
            if (!InitialOctetHasBitEightSet(value))
                throw new BerFormatException(new ArgumentOutOfRangeException(nameof(value)));

            if (!RawValueDoesNotHaveAllBitsSet(value))
                throw new BerFormatException(new ArgumentOutOfRangeException(nameof(value)));

            if (!SubsequentOctetCountIsNotZero(value))
                throw new BerFormatException($"The Subsequent Octet Count can not be zero in the context of a {nameof(LongLength)}");

            if (!SubsequentOctetCountIsSupportedByThisCodeBase(value))
            {
                throw new BerInternalException(
                    "This is embarrassing. Our code base doesn't support the amount of bytes needed to support the "
                    + $"{nameof(SubsequentOctets)} for the requested {nameof(LongLength)} object");
            }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="contentOctets"></param>
        /// <returns></returns>
        /// <exception cref="BerFormatException"></exception>
        public static int Serialize(ReadOnlySpan<byte> contentOctets)
        {
            if (contentOctets.Length > sbyte.MaxValue)
                throw new BerFormatException("This code base only supports content octets that are 127 bytes in length");

            return contentOctets.Length | LongLengthFlag;
        }

        #endregion
    }
}