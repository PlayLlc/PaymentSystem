namespace Play.Emv.TestData.Encoding;

public partial class EncodingTestDataFactory
{
    public class StrictAscii
    {
        #region Static Metadata

        public static byte[] ApplicationLabelBytes =
        {
            0x56, 0x49, 0x53, 0x41, 0x20, 0x50, 0x52, 0x45,
            0x50, 0x41, 0x49, 0x44
        };

        public const string ApplicationLabelAscii = "VISA PREPAID";

        #endregion
    }

    public class Numeric
    {
        #region Static Metadata

        public static byte[] ValueBytes = {1, 23, 45};
        public static byte[] TlvBytes = {1, 23, 45};
        public const ushort ValueUshort = 12345;
        public const uint ValueUint = 12345;
        public const ulong ValueUlong = 12345;
        public const string ValueHex = "012345";

        #endregion
    }
}