using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ProcessingOptionsDataObjectListRelatedDataTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        54, 23, 16, 24, 48, 31, 12, 46, 28, 25,
        36, 14
    };

    #endregion

    #region Constructor

    public ProcessingOptionsDataObjectListRelatedDataTestTlv() : base(_DefaultContentOctets)
    { }

    public ProcessingOptionsDataObjectListRelatedDataTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ProcessingOptionsDataObjectListRelatedData.Tag;
}