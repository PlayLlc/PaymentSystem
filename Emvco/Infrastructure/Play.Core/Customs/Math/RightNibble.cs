using System;

using Play.Core.Extensions;

namespace Play.Core
{
    internal readonly record struct RightNibble
    {
        #region Static Metadata

        private const byte _UnrelatedBits = 0xF0;

        #endregion

        #region Instance Values

        private readonly byte _Value;

        #endregion

        #region Constructor

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public RightNibble(byte value)
        {
            if (value.AreBitsSet(_UnrelatedBits))
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                                                      $"The {nameof(RightNibble)} could not be initialized because the argument provided has more bits set than a nibble");
            }

            _Value = value;
        }

        public RightNibble(Nibble value)
        {
            _Value = value;
        }

        #endregion

        #region Operator Overrides

        public static bool operator ==(byte left, RightNibble right) => left.GetMaskedValue(_UnrelatedBits) == right._Value;
        public static bool operator !=(byte left, RightNibble right) => left.GetMaskedValue(_UnrelatedBits) != right._Value;
        public static bool operator <(byte left, RightNibble right) => left.GetMaskedValue(_UnrelatedBits) < right._Value;
        public static bool operator >(byte left, RightNibble right) => left.GetMaskedValue(_UnrelatedBits) > right._Value;
        public static bool operator <=(byte left, RightNibble right) => left.GetMaskedValue(_UnrelatedBits) <= right._Value;
        public static bool operator >=(byte left, RightNibble right) => left.GetMaskedValue(_UnrelatedBits) >= right._Value;
        public static bool operator ==(RightNibble left, byte right) => left._Value == right.GetMaskedValue(_UnrelatedBits);
        public static bool operator !=(RightNibble left, byte right) => left._Value != right.GetMaskedValue(_UnrelatedBits);
        public static bool operator <(RightNibble left, byte right) => left._Value < right.GetMaskedValue(_UnrelatedBits);
        public static bool operator >(RightNibble left, byte right) => left._Value > right.GetMaskedValue(_UnrelatedBits);
        public static bool operator <=(RightNibble left, byte right) => left._Value <= right.GetMaskedValue(_UnrelatedBits);
        public static bool operator >=(RightNibble left, byte right) => left._Value >= right.GetMaskedValue(_UnrelatedBits);

        #endregion
    }
}