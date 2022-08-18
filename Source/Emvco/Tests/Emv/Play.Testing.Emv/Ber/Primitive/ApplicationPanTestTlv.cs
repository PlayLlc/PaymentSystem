using Play.Ber.Tags;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationPanTestTlv : TestTlv
{
    #region Static Metadata

    public static readonly TrackPrimaryAccountNumber _DefaultContent = new(new Nibble[]
    {
        0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x0,
        0x1, 0x2, 0x3
    });

    #endregion

    #region Constructor

    public ApplicationPanTestTlv() : base(_DefaultContent.Encode())
    { }

    public ApplicationPanTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationPan.Tag;
}