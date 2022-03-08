using System.Threading.Tasks;

using Play.Emv.Icc;
using Play.Emv.Icc.FileControlInformation;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Templates;

namespace Play.Emv.Pcd;

public class DirectoryDefinitionFileInformationSelector : ISelectDirectoryDefinitionFileInformation
{
    #region Instance Values

    private readonly IPcdTransceiver _PcdTransceiver;

    #endregion

    #region Constructor

    public DirectoryDefinitionFileInformationSelector(IPcdTransceiver pcdTransceiver)
    {
        _PcdTransceiver = pcdTransceiver;
    }

    #endregion

    #region Instance Members

    public async Task<SelectDirectoryDefinitionFileResponse> Transceive(SelectDirectoryDefinitionFileRequest command)
    {
        GetFileControlInformationRApduSignal response = new(await _PcdTransceiver
            .Transceive(GetFileControlInformationCApduSignal.Get(command.GetDirectoryDefinitionFileName()).Serialize())
            .ConfigureAwait(false));

        if (response.GetLevel1Error() != Level1Error.Ok)
            return new SelectDirectoryDefinitionFileResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);

        FileControlInformationDdf template = FileControlInformationDdf.Decode(response.GetData());

        return new SelectDirectoryDefinitionFileResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), template, response);
    }

    #endregion
}