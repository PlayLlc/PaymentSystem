using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;

namespace Play.Emv.Outcomes;

/// <summary>
///     Kernel OUT DataExchangeSignal
/// </summary>
public class FinalOutcome
{
    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;
    private readonly KernelSessionId? _KernelSessionId;
    private readonly DataRecord? _DataRecord;
    private readonly DiscretionaryData? _DiscretionaryData;
    private readonly OutcomeParameterSet _OutcomeParameterSet;
    private readonly UserInterfaceRequestData? _UserInterfaceRequestData;

    #endregion

    #region Constructor

    public FinalOutcome(
        TransactionSessionId transactionSessionId,
        KernelSessionId kernelSessionId,
        OutcomeParameterSet outcomeParameterSet,
        DiscretionaryData? discretionaryData = null,
        UserInterfaceRequestData? userInterfaceRequestData = null,
        DataRecord? dataRecord = null)
    {
        _TransactionSessionId = transactionSessionId;
        _KernelSessionId = kernelSessionId;
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _UserInterfaceRequestData = userInterfaceRequestData;
        _DataRecord = dataRecord;
    }

    public FinalOutcome(
        TransactionSessionId transactionSessionId,
        OutcomeParameterSet outcomeParameterSet,
        DiscretionaryData? discretionaryData = null,
        UserInterfaceRequestData? userInterfaceRequestData = null,
        DataRecord? dataRecord = null)
    {
        _KernelSessionId = null;
        _TransactionSessionId = transactionSessionId;
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _UserInterfaceRequestData = userInterfaceRequestData;
        _DataRecord = dataRecord;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    public bool TryGetKernelSessionId(out KernelSessionId? result)
    {
        result = _KernelSessionId;

        return result is null;
    }

    public bool TryGetDataRecord(out DataRecord? result)
    {
        if (_DataRecord == null)
        {
            result = null;

            return false;
        }

        result = _DataRecord;

        return true;
    }

    public bool TryGetDDiscretionaryData(out DiscretionaryData? result)
    {
        if (_DiscretionaryData == null)
        {
            result = null;

            return false;
        }

        result = _DiscretionaryData;

        return true;
    }

    public OutcomeParameterSet GetOutcomeParameterSet() => _OutcomeParameterSet;

    public bool TryUserInterfaceRequestData(out UserInterfaceRequestData? result)
    {
        if (_UserInterfaceRequestData == null)
        {
            result = null;

            return false;
        }

        result = _UserInterfaceRequestData;

        return true;
    }

    #endregion
}