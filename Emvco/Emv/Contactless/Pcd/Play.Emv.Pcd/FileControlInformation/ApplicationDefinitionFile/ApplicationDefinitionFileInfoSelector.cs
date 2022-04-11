using System;
using System.Threading.Tasks;

using Play.Emv.Ber.Enums;
using Play.Emv.Icc;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public class ApplicationDefinitionFileInfoSelector : ISelectApplicationDefinitionFileInformation
{
    #region Instance Values

    private readonly IPcdTransceiver _PcdTransceiver;

    #endregion

    #region Constructor

    public ApplicationDefinitionFileInfoSelector(IPcdTransceiver pcdTransceiver)
    {
        _PcdTransceiver = pcdTransceiver;
    }

    #endregion

    #region Instance Members

    public async Task<SelectApplicationDefinitionFileInfoResponse> Transceive(SelectApplicationDefinitionFileInfoRequest command)
    {
        try
        {
            GetFileControlInformationRApduSignal
                response = new(await _PcdTransceiver.Transceive(command.Serialize()).ConfigureAwait(false));

            return new SelectApplicationDefinitionFileInfoResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);
        }
        catch (PcdProtocolException)
        {
            // TODO: Logging

            return new SelectApplicationDefinitionFileInfoResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                new GetFileControlInformationRApduSignal(Array.Empty<byte>(), Level1Error.ProtocolError));
        }
        catch (PcdTimeoutException)
        {
            // TODO: Logging
            return new SelectApplicationDefinitionFileInfoResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                new GetFileControlInformationRApduSignal(Array.Empty<byte>(), Level1Error.TimeOutError));
        }
    }

    #endregion
}