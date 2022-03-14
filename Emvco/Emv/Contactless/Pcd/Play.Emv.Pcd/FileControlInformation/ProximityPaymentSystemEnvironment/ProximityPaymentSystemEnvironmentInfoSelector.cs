using System;
using System.Threading.Tasks;

using Play.Emv.Icc;
using Play.Emv.Pcd.Contracts;

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

    public async Task<SelectProximityPaymentSystemEnvironmentResponse> Transceive(SelectProximityPaymentSystemEnvironmentRequest command)
    {
        try
        {
            GetFileControlInformationRApduSignal? rApduSignal =
                new(await _PcdTransceiver.Transceive(command.Serialize()).ConfigureAwait(false));

            return new SelectProximityPaymentSystemEnvironmentResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                                                                       rApduSignal);
        }

        catch (PcdProtocolException)
        {
            // TODO: Logging

            return new SelectProximityPaymentSystemEnvironmentResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                                                                       new GetFileControlInformationRApduSignal(Array.Empty<byte>(),
                                                                        Level1Error.ProtocolError));
        }
        catch (PcdTimeoutException)
        {
            // TODO: Logging

            return new SelectProximityPaymentSystemEnvironmentResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                                                                       new GetFileControlInformationRApduSignal(Array.Empty<byte>(),
                                                                        Level1Error.ProtocolError));
        }
    }

    #endregion
}