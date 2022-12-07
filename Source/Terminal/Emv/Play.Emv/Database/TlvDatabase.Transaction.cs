using System;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Outcomes;

namespace Play.Emv.Database;

public abstract partial class TlvDatabase
{
    #region Instance Members

    /// <exception cref="TerminalException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public Transaction GetTransaction()
    {
        if (!IsActive())
        {
            throw new TerminalException(
                new InvalidOperationException($"A command to initialize the Kernel Database was invoked but the {nameof(TlvDatabase)} is already active"));
        }

        TryGet(AccountType.Tag, out AccountType? accountType);
        TryGet(AmountAuthorizedNumeric.Tag, out AmountAuthorizedNumeric? amountAuthorizedNumeric);
        TryGet(AmountOtherNumeric.Tag, out AmountOtherNumeric? amountOtherNumeric);
        TryGet(LanguagePreference.Tag, out LanguagePreference? languagePreference);
        TryGet(TerminalCountryCode.Tag, out TerminalCountryCode? terminalCountryCode);
        TryGet(TransactionDate.Tag, out TransactionDate? transactionDate);
        TryGet(TransactionTime.Tag, out TransactionTime? transactionTime);
        TryGet(TransactionType.Tag, out TransactionType? transactionType);
        TryGet(TransactionCurrencyCode.Tag, out TransactionCurrencyCode? transactionCurrencyCode);
        TryGet(TransactionCurrencyExponent.Tag, out TransactionCurrencyExponent? transactionCurrencyExponent);

        Outcome outcome = GetOutcome();

        return new Transaction(_TransactionSessionId!.Value, accountType!, amountAuthorizedNumeric!, amountOtherNumeric!, transactionType!, languagePreference!,
            terminalCountryCode!, transactionDate!, transactionTime!, transactionCurrencyExponent!, transactionCurrencyCode!, outcome);
    }

    #endregion
}