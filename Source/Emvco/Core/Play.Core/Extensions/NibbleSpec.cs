namespace Play.Core.Extensions;

public static class NibbleSpec
{
    #region Static Metadata

    public const byte BitsInANibble = 4;

    #endregion

    public static class LeftNibble
    {
        #region Static Metadata

        public const byte MostSignificantBit = ByteSpec.MostSignificantBit;
        public const byte LeastSignificantBit = ByteSpec.Five;

        #endregion
    }

    public static class RightNibble
    {
        #region Static Metadata

        public const byte MostSignificantBit = ByteSpec.Four;
        public const byte LeastSignificantBit = ByteSpec.One;

        #endregion
    }
}