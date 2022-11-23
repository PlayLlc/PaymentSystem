namespace Play.Core.Extensions;

public static class NibbleSpec
{
    #region Static Metadata

    public const byte _BitsInANibble = 4;

    #endregion

    public static class LeftNibble
    {
        #region Static Metadata

        public const byte _MostSignificantBit = ByteSpec._MostSignificantBit;
        public const byte _LeastSignificantBit = ByteSpec._Five;

        #endregion
    }

    public static class RightNibble
    {
        #region Static Metadata

        public const byte _MostSignificantBit = ByteSpec._Four;
        public const byte _LeastSignificantBit = ByteSpec._One;

        #endregion
    }
}