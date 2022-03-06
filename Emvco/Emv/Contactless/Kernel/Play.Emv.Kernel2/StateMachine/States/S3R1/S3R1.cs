using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Emv.DataElements.Emv;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.GetData;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel2.StateMachine.States.S3R1;
// TODO: Note that symbols S3R1.10, S3R1.11, S3R1.12, S3R1.13 and S3R1.18 are only implemented for the IDS/TORN Implementation Option.

public class S3R1
{
    #region Instance Values

    protected readonly KernelDatabase _KernelDatabase;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly IHandleBlockingPcdRequests _PcdEndpoint;
    private readonly IGetKernelState _KernelStateResolver;
    private readonly ICleanTornTransactions _KernelCleaner;

    #endregion

    #region Instance Members

    public async Task Process(KernelSession session, TagsToRead tagsToRead, ApplicationFileLocator? applicationFileLocator)
    {
        if (_DataExchangeKernelService.GetLength(DekRequestType.TagsToRead) != 0)
        {
            // HACK: We can only resolve the TagsToRead inside the DataExchangeKernelService, so we'll have to add the ProcessTagsToRead logic inside that service
            await ProcessTagsToRead(session.GetTransactionSessionId(), tagsToRead).ConfigureAwait(false);
        }

        if (applicationFileLocator != null)
            await ProcessApplicationData(session.GetTransactionSessionId(), applicationFileLocator!).ConfigureAwait(false);
    }

    // TODO: Resolve the GetDataResponse[] to TagLengthValue[] inside the GetDataBatchResponse
    // TODO: Move this to DataExchangeKernelService
    private async Task ProcessTagsToRead(TransactionSessionId sessionId, TagsToRead tagsToRead)
    {
        GetDataBatchRequest capdu = GetDataBatchRequest.Create(sessionId, tagsToRead);
        GetDataBatchResponse rapdu = await _PcdEndpoint.Transceive(capdu).ConfigureAwait(false);
    }

    private async Task ProcessApplicationData(TransactionSessionId sessionId, ApplicationFileLocator applicationFileLocator)
    {
        // TODO: Resolve the ReadElementaryFileRecordRangeResponse[] to TagLengthValue[] inside the GetDataBatchResponse
        ReadApplicationDataRequest capdu = ReadApplicationDataRequest.Create(applicationFileLocator, sessionId);
        ReadApplicationDataResponse rapdu = await _PcdEndpoint.Transceive(capdu).ConfigureAwait(false);

        TagLengthValue[] applicationData = rapdu.GetApplicationData();

        // HACK: Are we sure these are initialized first?
        _KernelDatabase.UpdateRange(applicationData);
    }

    #endregion
}