using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup.State5;

public partial class WaitingForGetDataResponse : KernelState
{
    #region RAPDU

    public override KernelState Handle(KernelSession session, QueryPcdResponse signal)
    {
        HandleRequestOutOfSync(session, signal);

        if (!signal.IsSuccessful())
        {
            PrepareUserInterfaceMessageForL1Error(session, signal);

            return _KernelStateResolver.GetKernelState(StateId);
        }

        if (session.TryDequeueActiveApplicationFileLocator(out Play.Icc.FileSystem.ElementaryFiles.RecordRange? recordRange))
        {
            
            var capdu = _PcdEndpoint.Request(ReadElementaryFileRecordRangeRequest.Create(session.GetTransactionSessionId(), recordRange!.Value.GetShortFileIdentifier(), recordRange.Value.GetRecords()))
        }

        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);
    }

    private void PrepareUserInterfaceMessageForL1Error(KernelSession session, QueryPcdResponse signal)
    {
        _KernelDatabase.Update(MessageIdentifier.TryAgain);
        _KernelDatabase.Update(Status.ReadyToRead);
        _KernelDatabase.Update(new MessageHoldTime(0));

        _KernelDatabase.Update(StatusOutcome.EndApplication);

        _KernelDatabase.Update(StartOutcome.B);
        _KernelDatabase.SetUiRequestOnRestartPresent(true);
        _KernelDatabase.Update(signal.GetLevel1Error());
        _KernelDatabase.Update(MessageOnErrorIdentifier.TryAgain);
        KernelOutcome.CreateEmvDiscretionaryData(_KernelDatabase, _DataExchangeKernelService);

        _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
    }

    #endregion
}