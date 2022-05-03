using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Kernel.DataExchange;

public record DekRequestType : EnumObject<Tag>
{
    #region Static Metadata

    public static readonly DekRequestType Empty = new();

    // away from card (Enqueued) and sent
    public static readonly DekRequestType DataNeeded = new(Ber.DataElements.DataNeeded.Tag);

    // to card (Dequeued)   
    public static readonly DekRequestType TagsToRead = new(Ber.DataElements.TagsToRead.Tag);

    private static readonly Dictionary<Tag, Func<DataExchangeRequest>> _ListMap = new()
    {
        {DataNeeded, () => new DataNeeded()}, {TagsToRead, () => new TagsToRead()}
    };

    private static readonly Dictionary<Tag, DekRequestType> _ValueObjectMap = new() {{DataNeeded, DataNeeded}, {TagsToRead, TagsToRead}};

    #endregion

    #region Constructor

    public DekRequestType()
    { }

    public DekRequestType(Tag original) : base(original)
    { }

    private DekRequestType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DekRequestType[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(Tag value, out EnumObject<Tag>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DekRequestType? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static DataExchangeRequest GetDefaultList(DekRequestType type) => _ListMap[type].Invoke();

    /// <exception cref="TerminalDataException"></exception>
    public static DekRequestType Get(Tag tag)
    {
        if (!_ValueObjectMap.ContainsKey(tag))
            throw new TerminalDataException($"The {nameof(Tag)} argument with the value {tag} could not be recognized for a {nameof(DekRequestType)}");

        return _ValueObjectMap[tag];
    }

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DekRequestType dekRequestType) => dekRequestType._Value;

    #endregion
}