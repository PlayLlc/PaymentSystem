using System;

using Play.Ber.Identifiers;
using Play.Emv.Database;
using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

internal record AmountInApplicationCurrencyAndUnderYValueCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(8);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => throw new NotImplementedException();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;
    protected override bool IsConditionSatisfied(IQueryTlvDatabase database) => throw new NotImplementedException();

    #endregion
}