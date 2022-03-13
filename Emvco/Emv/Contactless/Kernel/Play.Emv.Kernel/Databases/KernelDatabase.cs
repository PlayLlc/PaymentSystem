﻿using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Outcomes;
using Play.Emv.Security.Certificates;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel.Databases;

public abstract class KernelDatabase : IActivateKernelDatabase, IDeactivateKernelDatabase, IQueryKernelDatabase, IQueryTlvDatabase
{
    #region Instance Values

    protected readonly IKernelCertificateDatabase _KernelCertificateDatabase;
    protected readonly ITlvDatabase _TlvDatabase;
    protected KernelSessionId? _KernelSessionId;
    protected IHandleTerminalRequests _TerminalEndpoint;
    protected OutcomeParameterSet.Builder _OutcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
    protected UserInterfaceRequestData.Builder _UserInterfaceRequestDataBuilder = UserInterfaceRequestData.GetBuilder();
    protected ErrorIndication.Builder _ErrorIndicationBuilder = ErrorIndication.GetBuilder();
    protected TerminalVerificationResults.Builder _TerminalVerificationResultBuilder = TerminalVerificationResults.GetBuilder();

    #endregion

    #region Constructor

    protected KernelDatabase(
        IHandleTerminalRequests terminalEndpoint,
        ITlvDatabase tlvDatabase,
        IKernelCertificateDatabase kernelCertificateDatabase)
    {
        _TerminalEndpoint = terminalEndpoint;
        _TlvDatabase = tlvDatabase;
        _KernelCertificateDatabase = kernelCertificateDatabase;
    }

    #endregion

    #region Instance Members

    public abstract KernelConfiguration GetKernelConfiguration();

    /// <summary>
    ///     Activate
    /// </summary>
    /// <param name="kernelSessionId"></param>
    /// <param name="transaction"></param>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual void Activate(KernelSessionId kernelSessionId, Transaction transaction)
    {
        if (IsActive())
        {
            throw new InvalidOperationException(
                $"A command to initialize the Kernel Database was invoked but the {nameof(KernelDatabase)} is already active");
        }

        _KernelSessionId = kernelSessionId;

        Update(transaction.AsTagLengthValueArray());
    }

    /// <summary>
    ///     Resets the transient values in the database to their default values. The persistent values
    ///     will remain unchanged during the database lifetime
    /// </summary>
    public virtual void Deactivate()
    {
        _TlvDatabase.Clear();
        _KernelCertificateDatabase.PurgeRevokedCertificates();
    }

    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="TerminalDataException"></exception>
    public virtual TagLengthValue Get(Tag tag)
    {
        if (!IsActive())
            throw new TerminalDataException($"The method {nameof(Get)} cannot be accessed because {nameof(KernelDatabase)} is not active");

        return _TlvDatabase.Get(tag);
    }

    /// <summary>
    ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
    /// </summary>
    /// <param name="tag"></param>
    public abstract bool IsKnown(Tag tag);

    /// <summary>
    ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
    ///     zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public virtual bool IsPresent(Tag tag)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(IsPresent)} cannot be accessed because {nameof(KernelDatabase)} is not active");
        }

        return _TlvDatabase.IsPresent(tag);
    }

    /// <summary>
    ///     Returns TRUE if:
    ///     • The Database includes a data object with the provided <see cref="Tag" />
    ///     • The length of the data object is non-zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public virtual bool IsPresentAndNotEmpty(Tag tag)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(IsPresentAndNotEmpty)} cannot be accessed because {nameof(KernelDatabase)} is not active");
        }

        return _TlvDatabase.IsPresentAndNotEmpty(tag);
    }

    /// <summary>
    ///     Indicates if the <see cref="CaPublicKeyCertificate" /> is currently valid. If the current date and time
    ///     is before the certificate's active date or after the certificate's expiry date, then the certificate is
    ///     revoked. Certificates can also be revoked by the issuer
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    public virtual bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(IsRevoked)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        return _KernelCertificateDatabase!.IsRevoked(rid, caPublicKeyIndex);
    }

    /// <summary>
    ///     Updates the <see cref="IKernelCertificateDatabase" /> by removing any <see cref="CaPublicKeyCertificate" />
    ///     that has expired since the last time they were checked
    /// </summary>
    public virtual void PurgeRevokedCertificates()
    {
        if (!IsActive())
            return;

        _KernelCertificateDatabase.PurgeRevokedCertificates();
    }

    /// <summary>
    ///     Attempts to get the <see cref="CaPublicKeyCertificate" /> associated with the
    ///     <param name="rid" />
    ///     and
    ///     <param name="index"></param>
    ///     provided. If the <see cref="CaPublicKeyCertificate" /> is revoked or none
    ///     can be found then the return value will be false
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    public virtual bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        return _KernelCertificateDatabase.TryGet(rid, index, out result);
    }

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="DatabaseValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    /// <exception cref="TerminalDataException"></exception>
    public virtual bool TryGet(Tag tag, out TagLengthValue? result)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        return _TlvDatabase.TryGet(tag, out result);
    }

    public bool TryGetKernelSessionId(out KernelSessionId? result)
    {
        if (_KernelSessionId == null)
        {
            result = null;

            return false;
        }

        result = _KernelSessionId;

        return true;
    }

    /// <summary>
    ///     Updates the database with the
    ///     <param name="value"></param>
    ///     if it is a recognized object and discards the value if
    ///     it is not recognized
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="TerminalDataException"></exception>
    public virtual void Update(TagLengthValue value)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(Update)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        _TlvDatabase.Update(value);
    }

    /// <summary>
    ///     Updates the the database with all recognized
    ///     <param name="values" />
    ///     provided it is not recognized
    /// </summary>
    /// <param name="values"></param>
    /// <exception cref="TerminalDataException"></exception>
    public virtual void Update(TagLengthValue[] values)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(Update)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        _TlvDatabase.UpdateRange(values);
    }

    /// <summary>
    ///     Initialize
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="TerminalDataException"></exception>
    public virtual void Initialize(Tag tag)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(Initialize)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        _TlvDatabase.Update(new DatabaseValue(tag));
    }

    protected bool IsActive() => _KernelSessionId != null;

    #endregion

    #region Configuration

    /// <summary>
    ///     IDS builds the reading and writing functions into existing payment commands (GET PROCESSING OPTIONS and GENERATE
    ///     AC). The command-response sequence exchanged between the Card and Kernel is therefore unchanged from a normal
    ///     purchase transaction. It also addresses the security mechanisms of those exchanges.
    /// </summary>
    /// <returns></returns>
    /// <remarks>EMV Book C-2 Section 3.3</remarks>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsIntegratedDataStorageSupported() =>
        IsPresent(DataStorageRequestedOperatorId.Tag) && IsPresentAndNotEmpty(DataStorageVersionNumberTerminal.Tag);

    /// <summary>
    ///     Through the number of entries possible in the torn transaction log, indicated by the value of data object Max
    ///     Number of Torn Transaction Log Records If Max Number of Torn Transaction Log Records is present and set to a value
    ///     different from zero, then torn transaction recovery is supported.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsTornTransactionRecoverySupported() =>
        IsPresentAndNotEmpty(MaxNumberOfTornTransactionLogRecords.Tag)
        && (Get(MaxNumberOfTornTransactionLogRecords.Tag).GetValueByteCount() > 0);

    public bool IsEmvModeSupported() => GetKernelConfiguration().IsEmvModeSupported();
    public bool IsMagstripeModeSupported() => GetKernelConfiguration().IsMagstripeModeSupported();
    public bool IsOnDeviceCardholderVerificationSupported() => GetKernelConfiguration().IsOnDeviceCardholderVerificationSupported();
    public bool IsRelayResistanceProtocolSupported() => GetKernelConfiguration().IsRelayResistanceProtocolSupported();
    public bool IsReadAllRecordsActivated() => GetKernelConfiguration().IsReadAllRecordsActivated();

    #endregion

    #region Outcome

    public void CreateEmvDiscretionaryData(DataExchangeKernelService dataExchanger)
    {
        KernelOutcome.CreateEmvDiscretionaryData(this, dataExchanger);
    }

    public void CreateMagstripeDiscretionaryData(DataExchangeKernelService dataExchanger)
    {
        KernelOutcome.CreateMagstripeDiscretionaryData(this, dataExchanger);
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public ErrorIndication GetErrorIndication() => ErrorIndication.Decode(Get(ErrorIndication.Tag).EncodeValue().AsSpan());

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private OutcomeParameterSet GetOutcomeParameterSet() => OutcomeParameterSet.Decode(Get(OutcomeParameterSet.Tag).EncodeValue().AsSpan());

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private TerminalVerificationResults GetTerminalVerificationResults() =>
        TerminalVerificationResults.Decode(Get(TerminalVerificationResults.Tag).EncodeValue().AsSpan());

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private UserInterfaceRequestData GetUserInterfaceRequestData()
    {
        if (IsPresentAndNotEmpty(UserInterfaceRequestData.Tag))
        {
            UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

            return builder.Complete();
        }

        return UserInterfaceRequestData.Decode(Get(UserInterfaceRequestData.Tag).EncodeValue().AsSpan());
    }

    /// <summary>
    ///     GetDataRecord
    /// </summary>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    private DataRecord? GetDataRecord()
    {
        if (IsPresentAndNotEmpty(DataRecord.Tag))
            return null;

        return DataRecord.Decode(Get(DataRecord.Tag).EncodeValue().AsSpan());
    }

    /// <summary>
    ///     GetDiscretionaryData
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private DiscretionaryData? GetDiscretionaryData()
    {
        if (IsPresentAndNotEmpty(DiscretionaryData.Tag))
            return null;

        return DiscretionaryData.Decode(Get(DiscretionaryData.Tag).EncodeValue().AsSpan());
    }

    #region Write Outcome

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(MessageIdentifier value)
    {
        _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
        _UserInterfaceRequestDataBuilder.Set(value);
        Update(_UserInterfaceRequestDataBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(Status value)
    {
        _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
        _UserInterfaceRequestDataBuilder.Set(value);
        Update(_UserInterfaceRequestDataBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(MessageHoldTime value)
    {
        _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
        _UserInterfaceRequestDataBuilder.Set(value);
        Update(_UserInterfaceRequestDataBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(StatusOutcome value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.Set(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(CvmPerformedOutcome value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.Set(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(OnlineResponseOutcome value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.Set(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(FieldOffRequestOutcome value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.Set(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(StartOutcome value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.Set(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Set(TerminalVerificationResult value)
    {
        _TerminalVerificationResultBuilder.Reset(GetTerminalVerificationResults());
        _TerminalVerificationResultBuilder.Set(value);
        Update(_TerminalVerificationResultBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void SetUiRequestOnRestartPresent(bool value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void SetIsDataRecordPresent(bool value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.SetIsDataRecordPresent(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void SetIsDiscretionaryDataPresent(bool value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.SetIsDiscretionaryDataPresent(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void SetIsReceiptPresent(bool value)
    {
        _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
        _OutcomeParameterSetBuilder.SetIsUiRequestOnRestartPresent(value);
        Update(_OutcomeParameterSetBuilder.Complete());
    }

    /// <summary>
    ///     Update
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(Level1Error value)
    {
        _ErrorIndicationBuilder.Reset(GetErrorIndication());
        _ErrorIndicationBuilder.Set(value);
        Update(_ErrorIndicationBuilder.Complete());
    }

    /// <summary>
    ///     Update
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(Level2Error value)
    {
        _ErrorIndicationBuilder.Reset(GetErrorIndication());
        _ErrorIndicationBuilder.Set(value);
        Update(_ErrorIndicationBuilder.Complete());
    }

    /// <summary>
    ///     Update
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(Level3Error value)
    {
        _ErrorIndicationBuilder.Reset(GetErrorIndication());
        _ErrorIndicationBuilder.Set(value);
        Update(_ErrorIndicationBuilder.Complete());
    }

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void Update(StatusWords value)
    {
        _ErrorIndicationBuilder.Reset(GetErrorIndication());
        _ErrorIndicationBuilder.Set(value);
        Update(_ErrorIndicationBuilder.Complete());
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

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public Outcome GetOutcome() =>
        new(GetErrorIndication(), GetOutcomeParameterSet(), GetDataRecord(), GetDiscretionaryData(), GetUserInterfaceRequestData());

    #endregion
}