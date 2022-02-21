using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.DataElements.Emv;
using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Globalization;
using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

/// <summary>
///     ACT DataExchangeSignal. Generate an Answer to Reset, start polling for an PICC or HCE
/// </summary>
public record ActivateKernelRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivateKernelRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    private readonly CombinationCompositeKey _CombinationCompositeKey;
    private readonly TerminalTransactionQualifiers _TerminalTransactionQualifiers;
    private readonly SelectApplicationDefinitionFileInfoResponse _FileControlInformation;
    private readonly KernelSessionId _KernelSessionId;
    private readonly StatusWords _StatusWordsForSelectAid;
    private readonly TagsToRead? _TagsToRead;
    private readonly Transaction _Transaction;

    #endregion

    #region Constructor

    public ActivateKernelRequest(
        KernelSessionId kernelSessionId,
        CombinationCompositeKey combinationCompositeKey,
        Transaction transaction,
        TagsToRead? tagsToRead,
        TerminalTransactionQualifiers terminalTransactionQualifiers,
        SelectApplicationDefinitionFileInfoResponse fileControlInformation,
        StatusWords statusWordsForSelectAid) : base(MessageTypeId, ChannelTypeId)
    {
        _KernelSessionId = kernelSessionId;
        _Transaction = transaction;
        _TagsToRead = tagsToRead;
        _CombinationCompositeKey = combinationCompositeKey;
        _TerminalTransactionQualifiers = terminalTransactionQualifiers;
        _FileControlInformation = fileControlInformation;
        _StatusWordsForSelectAid = statusWordsForSelectAid;
    }

    #endregion

    #region Instance Members

    public TagLengthValue[] AsTagLengthValueArray()
    {
        List<TagLengthValue> buffer = new();

        // TODO: Do we need to flatten the primitive values returned from the ICC when
        // TODO: inserting into the DB?
        buffer.AddRange(_FileControlInformation.AsTagLengthValues());

        if (_TagsToRead != null)
            buffer.Add(_TagsToRead.AsTagLengthValue());
        buffer.AddRange(_Transaction.AsTagLengthValueArray());

        return buffer.ToArray();
    }

    public TransactionSessionId GetTransactionSessionId() => _Transaction.GetTransactionSessionId();
    public TerminalVerificationResults GetTerminalVerificationResults() => _Transaction.GetTerminalVerificationResults();
    public Transaction GetTransaction() => _Transaction;
    public SelectApplicationDefinitionFileInfoResponse GetFileControlInformationCardResponse() => _FileControlInformation;
    public AmountAuthorizedNumeric GetAmountAuthorizedNumeric() => _Transaction.GetAmountAuthorizedNumeric();
    public AmountOtherNumeric GetAmountOtherNumeric() => _Transaction.GetAmountOtherNumeric();
    public DedicatedFileName GetApplicationIdentifier() => _CombinationCompositeKey.GetApplicationId();
    public CombinationCompositeKey GetCombinationCompositeKey() => _CombinationCompositeKey;
    public CultureProfile GetCultureProfile() => _Transaction.GetCultureProfile();
    public KernelId GetKernelId() => _CombinationCompositeKey.GetKernelId();
    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public Alpha2LanguageCode GetLanguageCode() => GetCultureProfile().GetAlpha2LanguageCode();
    public ref readonly Outcome GetOutcome() => ref _Transaction.GetOutcome();
    public TerminalTransactionQualifiers GetTerminalTransactionQualifiers() => _TerminalTransactionQualifiers;

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIndicator() =>
        _CombinationCompositeKey.GetRegisteredApplicationProviderIndicator();

    public StatusWords GetStatusWordsForSelectAid() => _StatusWordsForSelectAid;

    public bool TryGetTagsToRead(out TagsToRead? result)
    {
        if (_TagsToRead is null)
        {
            result = null;

            return false;
        }

        result = _TagsToRead;

        return true;
    }

    public Alpha2CountryCode GetTerminalCountryCode() => GetCultureProfile().GetAlpha2CountryCode();
    public Alpha3CurrencyCode GetTransactionCurrencyCode() => GetCultureProfile().GetAlpha3CurrencyCode();
    public TransactionType GetTransactionType() => _CombinationCompositeKey.GetTransactionType();

    #endregion
}