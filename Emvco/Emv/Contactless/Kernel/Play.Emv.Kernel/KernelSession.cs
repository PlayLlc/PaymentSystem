using System;

using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Outcomes;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Timeouts;
using Play.Globalization.Time;

namespace Play.Emv.Kernel;

public class KernelSession
{
    #region Instance Values

    private readonly KernelSessionId _KernelSessionId;
    private readonly KernelDatabase _KernelDatabase;
    private readonly DataExchangeKernelService _DataExchangeKernelService;
    private readonly TimeoutManager _TimeoutManager;

    #endregion

    #region Constructor

    public KernelSession(
        KernelSessionId kernelSessionId,
        IHandleTerminalRequests terminalEndpoint,
        KernelDatabase kernelDatabase,
        ISendTerminalQueryResponse kernelEndpoint)
    {
        _KernelSessionId = kernelSessionId;
        _KernelDatabase = kernelDatabase;
        _DataExchangeKernelService = new DataExchangeKernelService(kernelSessionId, terminalEndpoint, kernelDatabase, kernelEndpoint);
        _TimeoutManager = new TimeoutManager();
    }

    #endregion

    #region Instance Members

    public DataExchangeKernelService GetDataExchangeKernelService() => _DataExchangeKernelService;

    #endregion

    #region Timeout Management

    public void StartTimeout(Milliseconds timeout) => _TimeoutManager.Start(timeout);
    public void StopTimeout() => _TimeoutManager.Stop();
    public bool TimedOut() => _TimeoutManager.TimedOut();

    #endregion

    #region Read

    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();

    public Outcome GetOutcome() =>
        new(GetErrorIndication(), GetOutcomeParameterSet(), GetDataRecord(), GetDiscretionaryData(), GetUserInterfaceRequestData());

    private ErrorIndication GetErrorIndication() => ErrorIndication.Decode(_KernelDatabase.Get(ErrorIndication.Tag).EncodeValue().AsSpan());

    private OutcomeParameterSet GetOutcomeParameterSet() =>
        OutcomeParameterSet.Decode(_KernelDatabase.Get(OutcomeParameterSet.Tag).EncodeValue().AsSpan());

    private UserInterfaceRequestData? GetUserInterfaceRequestData()
    {
        if (_KernelDatabase.IsPresentAndNotEmpty(UserInterfaceRequestData.Tag))
            return null;

        return UserInterfaceRequestData.Decode(_KernelDatabase.Get(UserInterfaceRequestData.Tag).EncodeValue().AsSpan());
    }

    private DataRecord? GetDataRecord()
    {
        if (_KernelDatabase.IsPresentAndNotEmpty(DataRecord.Tag))
            return null;

        return DataRecord.Decode(_KernelDatabase.Get(DataRecord.Tag).EncodeValue().AsSpan());
    }

    private DiscretionaryData? GetDiscretionaryData()
    {
        if (_KernelDatabase.IsPresentAndNotEmpty(DiscretionaryData.Tag))
            return null;

        return DiscretionaryData.Decode(_KernelDatabase.Get(DiscretionaryData.Tag).EncodeValue().AsSpan());
    }

    #endregion

    #region Write

    public void Update(Level1Error value)
    {
        _KernelDatabase.Update(new ErrorIndication(GetErrorIndication(), value));
    }

    public void Update(Level2Error value)
    {
        _KernelDatabase.Update(new ErrorIndication(GetErrorIndication(), value));
    }

    public void Update(Level3Error value)
    {
        _KernelDatabase.Update(new ErrorIndication(GetErrorIndication(), value));
    }

    public void Reset(OutcomeParameterSet value)
    {
        _KernelDatabase.Update(value);
    }

    public void Reset(UserInterfaceRequestData value)
    {
        _KernelDatabase.Update(value);
    }

    public void Reset(ErrorIndication value)
    {
        _KernelDatabase.Update(value);
    }

    public void Update(OutcomeParameterSet.Builder value)
    {
        _KernelDatabase.Update(GetOutcomeParameterSet() | value.Complete());
    }

    public void Update(UserInterfaceRequestData.Builder value)
    {
        UserInterfaceRequestData? userInterfaceRequestData = GetUserInterfaceRequestData();

        if (userInterfaceRequestData == null)
            _KernelDatabase.Update(value.Complete());

        _KernelDatabase.Update(GetUserInterfaceRequestData()! | value.Complete());
    }

    #endregion
}