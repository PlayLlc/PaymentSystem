using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record class UnattendedCash : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(1);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => Array.Empty<Tag>();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;
    protected override bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount) => true;

    #endregion
}