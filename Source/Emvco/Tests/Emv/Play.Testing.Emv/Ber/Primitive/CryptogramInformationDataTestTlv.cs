using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CryptogramInformationDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34 };

    public CryptogramInformationDataTestTlv() : base(_DefaultContentOctets) { }

    public CryptogramInformationDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => CryptogramInformationData.Tag;
}
