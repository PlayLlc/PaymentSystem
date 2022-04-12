using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Messaging;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2Session : KernelSession
{
    #region Instance Values

    private readonly TornEntry? _TornEntry = null;
    private CryptogramTypes _ApplicationCryptogramTypes = CryptogramTypes.ApplicationAuthenticationCryptogram;
    private OdaStatusTypes _OdaStatusTypes = OdaStatusTypes.NotAvailable;
    private bool _IsPdolDataMissing = true;
    private byte _RelayResistanceProtocolCount;

    #endregion

    #region Constructor

    public Kernel2Session(CorrelationId correlationId, KernelSessionId kernelSessionId) : base(correlationId, kernelSessionId)
    { }

    #endregion

    #region Read

    public bool TryGetTornEntry(out TornEntry? result)
    {
        if (_TornEntry is null)
        {
            result = null;

            return false;
        }

        result = _TornEntry;

        return true;
    }

    public byte GetRelayResistanceProtocolCount() => _RelayResistanceProtocolCount;
    public CryptogramTypes GetApplicationCryptogramType() => _ApplicationCryptogramTypes;
    public OdaStatusTypes GetOdaStatus() => _OdaStatusTypes;
    public bool IsPdolDataMissing() => _IsPdolDataMissing;

    #endregion

    #region Write

    public void IncrementRelayResistanceProtocolCount() => _RelayResistanceProtocolCount++;
    public void SetIsPdolDataMissing(bool value) => _IsPdolDataMissing = value;

    /// <summary>
    ///     Updates the Application Cryptogram Type of the current transaction
    /// </summary>
    /// <param name="value"></param>
    public void Update(CryptogramTypes value) => _ApplicationCryptogramTypes = value;

    public void Update(OdaStatusTypes value) => _OdaStatusTypes = value;

    #endregion
}