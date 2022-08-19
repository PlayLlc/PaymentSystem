using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class AccountTypeTestTlv : TestTlv
{
    #region Instance Values

    #region StaticMetadata

    public byte _DefaultContent = 0x00;

    #endregion

    #endregion

    #region Constructor

    public AccountTypeTestTlv(byte contentOctets) : base(new byte[] {contentOctets})
    { }

    public AccountTypeTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => AccountType.Tag;

    #endregion
}