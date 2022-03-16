using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Kernel.Services.Conditions;

namespace Play.Emv.Kernel.Services;

internal record NotUnattendedCashOrManualCashOrPurchaseWithCashback : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(2);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => throw new NotImplementedException();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;
    protected override bool IsConditionSatisfied(IQueryTlvDatabase database) => throw new NotImplementedException();

    #endregion
}