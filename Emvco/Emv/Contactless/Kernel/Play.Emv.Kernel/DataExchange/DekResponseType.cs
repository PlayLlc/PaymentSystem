using System;
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Kernel.DataExchange;

public record DekResponseType : EnumObject<Tag>
{
    #region Static Metadata

    public static readonly DekResponseType TagsToWriteBeforeGenAc = new(Ber.DataElements.TagsToWriteBeforeGenAc.Tag);
    public static readonly DekResponseType TagsToWriteAfterGenAc = new(Ber.DataElements.TagsToWriteAfterGenAc.Tag);
    public static readonly DekResponseType DataToSend = new(Ber.DataElements.DataToSend.Tag);
    public static readonly DekResponseType DataRecord = new(Ber.DataElements.DataRecord.Tag);
    public static readonly DekResponseType DiscretionaryData = new(Ber.DataElements.DiscretionaryData.Tag);

    private static readonly Dictionary<DekResponseType, Func<DataExchangeResponse>> _DefaultMap = new()
    {
        {TagsToWriteBeforeGenAc, () => new TagsToWriteBeforeGenAc()},
        {TagsToWriteAfterGenAc, () => new TagsToWriteAfterGenAc()},
        {DataToSend, () => new DataToSend()},
        {DataRecord, () => new DataRecord()},
        {DiscretionaryData, () => new DiscretionaryData()}
    };

    private static readonly Dictionary<Tag, DekResponseType> _TagMap = new()
    {
        {TagsToWriteBeforeGenAc, TagsToWriteBeforeGenAc},
        {TagsToWriteAfterGenAc, TagsToWriteAfterGenAc},
        {DataToSend, DataToSend},
        {DataRecord, DataRecord},
        {DiscretionaryData, DiscretionaryData}
    };

    #endregion

    #region Constructor

    public DekResponseType(Tag original) : base(original)
    { }

    private DekResponseType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    public static DekResponseType Get(Tag tag)
    {
        if (!_TagMap.ContainsKey(tag))
        {
            throw new TerminalDataException(
                $"The {nameof(Tag)} argument with the value {tag} could not be recognized for a {nameof(DekResponseType)}");
        }

        return _TagMap[tag];
    }

    public static DataExchangeResponse GetDefaultList(DekResponseType listType) => _DefaultMap[listType].Invoke();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DekResponseType dekResponseType) => dekResponseType._Value;

    #endregion
}