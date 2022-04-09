namespace Play.Testing.Emv.Infrastructure.Ber.SetOf;

//public abstract class SetOfTlv : TestTlv
//{
//    protected SetOfTlv(TestTlv[] set) : base(ParseSet(set))
//    { }

//    public static byte[] ParseSet(TestTlv[] set)
//    {
//        Span<byte> buffer = new byte[set.Sum(a => a.GetTagLengthValueByteCount())];

//        for (int i = 0, j = 0; i < set.Length; i++)
//        {
//            set[i].EncodeTagLengthValue().CopyTo(buffer[j..]);
//            j += set[i].GetTagLengthValueByteCount();
//        }

//        return buffer.ToArray();
//    }
//}