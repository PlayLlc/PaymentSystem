using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record AmountInApplicationCurrencyAndOverYValueCondition : CvmCondition
{
    #region Static Metadata

    private static readonly Tag[] _RequiredTags = new[] {ApplicationCurrencyCode.Tag, TransactionCurrencyCode.Tag, AmountAuthorizedNumeric.Tag};
    public static readonly CvmConditionCode Code = new(9);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => _RequiredTags;

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
    /// <exception cref="TerminalDataException"></exception>
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