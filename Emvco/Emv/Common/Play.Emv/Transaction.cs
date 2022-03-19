using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Globalization;

namespace Play.Emv;

public class Transaction
{
    #region Instance Values

    private readonly AccountType _AccountType;
    private readonly AmountAuthorizedNumeric _AmountAuthorizedNumeric;
    private readonly AmountOtherNumeric _AmountOtherNumeric;
    private readonly LanguagePreference _LanguagePreference;
    private readonly TerminalCountryCode _TerminalCountryCode;
    private readonly Outcome _Outcome;
    private readonly TransactionDate _TransactionDate;
    private readonly TransactionTime _TransactionTime;
    private readonly TransactionSessionId _TransactionSessionId;
    private readonly TransactionType _TransactionType;

    #endregion

    #region Constructor

    public Transaction(
        TransactionSessionId transactionSessionId,
        AccountType accountType,
        AmountAuthorizedNumeric amountAuthorizedNumeric,
        AmountOtherNumeric amountOtherNumeric,
        TransactionType transactionType,
        LanguagePreference languagePreference,
        TerminalCountryCode terminalCountryCode,
        TransactionDate transactionDate,
        TransactionTime transactionTime)
    {
        _AccountType = accountType;
        _TransactionSessionId = transactionSessionId;
        _AmountAuthorizedNumeric = amountAuthorizedNumeric;
        _AmountOtherNumeric = amountOtherNumeric;
        _TransactionType = transactionType;
        _TransactionDate = transactionDate;
        _TransactionTime = transactionTime;
        _LanguagePreference = languagePreference;
        _TerminalCountryCode = terminalCountryCode;

        _Outcome = new Outcome();
    }

    #endregion

    #region Instance Members

    public AccountType GetAccountType() => _AccountType;
    public TransactionTime GetTransactionTime() => _TransactionTime;
    public OutcomeParameterSet GetOutcomeParameterSet() => _Outcome.GetOutcomeParameterSet();

    public PrimitiveValue[] AsPrimitiveValueArray()
    {
        List<PrimitiveValue> buffer = new()
        {
             
            _AccountType,
            _AmountAuthorizedNumeric,
            _AmountOtherNumeric,
            _LanguagePreference,
            _TerminalCountryCode,
            _TransactionDate,
            _TransactionTime,
            _TransactionType, 
            _AmountAuthorizedNumeric,
            _AmountOtherNumeric,
            _TransactionType,
            _TransactionDate,




        };

        buffer.AddRange(_Outcome.AsPrimitiveValueArray());

        return buffer.ToArray();
    }

    // BUG: This should return the TransactionCurrencyExponent. You can get that from the CultureProfile
    public TransactionCurrencyExponent GetTransactionCurrencyExponent() => throw new NotImplementedException();
    public bool TryGetDataRecord(out DataRecord? result) => _Outcome.TryGetDataRecord(out result);
    public bool TryGetDiscretionaryData(out DiscretionaryData? result) => _Outcome.TryGetDiscretionaryData(out result);
    public AmountAuthorizedNumeric GetAmountAuthorizedNumeric() => _AmountAuthorizedNumeric;
    public AmountOtherNumeric GetAmountOtherNumeric() => _AmountOtherNumeric;
    public LanguagePreference GetLanguagePreference() => _LanguagePreference;
    public TerminalCountryCode GetTerminalCountryCode() => _TerminalCountryCode;
    public TransactionCurrencyCode GetTransactionCurrencyCode() => new(GetCultureProfile());
    public CultureProfile GetCultureProfile() => new(_TerminalCountryCode.AsCountryCode(), _LanguagePreference.GetPreferredLanguage());
    public ref readonly Outcome GetOutcome() => ref _Outcome;
    public TerminalVerificationResults GetTerminalVerificationResults() => _Outcome.GetTerminalVerificationResults();
    public TransactionDate GetTransactionDate() => _TransactionDate;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public bool TryGetUserInterfaceRequestData(out UserInterfaceRequestData? result) => _Outcome.TryGetUserInterfaceRequestData(out result);
    public TransactionType GetTransactionType() => _TransactionType;

    #endregion
}