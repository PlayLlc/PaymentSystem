using Play.Ber.Identifiers;
using Play.Core;

namespace Play.Emv.Kernel.DataExchange;

public record DetRequestType : EnumObject<Tag>
{
    #region Static Metadata

    public static readonly DetRequestType DataNeeded = new(DataElements.DataNeeded.Tag);
    public static readonly DetRequestType TagsToRead = new(DataElements.TagsToRead.Tag);

    #endregion

    #region Constructor

    public DetRequestType(Tag original) : base(original)
    { }

    private DetRequestType(byte value) : base(value)
    { }

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DetRequestType detRequestType) => detRequestType._Value;

    #endregion
}