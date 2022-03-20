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

    protected override bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount)
    {
        ApplicationCurrencyCode applicationCurrencyCode = (ApplicationCurrencyCode) database.Get(ApplicationCurrencyCode.Tag);

        TransactionCurrencyCode transactionCurrencyCode = (TransactionCurrencyCode) database.Get(TransactionCurrencyCode.Tag);

        if ((NumericCurrencyCode) applicationCurrencyCode != (NumericCurrencyCode) transactionCurrencyCode)
            return false;

        AmountAuthorizedNumeric transactionAmount = (AmountAuthorizedNumeric) database.Get(AmountAuthorizedNumeric.Tag);

        return transactionAmount.AsMoney((NumericCurrencyCode) applicationCurrencyCode) > yAmount;
    }

    #endregion
}