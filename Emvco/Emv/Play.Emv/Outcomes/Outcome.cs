using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Globalization.Time;
using Play.Icc.Emv;

namespace Play.Emv.Outcomes;

public class Outcome
{
    #region Static Metadata

    public static readonly Outcome Default;

    #endregion

    #region Instance Values

    private readonly DataRecord? _DataRecord;
    private readonly DiscretionaryData? _DiscretionaryData;
    private ErrorIndication _ErrorIndication;
    private OutcomeParameterSet _OutcomeParameterSet;
    private UserInterfaceRequestData? _UserInterfaceRequestData;
    private TerminalVerificationResults _TerminalVerificationResults;

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

    public Outcome(
        OutcomeParameterSet outcomeParameterSet,
        DiscretionaryData discretionaryData,
        UserInterfaceRequestData userInterfaceRequestData)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _UserInterfaceRequestData = userInterfaceRequestData;

        _TerminalVerificationResults = new TerminalVerificationResults(0);
    }

    public Outcome(
        ErrorIndication errorIndication,
        OutcomeParameterSet outcomeParameterSet,
        DataRecord? dataRecord = null,
        DiscretionaryData? discretionaryData = null,
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

    public TagLengthValue[] AsTagLengthValueArray()
    {
        List<TagLengthValue> buffer = new()
        {
            _ErrorIndication!.AsTagLengthValue(), _OutcomeParameterSet!.AsTagLengthValue(),
            _TerminalVerificationResults.AsTagLengthValue()
        };

        if (_DataRecord != null)
            buffer.Add(_DataRecord!.AsTagLengthValue());
        if (_DiscretionaryData != null)
            buffer.Add(_DiscretionaryData!.AsTagLengthValue());
        if (_UserInterfaceRequestData != null)
            buffer.Add(_UserInterfaceRequestData!.AsTagLengthValue());

        return buffer.ToArray();
    }

    public FieldOffRequestOutcome GetFieldOffRequestOutcome()
    {
        return _OutcomeParameterSet.GetFieldOffRequestOutcome();
    }

    public StartOutcome GetStartOutcome()
    {
        return _OutcomeParameterSet.GetStartOutcome();
    }

    public StatusOutcome GetStatusOutcome()
    {
        return _OutcomeParameterSet.GetStatusOutcome();
    }

    public Milliseconds GetTimeout()
    {
        return _OutcomeParameterSet.GetTimeout();
    }

    public bool IsErrorPresent()
    {
        return _ErrorIndication.IsErrorPresent();
    }

    public bool IsRestart()
    {
        return IsRestartedByEntryPoint();

        // etc
    }

    /// <summary>
    ///     This flag indicates if the transaction should reprocess. The <see cref="StatusOutcome" /> will determine
    ///     which Start Point the processing point enters
    /// </summary>
    /// <remarks>
    ///     Book B Section 3.2.1.1
    /// </remarks>
    public bool IsRestartedByEntryPoint()
    {
        StatusOutcome statusOutcome = _OutcomeParameterSet.GetStatusOutcome();

        if (statusOutcome == StatusOutcome.NotAvailable)
            return false;

        if (statusOutcome == StatusOutcome.TryAgain)
            return true;

        if (statusOutcome == StatusOutcome.SelectNext)
            return true;

        return false;
    }

    public bool IsSelectNext()
    {
        return GetStatusOutcome() == StatusOutcome.SelectNext;
    }

    public TerminalVerificationResults GetTerminalVerificationResults()
    {
        return _TerminalVerificationResults;
    }

    public bool IsTryAgain()
    {
        return GetStatusOutcome() == StatusOutcome.TryAgain;
    }

    public bool IsUiRequestOnOutcomePresent()
    {
        return _OutcomeParameterSet.IsUiRequestOnOutcomePresent();
    }

    public bool IsUiRequestOnRestartPresent()
    {
        return _OutcomeParameterSet.IsUiRequestOnRestartPresent();
    }

    public void Reset(ErrorIndication errorIndication)
    {
        _ErrorIndication = errorIndication;
    }

    public void Update(Level1Error level1Error)
    {
        _ErrorIndication = new ErrorIndication(_ErrorIndication, level1Error);
    }

    public void Update(Level2Error level2Error)
    {
        _ErrorIndication = new ErrorIndication(_ErrorIndication, level2Error);
    }

    public void Update(Level3Error level3Error)
    {
        _ErrorIndication = new ErrorIndication(_ErrorIndication, level3Error);
    }

    public void Update(TerminalVerificationResult value)
    {
        _TerminalVerificationResults |= new TerminalVerificationResults(value);
    }

    public void Reset(TerminalVerificationResults value)
    {
        _TerminalVerificationResults = value;
    }

    public void Reset(OutcomeParameterSet outcomeParameterSet)
    {
        _OutcomeParameterSet = outcomeParameterSet;
    }

    public void Reset(UserInterfaceRequestData userInterfaceRequestData)
    {
        _UserInterfaceRequestData = userInterfaceRequestData;
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

    public OutcomeParameterSet GetOutcomeParameterSet()
    {
        return _OutcomeParameterSet;
    }

    public DiscretionaryData? GetDiscretionaryData()
    {
        return _DiscretionaryData;
    }

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