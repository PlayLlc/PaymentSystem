using System;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Outcomes;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel.Databases;

public partial class KernelDatabase
{
    #region Instance Values

    protected OutcomeParameterSet.Builder _OutcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
    protected UserInterfaceRequestData.Builder _UserInterfaceRequestDataBuilder = UserInterfaceRequestData.GetBuilder();
    protected ErrorIndication.Builder _ErrorIndicationBuilder = ErrorIndication.GetBuilder();
    protected TerminalVerificationResults.Builder _TerminalVerificationResultBuilder = TerminalVerificationResults.GetBuilder();
    protected TerminalCapabilities.Builder _TerminalCapabilitiesBuilder = TerminalCapabilities.GetBuilder();
    protected DataStorageSummaryStatus.Builder _DataStorageSummaryStatusBuilder = DataStorageSummaryStatus.GetBuilder();

    #endregion

    #region Outcome

    #region Read

    /// <summary>
    ///     GetDataStorageSummaryStatus
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public DataStorageSummaryStatus GetDataStorageSummaryStatus()
    {
        if (IsPresentAndNotEmpty(DataStorageSummaryStatus.Tag))
            return _DataStorageSummaryStatusBuilder.Complete();

        return (DataStorageSummaryStatus) Get(DataStorageSummaryStatus.Tag);
    }

    /// <exception cref="TerminalDataException"></exception>
    public ErrorIndication GetErrorIndication() => (ErrorIndication) Get(ErrorIndication.Tag);

    /// <exception cref="TerminalDataException"></exception>
    public OutcomeParameterSet GetOutcomeParameterSet() => (OutcomeParameterSet) Get(OutcomeParameterSet.Tag);

    /// <exception cref="TerminalDataException"></exception>
    private TerminalVerificationResults GetTerminalVerificationResults() => (TerminalVerificationResults) Get(TerminalVerificationResults.Tag);

    /// <exception cref="TerminalDataException"></exception>
    public UserInterfaceRequestData GetUserInterfaceRequestData()
    {
        if (IsPresentAndNotEmpty(UserInterfaceRequestData.Tag))
        {
            UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

            return builder.Complete();
        }

        return (UserInterfaceRequestData) Get(UserInterfaceRequestData.Tag);
    }

    /// <summary>
    ///     GetDataRecord
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private DataRecord? GetDataRecord()
    {
        if (IsPresentAndNotEmpty(DataRecord.Tag))
            return null;

        return (DataRecord) Get(DataRecord.Tag);
    }

    /// <exception cref="TerminalDataException"></exception>
    public bool IsSet(TerminalVerificationResultCodes value)
    {
        if (IsPresentAndNotEmpty(TerminalVerificationResults.Tag))
            return false;

        return ((TerminalVerificationResults) Get(TerminalVerificationResults.Tag)).IsSet(value);
    }

    /// <summary>
    ///     GetDiscretionaryData
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    private DiscretionaryData? GetDiscretionaryData()
    {
        if (!IsPresentAndNotEmpty(DiscretionaryData.Tag))
            return null;

        return (DiscretionaryData) Get(DiscretionaryData.Tag);
    }

    /// <exception cref="TerminalDataException"></exception>
    public Outcome GetOutcome() => new(GetErrorIndication(), GetOutcomeParameterSet(), GetUserInterfaceRequestData(), GetDataRecord(), GetDiscretionaryData());

    #endregion

    #region Write Outcome

    /// <summary>
    ///     CreateEmvDiscretionaryData
    /// </summary>
    /// <param name="dataExchanger"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void CreateEmvDiscretionaryData(DataExchangeKernelService dataExchanger)
    {
        // HACK: this logic should live inside discretionary data
        KernelOutcome.CreateEmvDiscretionaryData(this, dataExchanger);
    }

    /// <exception cref="TerminalDataException"></exception>
    public void CreateEmvDataRecord(DataExchangeKernelService dataExchanger) =>
        dataExchanger.Enqueue(DekResponseType.DiscretionaryData, DataRecord.CreateEmvDataRecord(this));

    /// <exception cref="TerminalDataException"></exception>
    public void CreateMagstripeDataRecord(DataExchangeKernelService dataExchanger) =>
        dataExchanger.Enqueue(DekResponseType.DiscretionaryData, DataRecord.CreateMagstripeDataRecord(this));

    /// <summary>
    ///     CreateMagstripeDiscretionaryData
    /// </summary>
    /// <param name="dataExchanger"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void CreateMagstripeDiscretionaryData(DataExchangeKernelService dataExchanger)
    {
        KernelOutcome.CreateMagstripeDiscretionaryData(this, dataExchanger);
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(MessageOnErrorIdentifiers value)
    {
        try
        {
            _ErrorIndicationBuilder.Reset(GetErrorIndication());
            _ErrorIndicationBuilder.Set(value);
            Update(_ErrorIndicationBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(MessageIdentifiers value)
    {
        try
        {
            _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
            _UserInterfaceRequestDataBuilder.Set(value);
            Update(_UserInterfaceRequestDataBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(ValueQualifiers value)
    {
        try
        {
            _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
            _UserInterfaceRequestDataBuilder.Set(value);
            Update(_UserInterfaceRequestDataBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(Statuses value)
    {
        try
        {
            _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
            _UserInterfaceRequestDataBuilder.Set(value);
            Update(_UserInterfaceRequestDataBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(MessageHoldTime value)
    {
        try
        {
            _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
            _UserInterfaceRequestDataBuilder.Set(value);
            Update(_UserInterfaceRequestDataBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Set(TerminalVerificationResult value)
    {
        try
        {
            _TerminalVerificationResultBuilder.Reset(GetTerminalVerificationResults());
            _TerminalVerificationResultBuilder.Set(value);
            Update(_TerminalVerificationResultBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(TerminalVerificationResults)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(TerminalVerificationResults)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(TerminalVerificationResults)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(StatusOutcomes value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.Set(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(CvmPerformedOutcome value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.Set(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(OnlineResponseOutcome value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.Set(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(FieldOffRequestOutcome value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.Set(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(StartOutcomes value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.Set(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetUiRequestOnRestartPresent(bool value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.SetIsUiRequestOnRestartPresent(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetUiRequestOnOutcomePresent(bool value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetIsDataRecordPresent(bool value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.SetIsDataRecordPresent(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetIsDiscretionaryDataPresent(bool value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.SetIsDiscretionaryDataPresent(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetIsReceiptPresent(bool value)
    {
        try
        {
            _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
            _OutcomeParameterSetBuilder.SetIsUiRequestOnRestartPresent(value);
            Update(_OutcomeParameterSetBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetReadIsSuccessful(bool value)
    {
        try
        {
            _DataStorageSummaryStatusBuilder.Reset(GetDataStorageSummaryStatus());
            _DataStorageSummaryStatusBuilder.SetReadIsSuccessful(value);
            Update(_DataStorageSummaryStatusBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void SetWriteIsSuccessful(bool value)
    {
        try
        {
            _DataStorageSummaryStatusBuilder.Reset(GetDataStorageSummaryStatus());
            _DataStorageSummaryStatusBuilder.SetWriteIsSuccessful(value);
            Update(_DataStorageSummaryStatusBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
        }
    }

    /// <summary>
    ///     Update
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(Level1Error value)
    {
        try
        {
            _ErrorIndicationBuilder.Reset(GetErrorIndication());
            _ErrorIndicationBuilder.Set(value);
            Update(_ErrorIndicationBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
    }

    /// <summary>
    ///     Update
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(Level2Error value)
    {
        try
        {
            _ErrorIndicationBuilder.Reset(GetErrorIndication());
            _ErrorIndicationBuilder.Set(value);
            Update(_ErrorIndicationBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
    }

    /// <summary>
    ///     Update
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(Level3Error value)
    {
        try
        {
            _ErrorIndicationBuilder.Reset(GetErrorIndication());
            _ErrorIndicationBuilder.Set(value);
            Update(_ErrorIndicationBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Update(StatusWords value)
    {
        try
        {
            _ErrorIndicationBuilder.Reset(GetErrorIndication());
            _ErrorIndicationBuilder.Set(value);
            Update(_ErrorIndicationBuilder.Complete());
        }
        catch (DataElementParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
        catch (CodecParsingException exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
        catch (Exception exception)
        {
            throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
        }
    }

    /// <summary>
    ///     Reset
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Reset(OutcomeParameterSet value)
    {
        Update(value);
    }

    /// <summary>
    ///     Reset
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Reset(UserInterfaceRequestData value)
    {
        Update(value);
    }

    /// <summary>
    ///     Reset
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Reset(ErrorIndication value)
    {
        Update(value);
    }

    #endregion

    #endregion
}