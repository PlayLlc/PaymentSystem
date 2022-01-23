﻿using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Core.Exceptions;
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
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet)
    {
        CheckCore.ForNull(outcomeParameterSet, nameof(outcomeParameterSet));
        _OutcomeParameterSet = outcomeParameterSet;
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet, UserInterfaceRequestData userInterfaceRequestData)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _UserInterfaceRequestData = userInterfaceRequestData;
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet, DiscretionaryData discretionaryData)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
    }

    public Outcome(OutcomeParameterSet outcomeParameterSet, DiscretionaryData discretionaryData, DataRecord dataRecord)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _DataRecord = dataRecord;
    }

    public Outcome(
        OutcomeParameterSet outcomeParameterSet,
        DiscretionaryData discretionaryData,
        UserInterfaceRequestData userInterfaceRequestData)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _UserInterfaceRequestData = userInterfaceRequestData;
    }

    public Outcome(
        ErrorIndication errorIndication,
        OutcomeParameterSet outcomeParameterSet,
        DataRecord? dataRecord = null,
        DiscretionaryData? discretionaryData = null,
        UserInterfaceRequestData? userInterfaceRequestData = null)
    {
        _OutcomeParameterSet = outcomeParameterSet;
        _DiscretionaryData = discretionaryData;
        _UserInterfaceRequestData = userInterfaceRequestData;
        _DataRecord = dataRecord;
    }

    #endregion

    #region Instance Members

    public TagLengthValue[] AsTagLengthValueArray()
    {
        List<TagLengthValue> buffer = new() {_ErrorIndication!.AsTagLengthValue(), _OutcomeParameterSet!.AsTagLengthValue()};

        if (_DataRecord != null)
            buffer.Add(_DataRecord!.AsTagLengthValue());
        if (_DiscretionaryData != null)
            buffer.Add(_DiscretionaryData!.AsTagLengthValue());
        if (_UserInterfaceRequestData != null)
            buffer.Add(_UserInterfaceRequestData!.AsTagLengthValue());

        return buffer.ToArray();
    }

    public FieldOffRequestOutcome GetFieldOffRequestOutcome() => _OutcomeParameterSet.GetFieldOffRequestOutcome();
    public StartOutcome GetStartOutcome() => _OutcomeParameterSet.GetStartOutcome();
    public StatusOutcome GetStatusOutcome() => _OutcomeParameterSet.GetStatusOutcome();
    public Milliseconds GetTimeout() => _OutcomeParameterSet.GetTimeout();
    public bool IsErrorPresent() => _ErrorIndication.IsErrorPresent();
    public bool IsRestart() => IsRestartedByEntryPoint(); // etc

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

    public bool IsSelectNext() => GetStatusOutcome() == StatusOutcome.SelectNext;
    public bool IsTryAgain() => GetStatusOutcome() == StatusOutcome.TryAgain;
    public bool IsUiRequestOnOutcomePresent() => _OutcomeParameterSet.IsUiRequestOnOutcomePresent();
    public bool IsUiRequestOnRestartPresent() => _OutcomeParameterSet.IsUiRequestOnRestartPresent();
    public void Reset(ErrorIndication errorIndication) => _ErrorIndication = errorIndication;

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

    public void Reset(OutcomeParameterSet outcomeParameterSet) => _OutcomeParameterSet = outcomeParameterSet;
    public void Reset(UserInterfaceRequestData userInterfaceRequestData) => _UserInterfaceRequestData = userInterfaceRequestData;

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

    public void Update(OutcomeParameterSet.Builder outcomeParameterSet) => _OutcomeParameterSet |= outcomeParameterSet.Complete();

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