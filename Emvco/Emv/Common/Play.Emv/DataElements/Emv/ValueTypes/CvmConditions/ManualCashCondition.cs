using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.CvmConditions;

internal record ManualCashCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(4);

    #endregion

    #region Instance Values

    protected override Tag[] RequiredData => new Tag[] {PosEntryMode.Tag, TransactionType.Tag};

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    protected override bool IsConditionSatisfied(IQueryTlvDatabase database)
    {
        PosEntryMode posEntryMode = PosEntryMode.Decode(database.Get(PosEntryMode.Tag).EncodeValue().AsSpan());
        TransactionType transactionType = TransactionType.Decode(database.Get(TransactionType.Tag).EncodeValue().AsSpan());

        return IsManualCash(posEntryMode, transactionType);
    }

    #endregion
}