using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationLabelTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x56, 0x49, 0x53, 0x41, 0x20, 0x50, 0x52, 0x45,
        0x50, 0x41, 0x49, 0x44
    };

    #endregion

    #region Constructor

    public ApplicationLabelTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationLabelTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static byte[] GetDefaultAsBytes() => _DefaultContentOctets;
    public static string GetDefaultAsString() => "VISA PREPAID";
    public override Tag GetTag() => ApplicationLabel.Tag;

    #endregion
}