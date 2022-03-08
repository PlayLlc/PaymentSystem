using Play.Ber.Identifiers;
using Play.Core;

namespace Play.Emv.Kernel.DataExchange;

public record DekRequestType : EnumObject<Tag>
{
    #region Static Metadata

    // away from card (Enqueued) and sent
    public static readonly DekRequestType DataNeeded = new(Ber.DataObjects.DataNeeded.Tag);

    // to card (Dequeued)   
    public static readonly DekRequestType TagsToRead = new(DataElements.Emv.Primitives.DataStorage.TornTransaction.TagsToRead.Tag);

    #endregion

    #region Constructor

    public DekRequestType(Tag original) : base(original)
    { }

    private DekRequestType(byte value) : base(value)
    { }

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DekRequestType dekRequestType) => dekRequestType._Value;

    #endregion
}