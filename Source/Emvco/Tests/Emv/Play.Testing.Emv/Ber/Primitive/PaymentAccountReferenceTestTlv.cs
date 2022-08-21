using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv.Ber.Primitive;

public class PaymentAccountReferenceTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContent = Enumerable.Range(0, 29).Select(_ => (byte) Randomize.AlphaNumeric.Char()).ToArray();

    #endregion

    #region Constructor

    public PaymentAccountReferenceTestTlv() : base(_DefaultContent)
    { }

    public PaymentAccountReferenceTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => PaymentAccountReference.Tag;
}