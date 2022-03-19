﻿using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class ApplicationPreferredNameTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x56, 0x49, 0x53, 0x41, 0x20, 0x43, 0x52, 0x45,
        0x44, 0x49, 0x54, 0x4F
    };

    #endregion

    #region Constructor

    public ApplicationPreferredNameTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationPreferredNameTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => ApplicationPreferredName.Tag;

    #endregion
}

//public class Children
//{
//    public class ApplicationDedicatedFileName
//    {
//        public const string ValueHex = "4F07A0000000031010";

//        public static readonly byte[] ValueBytes = { 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10 };

//        public static readonly byte[] TlvBytes = { 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10 };
//    }

//    public class ApplicationPriorityIndicator
//    {
//        public const string ValueHex = "870101";
//        public static readonly byte[] ValueBytes = { 0x01 };

//        public static readonly byte[] TlvBytes = { 0x87, 0x01, 0x01 };
//    }

//    public class KernelIdentifier
//    {
//        public const string ValueHex = "9F2A0103";
//        public static readonly byte[] ValueBytes = { 0x03 };

//        public static readonly byte[] TlvBytes = { 0x9F, 0x2A, 0x01, 0x03 };
//    }
//}

//public abstract class TemplateTestDouble
//{
//    public abstract byte[] EncodeTagLengthValue();
//    public abstract byte[] GetExpectedResult();
//}

//public class DirectoryEntry1 : TemplateTestDouble
//{
//    public override byte[] EncodeTagLengthValue()
//    {
//        return new byte[]
//        {
//            0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10, 0x87, 0x01, 0x01,
//            0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53
//        };
//    }

//    public override byte[] GetExpectedResult()
//    {
//        return new byte[]
//        {
//            0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10, 0x87, 0x01, 0x01,
//            0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53
//        };
//    }
//}

//public class FciIssuerDiscretionaryPpseTestDouble : TemplateTestDouble
//{
//    public override byte[] EncodeTagLengthValue()
//    {
//        return new byte[]
//        {
//            0xBF, 0x0C, 0x38, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10,
//            0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03, 0x40, 0x81, 0x38, 0x5F, 0x55,
//            0x02, 0x55, 0x53, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40,
//            0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03, 0x40, 0x81, 0x38, 0x5F, 0x55,
//            0x02, 0x55, 0x53
//        };
//    }

//    public override byte[] GetExpectedResult()
//    {
//        return new byte[]
//        {
//            0xBF, 0x0C, 0x38, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10,
//            0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00,
//            0x00, 0x98, 0x08, 0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03
//        };
//    }
//}