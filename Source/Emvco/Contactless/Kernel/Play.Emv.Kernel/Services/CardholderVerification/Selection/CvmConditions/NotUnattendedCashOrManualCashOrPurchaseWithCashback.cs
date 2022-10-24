using System;

using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record NotUnattendedCashOrManualCashOrPurchaseWithCashback : CvmCondition
{
    #region Static Metadata

    private static readonly Tag[] _RequiredTags = new[] { TerminalType.Tag, TransactionType.Tag };
    public static readonly CvmConditionCode Code = new(2);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => _RequiredTags;

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;

    protected override bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount)
    {
        if (database.IsUnattendedCashTransaction())
            return false;

        if (database.IsManualCashTransaction())
            return false;

        if (database.IsPurchaseTransactionWithCashback())
            return false;

        return true;
    }

    #endregion
}