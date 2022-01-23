﻿using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;
using Play.Icc.Emv.GetProcessingOptions;

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

    public async Task<GetProcessingOptionsResponse> Transceive(GetProcessingOptionsCommand command)
    {
        GetProcessingOptionsRApduSignal response = new(await _ChipReader.Transceive(command.Serialize()).ConfigureAwait(false));

        return new GetProcessingOptionsResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);
    }

    #endregion
}