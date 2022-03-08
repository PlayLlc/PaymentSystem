using System.Threading.Tasks;

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

    public async Task<SelectApplicationDefinitionFileInfoResponse> Transceive(SelectApplicationDefinitionFileInfoRequest infoCommand)
    {
        GetFileControlInformationRApduSignal response = new(await _PcdTransceiver.Transceive(infoCommand.Serialize()));

        return new SelectApplicationDefinitionFileInfoResponse(infoCommand.GetCorrelationId(), infoCommand.GetTransactionSessionId(),
            response);
    }

    #endregion
}