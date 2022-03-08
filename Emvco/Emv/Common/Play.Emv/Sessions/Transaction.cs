using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv.Primitives.Acquirer;
using Play.Emv.DataElements.Emv.Primitives.Outcome;
using Play.Emv.DataElements.Emv.Primitives.Terminal;
using Play.Emv.DataElements.Emv.Primitives.Transaction;
using Play.Emv.DataElements.Emv.ValueTypes;
using Play.Emv.Icc;
using Play.Emv.Outcomes;
using Play.Globalization;

namespace Play.Emv.Sessions;

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

    public TagLengthValue[] AsTagLengthValueArray()
    {
        List<TagLengthValue> buffer = new()
        {
            _AmountAuthorizedNumeric.AsTagLengthValue(),
            _AmountOtherNumeric.AsTagLengthValue(),
            _TransactionType.AsTagLengthValue(),
            _TransactionDate.AsTagLengthValue()
        };

        buffer.AddRange(_Outcome.AsTagLengthValueArray());

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

    public void Update(TerminalVerificationResult value)
    {
        _Outcome.Update(value);
    }

    public void Update(OutcomeParameterSet.Builder value)
    {
        _Outcome.Update(value);
    }

    public void Update(UserInterfaceRequestData.Builder value)
    {
        _Outcome.Update(value);
    }

    public void Reset(TerminalVerificationResults value)
    {
        _Outcome.Reset(value);
    }

    public void Reset(ErrorIndication errorIndication)
    {
        _Outcome.Reset(errorIndication);
    }

    public void Reset(OutcomeParameterSet outcomeParameterSet)
    {
        _Outcome.Reset(outcomeParameterSet);
    }

    public void Reset(UserInterfaceRequestData userInterfaceRequestData)
    {
        _Outcome.Reset(userInterfaceRequestData);
    }

    public TransactionDate GetTransactionDate() => _TransactionDate;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public bool TryGetUserInterfaceRequestData(out UserInterfaceRequestData? result) => _Outcome.TryGetUserInterfaceRequestData(out result);
    public TransactionType GetTransactionType() => _TransactionType;

    public void Update(Level1Error level1Error)
    {
        _Outcome.Update(level1Error);
    }

    public void Update(Level2Error level2Error)
    {
        _Outcome.Update(level2Error);
    }

    public void Update(Level3Error level3Error)
    {
        _Outcome.Update(level3Error);
    }

    #endregion
}