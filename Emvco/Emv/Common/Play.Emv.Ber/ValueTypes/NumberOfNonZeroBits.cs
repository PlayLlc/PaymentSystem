using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber
{
    /// <summary>
    ///     Number of non-zero bits in PUNATC(Track2) – NATC(Track2)
    /// </summary>
    /// <param name="value"></param>
    /// <remarks>EMV Book C-2 Section A.1.114</remarks>
    public readonly record struct NumberOfNonZeroBits
    {
        #region Instance Values

        private readonly byte _Value;

        #endregion

        #region Constructor

        public NumberOfNonZeroBits(PunatcTrack2 punatc, NumericApplicationTransactionCounterTrack2 natc)
        {
            _Value = (byte) (punatc.GetSetBitCount() - natc.GetSetBitCount());
        }

        #endregion

        /// <summary>
        ///     Validates whether the Track 2 object that initialized this object are in a valid range
        /// </summary>
        public bool IsInRange() => _Value is < 0 or > 8;

        /// <summary>
        ///     Validates whether the Track 1 objects provided in the arguments are valid
        /// </summary>
        public bool IsInRange(PunatcTrack1 punatc, NumericApplicationTransactionCounterTrack1 natc) =>
            IsInRange() && ((punatc.GetSetBitCount() - natc.GetSetBitCount()) != _Value);

        #region Operator Overrides

        public static implicit operator byte(NumberOfNonZeroBits value) => value._Value;
        public static bool operator >(byte left, NumberOfNonZeroBits right) => left > right._Value;
        public static bool operator <(byte left, NumberOfNonZeroBits right) => left < right._Value;
        public static bool operator >=(byte left, NumberOfNonZeroBits right) => left >= right._Value;
        public static bool operator <=(byte left, NumberOfNonZeroBits right) => left <= right._Value;
        public static bool operator ==(byte left, NumberOfNonZeroBits right) => left == right._Value;
        public static bool operator !=(byte left, NumberOfNonZeroBits right) => left != right._Value;
        public static bool operator >(NumberOfNonZeroBits left, byte right) => left._Value > right;
        public static bool operator <(NumberOfNonZeroBits left, byte right) => left._Value < right;
        public static bool operator >=(NumberOfNonZeroBits left, byte right) => left._Value >= right;
        public static bool operator <=(NumberOfNonZeroBits left, byte right) => left._Value <= right;
        public static bool operator ==(NumberOfNonZeroBits left, byte right) => left._Value == right;
        public static bool operator !=(NumberOfNonZeroBits left, byte right) => left._Value != right;

        #endregion
    }
}