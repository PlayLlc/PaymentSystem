using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2Session : KernelSession
{
    #region Instance Values

    private CryptogramTypes _ApplicationCryptogramTypes = CryptogramTypes.ApplicationAuthenticationCryptogram;
    private OdaStatusTypes _OdaStatusTypes = OdaStatusTypes.NotAvailable;
    private RrpCounter _RrpCounter = new(0);
    private bool _IsPdolDataMissing = true;

    #endregion

    #region Constructor

    public Kernel2Session(CorrelationId correlationId, KernelSessionId kernelSessionId) : base(correlationId, kernelSessionId)
    { }

    #endregion

    #region Read

    public CryptogramTypes GetAcType() => _ApplicationCryptogramTypes;
    public OdaStatusTypes GetOdaStatus() => _OdaStatusTypes;
    public RrpCounter GetRrpCounter() => _RrpCounter;
    public bool IsPdolDataMissing() => _IsPdolDataMissing;

    #endregion

    #region Write

    public void SetIsPdolDataMissing(bool value) => _IsPdolDataMissing = value;
    public void Update(CryptogramTypes value) => _ApplicationCryptogramTypes = value;
    public void Update(OdaStatusTypes value) => _OdaStatusTypes = value;
    public void Update(RrpCounter value) => _RrpCounter = value;

    #endregion
}