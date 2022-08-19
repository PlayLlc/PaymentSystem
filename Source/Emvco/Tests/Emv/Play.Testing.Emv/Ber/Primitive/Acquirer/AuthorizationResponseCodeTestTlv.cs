using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive.Acquirer;

public class AuthorizationResponseCodeTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] {49, 50};

    #endregion

    #region Constructor

    public AuthorizationResponseCodeTestTlv() : base(_DefaultContentOctets)
    { }

    public AuthorizationResponseCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => AuthorizationResponseCode.Tag;
}