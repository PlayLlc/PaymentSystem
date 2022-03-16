using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.CvmConditions;

internal record AmountInApplicationCurrencyAndOverYValueCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(9);

    #endregion

    #region Instance Values

    protected override Tag[] RequiredData => throw new NotImplementedException();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;
    protected override bool IsConditionSatisfied(IQueryTlvDatabase database) => throw new NotImplementedException();

    #endregion
}