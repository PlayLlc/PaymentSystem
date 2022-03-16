using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.CvmConditions;

internal record AmountInApplicationCurrencyAndUnderXValueCondition : CvmCondition
{
    public static readonly CvmConditionCode Code = new CvmConditionCode(6);
    protected override Tag[] RequiredData => throw new NotImplementedException();

    public override CvmConditionCode GetConditionCode() => CvmConditionCode
    protected override bool IsConditionSatisfied(IQueryTlvDatabase database) => throw new NotImplementedException();
}