using Play.Emv.DataElements;

namespace Play.Emv.Kernel2.Configuration;

public class Kernel2SessionConfiguration
{
    #region Instance Values

    private AcType _AcType = AcType.Aac;
    private OdaStatus _OdaStatus = OdaStatus.NotAvailable;
    private RrpCounter _RrpCounter = new(0);

    #endregion

    #region Instance Members

    public void Update(AcType value) => _AcType = value;
    public void Update(OdaStatus value) => _OdaStatus = value;
    public void Update(RrpCounter value) => _RrpCounter = value;

    #endregion
}