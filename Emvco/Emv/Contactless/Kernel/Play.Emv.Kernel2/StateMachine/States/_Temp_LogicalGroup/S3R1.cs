using System;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup;
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

    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public async Task<KernelState> Process(KernelSession session)
    {
        // TODO: Optimized AFL means we'll be sending GET DATA commands. Otherwise, we'll be sending READ FILE commands

        _DataExchangeKernelService.Resolve(DekRequestType.TagsToRead);

        if (_DataExchangeKernelService.TryGetDataBatchRequest(session.GetTransactionSessionId(),
            out GetDataBatchRequest? getDataBatchRequest))
            await ProcessTagsToRead(getDataBatchRequest!).ConfigureAwait(false);

        if (_KernelDatabase.IsPresentAndNotEmpty(ApplicationFileLocator.Tag))

        {
            await ProcessApplicationData(session.GetTransactionSessionId(),
                    ApplicationFileLocator.Decode(_KernelDatabase.Get(ApplicationFileLocator.Tag).EncodeTagLengthValue().AsSpan()))
                .ConfigureAwait(false);
        }

        throw new NotImplementedException();
    }

    /// <summary>
    ///     HandleEmptyApplicationFileLocator
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    private void HandleEmptyApplicationFileLocator()
    {
        _KernelDatabase.Update(Level2Error.CardDataError);
    }

    /// <summary>
    ///     ProcessTagsToRead
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task ProcessTagsToRead(GetDataBatchRequest request)
    {
        try
        {
            GetDataBatchResponse rapdu = await _PcdEndpoint.Transceive(request).ConfigureAwait(false);
            _KernelDatabase.Update(rapdu.GetTagLengthValuesResult());
        }
        catch (BerParsingException)
        {
            //    /* logging */
            //    _KernelDatabase.Update(Level2Error.ParsingError);
            //    HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId());
            //}
            //catch (BerException)
            //{
            //    /* logging */
            //    signal.GetTransaction().Update(Level2Error.ParsingError);
            //    HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId());
            //}
            //catch (CardDataMissingException)
            //{
            //    /* logging */
            //    signal.GetTransaction().Update(Level2Error.CardDataError);
            //    HandleBerEncodingException(signal.GetCorrelationId(), signal.GetKernelSessionId());
        }
    }

    /// <summary>
    ///     ProcessApplicationData
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="applicationFileLocator"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task ProcessApplicationData(TransactionSessionId sessionId, ApplicationFileLocator applicationFileLocator)
    {
        ReadApplicationDataRequest capdu = ReadApplicationDataRequest.Create(applicationFileLocator, sessionId);
        ReadApplicationDataResponse rapdu = await _PcdEndpoint.Transceive(capdu).ConfigureAwait(false);

        TagLengthValue[] applicationData = rapdu.GetTagLengthValuesResult();

        _KernelDatabase.Update(applicationData);
    }

    #endregion
}