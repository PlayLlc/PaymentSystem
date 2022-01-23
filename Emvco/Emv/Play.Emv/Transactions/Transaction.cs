using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Outcomes;
using Play.Emv.Sessions;
using Play.Globalization;
using Play.Icc.Emv;

namespace Play.Emv.Transactions;

public class Transaction
{
    #region Instance Values

    private readonly AmountAuthorizedNumeric _AmountAuthorizedNumeric;
    private readonly AmountOtherNumeric _AmountOtherNumeric;
    private readonly LanguagePreference _LanguagePreference;
    private readonly TerminalCountryCode _TerminalCountryCode;
    private readonly Outcome _Outcome;
    private readonly TransactionDate _TransactionDate;
    private readonly TransactionSessionId _TransactionSessionId;
    private readonly TransactionType _TransactionType;

    #endregion

    #region Constructor

    public Transaction(
        TransactionSessionId transactionSessionId,
        AmountAuthorizedNumeric amountAuthorizedNumeric,
        AmountOtherNumeric amountOtherNumeric,
        TransactionType transactionType,
        LanguagePreference languagePreference,
        TerminalCountryCode terminalCountryCode,
        TransactionDate transactionDate)
    {
        _TransactionSessionId = transactionSessionId;
        _AmountAuthorizedNumeric = amountAuthorizedNumeric;
        _AmountOtherNumeric = amountOtherNumeric;
        _TransactionType = transactionType;
        _TransactionDate = transactionDate;
        _LanguagePreference = languagePreference;
        _TerminalCountryCode = terminalCountryCode;

        _Outcome = new Outcome();
    }

    #endregion

    #region Instance Members

    public OutcomeParameterSet GetOutcomeParameterSet()
    {
        return _Outcome.GetOutcomeParameterSet();
    }

    public TagLengthValue[] AsTagLengthValueArray()
    {
        List<TagLengthValue> buffer = new()
        {
            _AmountAuthorizedNumeric.AsTagLengthValue(), _AmountOtherNumeric.AsTagLengthValue(), _TransactionType.AsTagLengthValue(),
            _TransactionDate.AsTagLengthValue()
        };

        buffer.AddRange(_Outcome.AsTagLengthValueArray());

        return buffer.ToArray();
    }

    // BUG: This should return the TransactionCurrencyExponent. You can get that from the CultureProfile
    public TransactionCurrencyExponent GetTransactionCurrencyExponent()
    {
        throw new NotImplementedException();
    }

    public bool TryGetDataRecord(out DataRecord? result)
    {
        return _Outcome.TryGetDataRecord(out result);
    }

    public bool TryGetDiscretionaryData(out DiscretionaryData? result)
    {
        return _Outcome.TryGetDiscretionaryData(out result);
    }

    public AmountAuthorizedNumeric GetAmountAuthorizedNumeric()
    {
        return _AmountAuthorizedNumeric;
    }

    public AmountOtherNumeric GetAmountOtherNumeric()
    {
        return _AmountOtherNumeric;
    }

    public LanguagePreference GetLanguagePreference()
    {
        return _LanguagePreference;
    }

    public TerminalCountryCode GetTerminalCountryCode()
    {
        return _TerminalCountryCode;
    }

    public TransactionCurrencyCode GetTransactionCurrencyCode()
    {
        return new TransactionCurrencyCode(GetCultureProfile());
    }

    public CultureProfile GetCultureProfile()
    {
        return new CultureProfile(_TerminalCountryCode.AsCountryCode(), _LanguagePreference.GetPreferredLanguage());
    }

    public ref readonly Outcome GetOutcome()
    {
        return ref _Outcome;
    }

    public TerminalVerificationResults GetTerminalVerificationResults()
    {
        return _Outcome.GetTerminalVerificationResults();
    }

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

    public TransactionDate GetTransactionDate()
    {
        return _TransactionDate;
    }

    public TransactionSessionId GetTransactionSessionId()
    {
        return _TransactionSessionId;
    }

    public bool TryGetUserInterfaceRequestData(out UserInterfaceRequestData? result)
    {
        return _Outcome.TryGetUserInterfaceRequestData(out result);
    }

    public TransactionType GetTransactionType()
    {
        return _TransactionType;
    }

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