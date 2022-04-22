using System;
using System.Threading.Tasks;

using Play.Emv.Ber.Enums;
using Play.Emv.Icc;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public class ProcessingOptionsRetriever : IGetProcessingOptions
{
    #region Instance Values

    private readonly IPcdTransceiver _ChipReader;

    #endregion

    #region Constructor

    public ProcessingOptionsRetriever(IPcdTransceiver chipReader)
    {
        _ChipReader = chipReader;
    }

    #endregion

    #region Instance Members

    public async Task<GetProcessingOptionsResponse> Transceive(GetProcessingOptionsRequest command)
    {
        try
        {
            GetProcessingOptionsRApduSignal response = new(await _ChipReader.Transceive(command.Serialize()).ConfigureAwait(false));

            return new GetProcessingOptionsResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);
        }

        catch (PcdProtocolException)
        {
            // TODO: Logging

            return new GetProcessingOptionsResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                new GetProcessingOptionsRApduSignal(Array.Empty<byte>(), Level1Error.ProtocolError));
        }
        catch (PcdTimeoutException)
        {
            // TODO: Logging

            return new GetProcessingOptionsResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                new GetProcessingOptionsRApduSignal(Array.Empty<byte>(), Level1Error.ProtocolError));
        }
    }

    #endregion
}