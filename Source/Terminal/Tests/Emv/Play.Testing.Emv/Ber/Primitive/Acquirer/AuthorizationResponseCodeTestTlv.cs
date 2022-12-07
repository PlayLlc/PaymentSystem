using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive.Acquirer;

public class AuthorizationResponseCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = new byte[] { 49, 50 };

    public AuthorizationResponseCodeTestTlv(): base(_DefaultContentOctets) { }

    public AuthorizationResponseCodeTestTlv(byte[] contentOctets) : base(contentOctets) { }

    public override Tag GetTag() => AuthorizationResponseCode.Tag;
}
