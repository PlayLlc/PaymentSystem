namespace Play.Testing.Emv.Ber.SetOf;

//public class SetOfDirectoryEntryTestTlv : SetOfTlv
//{
//    public static readonly TestTlv[] _DefaultSetOf = new TestTlv[]
//    {
//        new DirectoryEntryTestTlv(),
//        new DirectoryEntryTestTlv(new TestTlv[]
//        {
//            new ApplicationDedicatedFileNameTestTlv(new byte[] {0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40}),
//            new ApplicationPriorityIndicatorTestTlv(new byte[] {0x02}), new KernelIdentifierTestTlv(new byte[] {0x03})
//        })
//    };

//    public SetOfDirectoryEntryTestTlv() : base(_DefaultSetOf)
//    { }

//    public SetOfDirectoryEntryTestTlv(TestTlv[] set) : base(set)
//    { }

//    public override Tag GetTag()
//    {
//        return DirectoryEntry.Tag;
//    }
//}