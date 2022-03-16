using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Kernel.Services.Conditions;

namespace Play.Emv.Kernel.Services;

internal record AmountInApplicationCurrencyAndUnderXValueCondition : CvmCondition
{
    public static readonly CvmConditionCode Code = new CvmConditionCode(6);
    protected override Tag[] _RequiredData => throw new NotImplementedException();

    public override CvmConditionCode GetConditionCode() => CvmConditionCode
    protected override bool IsConditionSatisfied(IQueryTlvDatabase database) => throw new NotImplementedException();
}