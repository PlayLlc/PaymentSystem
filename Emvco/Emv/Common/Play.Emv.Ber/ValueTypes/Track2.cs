using System.Numerics;

namespace Play.Emv.Ber.ValueTypes
{
    public readonly struct Track2
    {
        #region Static Metadata

        private const byte _MaxByteLength = 19;
        private const byte _StartSentinel1 = 0xB;
        private const byte _StartSentinel2 = (byte) ';';
        private const byte _FieldSeparator1 = 0xD;
        private const byte _FieldSeparator2 = (byte) '=';
        private const byte _EndSentinel1 = 0xF;
        private const byte _EndSentinel2 = (byte) '?';

        #endregion

        #region Instance Values

        private readonly BigInteger _Value;

        #endregion
    }
}