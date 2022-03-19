using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.State;
using Play.Messaging;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2Session : KernelSession
{
    #region Instance Values

    private CryptogramTypes _ApplicationCryptogramTypes = CryptogramTypes.ApplicationAuthenticationCryptogram;
    private OdaStatusTypes _OdaStatusTypes = OdaStatusTypes.NotAvailable; 
    private ReaderContactlessTransactionLimit? _TransactionLimit;
    private bool _IsPdolDataMissing = true;
    private byte _RelayResistanceProtocolCount = 0;

    #endregion

    #region Constructor

    public Kernel2Session(CorrelationId correlationId, KernelSessionId kernelSessionId) : base(correlationId, kernelSessionId)
    { }

    #endregion

    #region Read

    public byte GetRelayResistanceProtocolCount() => _RelayResistanceProtocolCount;
    public CryptogramTypes GetAcType() => _ApplicationCryptogramTypes;
    public OdaStatusTypes GetOdaStatus() => _OdaStatusTypes; 
    public bool IsPdolDataMissing() => _IsPdolDataMissing;

    public bool TryGetReaderContactlessTransactionLimit(out ReaderContactlessTransactionLimit? result)
    {
        if (_TransactionLimit == null)
        {
            result = null;

            return false;
        }

        result = _TransactionLimit;

        return true;
    }

    #endregion

    #region Write

    public void Update(ReaderContactlessTransactionLimit transactionLimit) => _TransactionLimit = transactionLimit;
    public void IncrementRelayResistanceProtocolCount() => _RelayResistanceProtocolCount++;
    public void SetIsPdolDataMissing(bool value) => _IsPdolDataMissing = value;
    public void Update(CryptogramTypes value) => _ApplicationCryptogramTypes = value;
    public void Update(OdaStatusTypes value) => _OdaStatusTypes = value; 

    #endregion
}