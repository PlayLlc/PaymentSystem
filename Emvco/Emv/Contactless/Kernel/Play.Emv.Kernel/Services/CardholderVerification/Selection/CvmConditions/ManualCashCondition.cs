using Play.Ber.Identifiers;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record ManualCashCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(4);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => new[] {PosEntryMode.Tag, TransactionType.Tag};

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;

    /// <exception cref="CodecParsingException"></exception>
    protected override bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount) =>
        database.IsManualCashTransaction();

    #endregion
}