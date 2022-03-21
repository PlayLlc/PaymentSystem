using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record AmountInApplicationCurrencyAndOverYValueCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(9);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => throw new NotImplementedException();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;

    /// <summary>
    ///     IsConditionSatisfied
    /// </summary>
    /// <param name="database"></param>
    /// <param name="xAmount"></param>
    /// <param name="yAmount"></param>
    /// <returns></returns>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    protected override bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount)
    {
        ApplicationCurrencyCode applicationCurrencyCode = database.Get<ApplicationCurrencyCode>(ApplicationCurrencyCode.Tag);

        TransactionCurrencyCode transactionCurrencyCode = database.Get<TransactionCurrencyCode>(TransactionCurrencyCode.Tag);

        if ((NumericCurrencyCode) applicationCurrencyCode != (NumericCurrencyCode) transactionCurrencyCode)
            return false;

        AmountAuthorizedNumeric transactionAmount = database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        return transactionAmount.AsMoney((NumericCurrencyCode) applicationCurrencyCode) > yAmount;
    }

    #endregion
}