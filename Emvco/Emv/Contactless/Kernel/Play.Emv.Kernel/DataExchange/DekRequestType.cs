using System;
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.DataExchange;

public record DekRequestType : EnumObject<Tag>
{
    #region Static Metadata

    // away from card (Enqueued) and sent
    public static readonly DekRequestType DataNeeded = new(Ber.DataElements.DataNeeded.Tag);

    // to card (Dequeued)   
    public static readonly DekRequestType TagsToRead = new(Ber.DataElements.TagsToRead.Tag);

    private static readonly Dictionary<DekRequestType, Func<DataExchangeRequest>> _Defaults = new()
    {
        {DataNeeded, () => new DataNeeded()}, {TagsToRead, () => new TagsToRead()}
    };

    #endregion

    #region Constructor

    public DekRequestType(Tag original) : base(original)
    { }

    private DekRequestType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static DataExchangeRequest GetDefault(DekRequestType type) => _Defaults[type].Invoke();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DekRequestType dekRequestType) => dekRequestType._Value;

    #endregion
}