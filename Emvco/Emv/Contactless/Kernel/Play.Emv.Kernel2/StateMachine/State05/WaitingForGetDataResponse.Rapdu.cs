using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataElements;
using Play.Emv.Icc.GetData;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGetDataResponse : KernelState
{
    #region RAPDU

    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (TryHandleL1Error(session, signal))
            return _KernelStateResolver.GetKernelState(StateId);

        PersistGetDataResponse(signal);

        if (!TryHandleGetDataToBeDone(session.GetTransactionSessionId()))
            HandleRemainingApplicationFilesToRead(session);

        return CommonProcessingS456.Process();
    }

    #region S5.5 - S5.6

    private bool TryHandleL1Error(KernelSession session, QueryPcdResponse signal)
    {
        if (!signal.IsSuccessful())
            return false;

        _KernelDatabase.Update(MessageIdentifier.TryAgain);
        _KernelDatabase.Update(Status.ReadyToRead);
        _KernelDatabase.Update(new MessageHoldTime(0));

        _KernelDatabase.Update(StatusOutcome.EndApplication);

        _KernelDatabase.Update(StartOutcome.B);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);
        _KernelDatabase.Update(signal.GetLevel1Error());
        _KernelDatabase.Update(MessageOnErrorIdentifier.TryAgain);
        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));

        return true;
    }

    #endregion

    #region S5.9

    // Instead of implementing a Current Tag, we're using a Peek and Resolve pattern from the Data Exchange Kernel's Tags To Read Yet list

    #endregion

    #region S5.10 - S5.13

    public bool TryHandleGetDataToBeDone(TransactionSessionId sessionId)
    {
        if (!_DataExchangeKernelService.TryPeek(DekRequestType.TagsToRead, out Tag tagToRead))
            return false;

        _PcdEndpoint.Request(GetDataRequest.Create(tagToRead, sessionId));

        return true;
    }

    #endregion

    #region S5.14 - S5.18

    public void HandleRemainingApplicationFilesToRead(KernelSession session)
    {
        if (!session.TryPeekActiveTag(out RecordRange recordRange))
            return;

        _PcdEndpoint.Request(ReadRecordRequest.Create(session.GetTransactionSessionId(), recordRange.GetShortFileIdentifier()));
    }

    #endregion

    #region S5.15

    // Instead of maintaining a 'NextCmd' object during the transaction session, we can use the Peek functionality of the
    // DataExchangeKernelService and the ActiveApplicationFileLocator objects to determine what the next command should be

    #endregion

    #region S5.19 - S5.24

    public void PersistGetDataResponse(QueryPcdResponse signal)
    {
        try
        {
            _KernelDatabase.Update(((GetDataResponse) signal).GetTagLengthValueResult());
            _DataExchangeKernelService.Resolve((GetDataResponse) signal);
        }
        catch (EmvParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(signal, _DataExchangeKernelService, _KernelDatabase);
        }
        catch (BerParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(signal, _DataExchangeKernelService, _KernelDatabase);
        }
        catch (CodecParsingException)
        {
            // TODO: Logging
            HandleBerParsingException(signal, _DataExchangeKernelService, _KernelDatabase);
        }
        catch (Exception)
        {
            // TODO: Logging
            HandleBerParsingException(signal, _DataExchangeKernelService, _KernelDatabase);
        }
    }

    private static void HandleBerParsingException(QueryPcdResponse signal, DataExchangeKernelService dataExchanger, KernelDatabase database)
    {
        dataExchanger.TryPeek(DekRequestType.TagsToRead, out Tag result);
        TagLengthValue nullResult = new(result, Array.Empty<byte>());

        byte[] emptyRapduBytes = new byte[nullResult.GetTagLengthValueByteCount() + 2];
        emptyRapduBytes[0] = StatusWords._9000.GetStatusWord1();
        emptyRapduBytes[1] = StatusWords._9000.GetStatusWord2();
        nullResult.EncodeTagLengthValue().AsSpan().CopyTo(emptyRapduBytes[2..]);

        database.Update(nullResult);
        dataExchanger.Resolve(new GetDataResponse(signal.GetCorrelationId(), signal.GetTransactionSessionId(),
                                                  new GetDataRApduSignal(emptyRapduBytes)));
    }

    #endregion

    #endregion
}