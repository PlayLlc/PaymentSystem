using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PhoneMessageTableTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] { };

    #endregion

    #region Constructor

    public PhoneMessageTableTestTlv() : base(_DefaultContentOctets)
    { }

    public PhoneMessageTableTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => PhoneMessageTable.Tag;
}