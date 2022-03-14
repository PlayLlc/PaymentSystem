using System;
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
        try
        {
            GetFileControlInformationRApduSignal response = new(await _PcdTransceiver
                                                                    .Transceive(GetFileControlInformationCApduSignal
                                                                                    .Get(command.GetDirectoryDefinitionFileName())
                                                                                    .Serialize()).ConfigureAwait(false));

            FileControlInformationDdf template = FileControlInformationDdf.Decode(response.GetData());

            return new SelectDirectoryDefinitionFileResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), template,
                                                             response);
        }

        catch (PcdProtocolException)
        {
            // TODO: Logging

            return new SelectDirectoryDefinitionFileResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                                                             new GetFileControlInformationRApduSignal(Array.Empty<byte>(),
                                                              Level1Error.ProtocolError));
        }
        catch (PcdTimeoutException)
        {
            // TODO: Logging

            return new SelectDirectoryDefinitionFileResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                                                             new GetFileControlInformationRApduSignal(Array.Empty<byte>(),
                                                              Level1Error.ProtocolError));
        }
    }

    #endregion
}