using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Kernel.Services.Conditions;

namespace Play.Emv.Kernel.Services;

internal record AmountInApplicationCurrencyAndUnderXValueCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(6);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => throw new NotImplementedException();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => CvmConditionCode
    protected override bool IsConditionSatisfied(IQueryTlvDatabase database) => throw new NotImplementedException();

    #endregion
}