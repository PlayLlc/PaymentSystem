using Play.Core;

namespace Play.Emv.Ber
{
    public readonly record struct TrackServiceCode
    {
        #region Static Metadata

        public const byte MaxNumberOfDigits = 3;

        #endregion

        #region Instance Values

        private readonly Nibble[] _Value;

        #endregion

        #region Constructor

        public TrackServiceCode(ReadOnlySpan<Nibble> value)
        {
            _Value = value.ToArray();
        }

        #endregion

        #region Instance Members

        public int GetCharCount() => _Value.Length;
        internal ReadOnlySpan<Nibble> AsNibbleArray() => _Value;

        #endregion
    }
}