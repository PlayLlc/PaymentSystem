﻿using Play.Emv.DataElements.Emv;
using Play.Emv.Sessions;

namespace Play.Emv.Outcomes;

/// <summary>
///     Kernel OUT DataExchangeSignal
/// </summary>
public class FinalOutcome
{
    #region Instance Values

    private readonly KernelSessionId _KernelSessionId;
    private readonly DataRecord? _DataRecord;
    private readonly DiscretionaryData? _DiscretionaryData;
    private readonly OutcomeParameterSet _OutcomeParameterSet;
    private readonly UserInterfaceRequestData? _UserInterfaceRequestData;

    #endregion

    #region Constructor

    public FinalOutcome(
        KernelSessionId kernelSessionId,
        OutcomeParameterSet outcomeParameterSet,
        DiscretionaryData? discretionaryData = null,
        UserInterfaceRequestData? userInterfaceRequestData = null,
        DataRecord? dataRecord = null)
    {
        _KernelSessionId = kernelSessionId;
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _UserInterfaceRequestData = userInterfaceRequestData;
        _DataRecord = dataRecord;
    }

    #endregion

    #region Instance Members

    public KernelSessionId GetKernelSessionId() => _KernelSessionId;

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