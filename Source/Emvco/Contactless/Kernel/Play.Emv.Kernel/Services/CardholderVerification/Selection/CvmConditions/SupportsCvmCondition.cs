using System;

using Play.Ber.Tags;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record SupportsCvmCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(3);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => throw new NotImplementedException();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;

    protected override bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount) =>
        database.IsNoCardVerificationMethodRequiredSupported();

    #endregion
}