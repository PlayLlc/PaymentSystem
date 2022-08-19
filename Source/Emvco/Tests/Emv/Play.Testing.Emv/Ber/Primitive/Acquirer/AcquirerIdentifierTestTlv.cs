using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive.Acquirer;
public class AcquirerIdentifierTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 6, 13, 22, 8, 11, 22 };

    public AcquirerIdentifierTestTlv() : base(_DefaultContentOctets) { }

    public AcquirerIdentifierTestTlv(byte[] contentOctets) : base(contentOctets) { }

    public override Tag GetTag() => AcquirerIdentifier.Tag;
}
