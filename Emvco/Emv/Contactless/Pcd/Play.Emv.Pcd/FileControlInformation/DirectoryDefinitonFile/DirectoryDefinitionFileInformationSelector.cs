using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;
using Play.Emv.Templates.FileControlInformation;
using Play.Icc.Emv;
using Play.Icc.Emv.FileControlInformation;

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

    public async Task<SelectDirectoryDefinitionFileResponse> Transceive(SelectDirectoryDefinitionFileCommand command)
    {
        GetFileControlInformationRApduSignal response = new(await _PcdTransceiver
                                                                .Transceive(GetFileControlInformationCApduSignal
                                                                                .Get(command.GetDirectoryDefinitionFileName()).Serialize())
                                                                .ConfigureAwait(false));

        if (response.GetLevel1Error() != Level1Error.Ok)
            return new SelectDirectoryDefinitionFileResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);

        FileControlInformationDdf template = FileControlInformationDdf.Decode(response.GetData());

        return new SelectDirectoryDefinitionFileResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), template, response);
    }

    #endregion
}