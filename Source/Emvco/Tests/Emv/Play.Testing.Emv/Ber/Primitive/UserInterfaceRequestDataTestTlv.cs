using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class UserInterfaceRequestDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
        12, 33, 101, 44, 15, 73, 134, 88, 33, 10,
        11, 32, 99, 45, 14, 63, 131, 38, 21, 10,
        2, 2 };

    public UserInterfaceRequestDataTestTlv() : base(_DefaultContentOctets) { }

    public UserInterfaceRequestDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => UserInterfaceRequestData.Tag;
}
