using System;

using Play.Ber.Identifiers;
using Play.Emv.Database;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record AmountInApplicationCurrencyAndUnderXValueCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(6);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => throw new NotImplementedException();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;
    protected override bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount)
    {
        ApplicationCurrencyCode applicationCurrencyCode = ApplicationCurrencyCode.Decode(database.Get(ApplicationCurrencyCode.Tag).EncodeValue().AsSpan());
        TransactionCurrencyCode transactionCurrencyCode = TransactionCurrencyCode.Decode(database.Get(TransactionCurrencyCode.Tag).EncodeValue().AsSpan());

        if ((NumericCurrencyCode)applicationCurrencyCode != (NumericCurrencyCode)transactionCurrencyCode)
            return false;

        AmountAuthorizedNumeric transactionAmount = AmountAuthorizedNumeric.Decode(database.Get(AmountAuthorizedNumeric.Tag).EncodeValue().AsSpan());

        return transactionAmount.AsMoney((NumericCurrencyCode)applicationCurrencyCode) < xAmount;
    }

    #endregion
}