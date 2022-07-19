using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

/// <summary>
///     ACT DataExchangeSignal. Generate an Answer to Reset, start polling for an PICC or HCE
/// </summary>
public record ActivateKernelRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivateKernelRequest));
    public static readonly ChannelTypeId ChannelTypeId = KernelChannel.Id;

    #endregion

    #region Instance Values

    // Session Info Shit 
    private readonly KernelSessionId _KernelSessionId;

    // RAPDU
    private readonly SelectApplicationDefinitionFileInfoResponse _Rapdu;

    //// Terminal
    //private readonly AccountType _AccountType;
    //private readonly LanguagePreference _LanguagePreference;
    //private readonly TerminalCountryCode _TerminalCountryCode;

    //// Application Shit 
    //private readonly KernelId _KernelId;
    //private readonly TransactionType _TransactionType;
    //private readonly TagsToRead _TagsToRead;
    //private readonly TerminalTransactionQualifiers _TerminalTransactionQualifiers;

    //// Transaction Shit
    //private readonly TransactionDate _TransactionDate;
    //private readonly TransactionTime _TransactionTime;
    //private readonly AmountAuthorizedNumeric _AmountAuthorizedNumeric;
    //private readonly AmountOtherNumeric _AmountOtherNumeric;
    //private readonly TransactionCurrencyExponent _TransactionCurrencyExponent;

    //// Outcome 
    //private readonly DataRecord _DataRecord;
    //private readonly DiscretionaryData _DiscretionaryData;
    //private readonly TerminalVerificationResults _TerminalVerificationResults;
    //private readonly ErrorIndication _ErrorIndication;
    //private readonly OutcomeParameterSet _OutcomeParameterSet;
    //private readonly UserInterfaceRequestData _UserInterfaceRequestData;
    private readonly List<PrimitiveValue> _TransactionValues;

    #endregion

    #region Constructor

    public ActivateKernelRequest(
        KernelSessionId kernelSessionId, PrimitiveValue[] transactionValues, SelectApplicationDefinitionFileInfoResponse rapdu) : base(MessageTypeId,
        ChannelTypeId)
    {
        _KernelSessionId = kernelSessionId;
        _Rapdu = rapdu;
        _TransactionValues = new List<PrimitiveValue>(transactionValues);
        _TransactionValues.AddRange(rapdu.AsPrimitiveValues());

        //_AccountType = transaction.GetAccountType();
        //_LanguagePreference = transaction.GetLanguagePreference();
        //_TerminalCountryCode = transaction.GetTerminalCountryCode();

        //_KernelId = combinationCompositeKey.GetKernelId();
        //_TransactionType = transaction.GetTransactionType();
        //_TagsToRead = tagsToRead ?? new TagsToRead();
        //_TerminalTransactionQualifiers = terminalTransactionQualifiers;
        //_TransactionDate = transaction.GetTransactionDate();
        //_TransactionTime = transaction.GetTransactionTime();
        //_AmountAuthorizedNumeric = transaction.GetAmountAuthorizedNumeric();
        //_AmountOtherNumeric = transaction.GetAmountOtherNumeric();
        //_TransactionCurrencyExponent = transaction.GetTransactionCurrencyExponent();
        //_TerminalVerificationResults = transaction.GetTerminalVerificationResults();
        //_ErrorIndication = transaction.GetErrorIndication();
        //_OutcomeParameterSet = transaction.GetOutcomeParameterSet();

        //_DataRecord = transaction.TryGetDataRecord(out DataRecord? dataRecord) ? dataRecord! : new DataRecord();
        //_DiscretionaryData = transaction.TryGetDiscretionaryData(out DiscretionaryData? discretionaryData) ? discretionaryData! : new DiscretionaryData();
        //_UserInterfaceRequestData = transaction.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData)
        //    ? userInterfaceRequestData!
        //    : UserInterfaceRequestData.GetBuilder().Complete();
    }

    #endregion

    #region Instance Members

    public PrimitiveValue[] GetPrimitiveValues() => _TransactionValues.ToArray();
    public SelectApplicationDefinitionFileInfoResponse GetRapdu() => _Rapdu;
    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();

    #endregion
}