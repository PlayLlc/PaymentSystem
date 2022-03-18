using System;

using Play.Ber.Identifiers;
using Play.Emv.Database;
using Play.Emv.DataElements;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record AlwaysCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(0);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => Array.Empty<Tag>();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;
    protected override bool IsConditionSatisfied(IQueryTlvDatabase database, Money xAmount, Money yAmount) => true;

    #endregion
}