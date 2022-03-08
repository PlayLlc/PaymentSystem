using System;

using Play.Ber.Exceptions;

namespace Play.Ber.Lengths.Long;

internal static partial class LongLength
{
    /// <summary>
    ///     The Long Length form has an Initial Octet, followed by Subsequent Octets. The Subsequent Octets hold
    ///     the length of TLV'_Stream Value field
    /// </summary>
    /// <remarks>
    ///     <see cref="ITUT_X690" /> 8.1.3.5 Note 1
    /// </remarks>
    private static class SubsequentOctets
    {
        #region Static Metadata

        /// <summary>
        ///     This is NOT a specification limitation. This is a limitation introduced purposefully by this code base.
        ///     This value may change in the future to support larger tags
        /// </summary>
        public const byte MaxByteCount = 2;

        /// <summary>
        ///     In the long form, the length octets shall have one or more Subsequent Octets.
        /// </summary>
        /// <remarks>
        ///     <see cref="ITUT_X690" /> Section 8.1.3.5
        /// </remarks>
        public const byte MinByteCount = 1;

        #endregion

        #region Instance Members

        // This is outside the scope of <see cref="ISO8825.Part1"/> and does not conform with <see cref="ISO8825.Part1"/>. There is no max
        // byte length specified in <see cref="ISO8825.Part1"/> so we are using the max byte length specified in [ISO7816-4]
        private static bool IsLengthSupportedInThisCodeBase(ReadOnlySpan<byte> value) => value.Length <= MaxByteCount;

        // This is outside the scope of <see cref="ISO8825.Part1"/> and does not conform with <see cref="ISO8825.Part1"/>. There is no max
        // byte length specified in <see cref="ISO8825.Part1"/> so we are using the max byte length specified in [ISO7816-4]
        private static bool IsLengthSupportedInThisCodeBase(uint value) => value <= MaxByteCount;
        public static bool IsValid(ReadOnlySpan<byte> value) => IsLengthSupportedInThisCodeBase(value);

        /// <summary>
        ///     Validate
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
        public static void Validate(ReadOnlySpan<byte> value)
        {
            if (!IsLengthSupportedInThisCodeBase(value))
            {
                throw new BerParsingException(
                    "This is embarrassing. The length of the TLV content is outside of what is currently supported in this code base");
            }
        }

        /// <summary>
        ///     Validate
        /// </summary>
        /// <param name="value"></param>
        
        public static void Validate(in uint value)
        {
            if (!IsLengthSupportedInThisCodeBase(value))
            {
                throw new BerParsingException(
                    $"This code base currently only supports a long Length with Subsequent Octets of {MaxByteCount} bytes or less");
            }
        }

        #endregion
    }
}