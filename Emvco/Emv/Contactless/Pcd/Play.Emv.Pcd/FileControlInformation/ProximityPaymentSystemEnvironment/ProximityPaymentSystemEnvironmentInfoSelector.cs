using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;
using Play.Icc.Emv.FileControlInformation;

namespace Play.Emv.Pcd;

public class ProximityPaymentSystemEnvironmentInfoSelector : ISelectProximityPaymentSystemEnvironmentInfo
{
    #region Instance Values

    private readonly IPcdTransceiver _PcdTransceiver;

    #endregion

    #region Constructor

    public ProximityPaymentSystemEnvironmentInfoSelector(IPcdTransceiver pcdTransceiver)
    {
        _PcdTransceiver = pcdTransceiver;
    }

    #endregion

    #region Instance Members

    public async Task<SelectProximityPaymentSystemEnvironmentResponse> Transceive(SelectProximityPaymentSystemEnvironmentRequest cApdu)
    {
        GetFileControlInformationRApduSignal? rApduSignal = new(await _PcdTransceiver.Transceive(cApdu.Serialize()).ConfigureAwait(false));

        return new SelectProximityPaymentSystemEnvironmentResponse(cApdu.GetCorrelationId(), cApdu.GetTransactionSessionId(), rApduSignal);
    }

    #endregion
}