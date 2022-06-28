using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Globalization.Time;

namespace Play.Emv.Outcomes;

public class Outcome
{
    #region Static Metadata

    public static readonly Outcome Default;

    #endregion

    #region Instance Values

    private readonly DataRecord? _DataRecord;
    private readonly DiscretionaryData? _DiscretionaryData;
    private readonly TerminalVerificationResults _TerminalVerificationResults;
    private ErrorIndication _ErrorIndication;
    private OutcomeParameterSet _OutcomeParameterSet;
    private UserInterfaceRequestData? _UserInterfaceRequestData;

    #endregion

    #region Constructor

    static Outcome()
    {
        Default = new Outcome(OutcomeParameterSet.Default);
    }

    public Outcome()
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        _OutcomeParameterSet = builder.Complete();
        _ErrorIndication = new ErrorIndication();
        _TerminalVerificationResults = new TerminalVerificationResults(0);
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _TerminalVerificationResults = new TerminalVerificationResults(0);
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet, UserInterfaceRequestData userInterfaceRequestData)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _UserInterfaceRequestData = userInterfaceRequestData;

        _TerminalVerificationResults = new TerminalVerificationResults(0);
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet, DiscretionaryData discretionaryData)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;

        _TerminalVerificationResults = new TerminalVerificationResults(0);
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet, DiscretionaryData discretionaryData, DataRecord dataRecord)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _DataRecord = dataRecord;

        _TerminalVerificationResults = new TerminalVerificationResults(0);
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet, DiscretionaryData discretionaryData, UserInterfaceRequestData userInterfaceRequestData)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _UserInterfaceRequestData = userInterfaceRequestData;

        _TerminalVerificationResults = new TerminalVerificationResults(0);
    }

    public Outcome(
        ErrorIndication errorIndication, OutcomeParameterSet outcomeParameterSet, DataRecord? dataRecord = null, DiscretionaryData? discretionaryData = null,
        UserInterfaceRequestData? userInterfaceRequestData = null)
    {
        _ErrorIndication = errorIndication;
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _UserInterfaceRequestData = userInterfaceRequestData;
        _DataRecord = dataRecord;
        _TerminalVerificationResults = new TerminalVerificationResults(0);
    }

    #endregion

    #region Instance Members

    public PrimitiveValue[] AsPrimitiveValues()
    {
        List<PrimitiveValue> buffer = new() {_ErrorIndication, _OutcomeParameterSet, _TerminalVerificationResults};

        if (_DataRecord != null)
            buffer.Add(_DataRecord!);
        if (_DiscretionaryData != null)
            buffer.Add(_DiscretionaryData!);
        if (_UserInterfaceRequestData != null)
            buffer.Add(_UserInterfaceRequestData!);

        return buffer.ToArray();
    }

    public FieldOffRequestOutcome GetFieldOffRequestOutcome() => _OutcomeParameterSet.GetFieldOffRequestOutcome();
    public StartOutcomes GetStartOutcome() => _OutcomeParameterSet.GetStartOutcome();
    public StatusOutcomes GetStatusOutcome() => _OutcomeParameterSet.GetStatusOutcome();
    public Milliseconds GetTimeout() => _OutcomeParameterSet.GetTimeout();
    public ErrorIndication GetErrorIndication() => _ErrorIndication;
    public bool IsErrorPresent() => _ErrorIndication.IsErrorPresent();
    public bool IsRestart() => IsRestartedByEntryPoint();

    // etc
    /// <summary>
    ///     This flag indicates if the transaction should reprocess. The <see cref="StatusOutcomes" /> will determine
    ///     which Start Point the processing point enters
    /// </summary>
    /// <remarks>
    ///     Book B Section 3.2.1.1
    /// </remarks>
    public bool IsRestartedByEntryPoint()
    {
        StatusOutcomes statusOutcomes = _OutcomeParameterSet.GetStatusOutcome();

        if (statusOutcomes == StatusOutcomes.NotAvailable)
            return false;

        if (statusOutcomes == StatusOutcomes.TryAgain)
            return true;

        if (statusOutcomes == StatusOutcomes.SelectNext)
            return true;

        return false;
    }

    public bool IsSelectNext() => GetStatusOutcome() == StatusOutcomes.SelectNext;
    public TerminalVerificationResults GetTerminalVerificationResults() => _TerminalVerificationResults;
    public bool IsTryAgain() => GetStatusOutcome() == StatusOutcomes.TryAgain;
    public bool IsUiRequestOnOutcomePresent() => _OutcomeParameterSet.IsUiRequestOnOutcomePresent();
    public bool IsUiRequestOnRestartPresent() => _OutcomeParameterSet.IsUiRequestOnRestartPresent();

    public void Reset(ErrorIndication errorIndication)
    {
        _ErrorIndication = errorIndication;
    }

    public bool TryGetUserInterfaceRequestData(out UserInterfaceRequestData? result)
    {
        if (_UserInterfaceRequestData == null)
        {
            result = null;

            return false;
        }

        result = _UserInterfaceRequestData;

        return true;
    }

    public void Update(OutcomeParameterSet.Builder outcomeParameterSet)
    {
        _OutcomeParameterSet |= outcomeParameterSet.Complete();
    }

    public void Update(UserInterfaceRequestData.Builder userInterfaceRequestData)
    {
        if (_UserInterfaceRequestData == null)
        {
            _UserInterfaceRequestData = userInterfaceRequestData.Complete();

            return;
        }

        _UserInterfaceRequestData = _UserInterfaceRequestData |= userInterfaceRequestData.Complete();
    }

    public OutcomeParameterSet GetOutcomeParameterSet() => _OutcomeParameterSet;
    public DiscretionaryData? GetDiscretionaryData() => _DiscretionaryData;

    public bool TryGetDiscretionaryData(out DiscretionaryData? result)
    {
        if (_DiscretionaryData == null)
        {
            result = null;

            return false;
        }

        result = _DiscretionaryData;

        return true;
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

    #endregion
}