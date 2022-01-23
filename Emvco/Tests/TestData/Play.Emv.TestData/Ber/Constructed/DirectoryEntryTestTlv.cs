﻿using Play.Ber.Identifiers;
using Play.Emv.Templates.FileControlInformation;
using Play.Emv.TestData.Ber.Primitive;

namespace Play.Emv.TestData.Ber.Constructed;

//public class FileControlInformationPpse
//{
//    public const string ValueHex =
//        "6F4D840E325041592E5359532E4444463031A53BBF0C38611A4F07A00000000310108701019F2A010342034081385F55025553611A4F07A00000009808408701029F2A010342034081385F550255539000";

//    public static byte[] ValueBytes =
//    {
//                0x84, 0x0E, 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46,
//                0x30, 0x31, 0xA5, 0x3B, 0xBF, 0x0C, 0x38, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00,
//                0x00, 0x03, 0x10, 0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03, 0x40,
//                0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53, 0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00,
//                0x00, 0x98, 0x08, 0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03, 0x40,
//                0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53, 0x90, 0x00
//            };

//    public static byte[] TlvBytes =
//    {
//                0x6F, 0x4D, 0x84, 0x0E, 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44,
//                0x44, 0x46, 0x30, 0x31, 0xA5, 0x3B, 0xBF, 0x0C, 0x38, 0x61, 0x1A, 0x4F, 0x07, 0xA0,
//                0x00, 0x00, 0x00, 0x03, 0x10, 0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03, 0x42,
//                0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53, 0x61, 0x1A, 0x4F, 0x07, 0xA0,
//                0x00, 0x00, 0x00, 0x98, 0x08, 0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03, 0x42,
//                0x03, 0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53, 0x90, 0x00
//            };

//    public class Children
//    {
//        public class DedicatedFileName
//        {
//            public const string ValuHex = "840E325041592E5359532E4444463031";
//            public static readonly byte[] ValueBytes = { 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31 };

//            public static readonly byte[] TlvBytes =
//            {
//                        0x84, 0x0E, 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46,
//                        0x30, 0x31
//                    };
//        }

//        public class FileControlInformationProprietary
//        {
//            public const string ValuHex = FileControlInformationProprietaryPpse.ValueHex;
//            public static readonly byte[] ValueBytes = FileControlInformationProprietaryPpse.ValueBytes;
//            public static readonly byte[] TlvBytes = FileControlInformationProprietaryPpse.TlvBytes;
//        }
//    }
//}

public class DirectoryEntryTestTlv : ConstructedTlv
{
    #region Static Metadata

    private static readonly TestTlv[] _DefaultChildren = new TestTlv[]
    {
        new ApplicationDedicatedFileNameTestTlv(), new ApplicationPriorityIndicatorTestTlv(), new KernelIdentifierTestTlv()
    };

    private static readonly Tag[] _ChildIndex = DirectoryEntry.ChildTags;

    #endregion

    #region Constructor

    public DirectoryEntryTestTlv() : base(_ChildIndex, _DefaultChildren)
    { }

    public DirectoryEntryTestTlv(TestTlv[] children) : base(_ChildIndex, children)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return DirectoryEntry.Tag;
    }

    #endregion
}