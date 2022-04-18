using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Security.Authentications;

namespace Play.Emv.Terminal.DataExchange;

public record DetRequestType : EnumObject<Tag>
{
    #region Static Metadata

    public static readonly DetRequestType DataNeeded = new(Ber.DataElements.DataNeeded.Tag);
    public static readonly DetRequestType TagsToRead = new(Ber.DataElements.TagsToRead.Tag);

    private static readonly ImmutableSortedDictionary<Tag, DetRequestType> _ValueObjectMap =
        new Dictionary<Tag, DetRequestType>() {{DataNeeded, DataNeeded}, {TagsToRead, TagsToRead}}.ToImmutableSortedDictionary();

    #endregion

    #region Constructor

    public DetRequestType(Tag original) : base(original)
    { }

    private DetRequestType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static DetRequestType[] GetAll() => _ValueObjectMap.Values.ToArray();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DetRequestType detRequestType) => detRequestType._Value;

    #endregion
}