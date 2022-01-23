namespace Play.Emv.TestData.Icc.Apdu;

public partial class ApduTestData
{
    public class CApdu
    {
        public class Select
        {
            public class Ppse
            {
                #region Static Metadata

                public static byte[] PpseBytes =
                {
                    0x00, 0xA4, 0x04, 0x00, 0x0E, 0x32, 0x50, 0x41,
                    0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44,
                    0x46, 0x30, 0x31
                };

                public const string PpseHex = "00A404000E325041592E5359532E4444463031";

                #endregion
            }

            public class Applet1
            {
                #region Static Metadata

                public static string DedicatedFileNameHex = "A0000000980840";
                public static byte[] DedicatedFileName = {0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40};

                public static byte[] CApdu =
                {
                    0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x00, 0x00,
                    0x00, 0x98, 0x08, 0x40
                };

                #endregion
            }

            public class Applet2
            {
                #region Static Metadata

                public static byte[] DedicatedFileName = {0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10};

                public static byte[] CApdu =
                {
                    0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x00, 0x00,
                    0x00, 0x03, 0x10, 0x10
                };

                public const string DedicatedFileNameHex = "A0000000031010";

                #endregion
            }
        }
    }
}