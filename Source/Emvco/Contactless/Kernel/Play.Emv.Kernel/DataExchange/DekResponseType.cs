using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Kernel.DataExchange;

public record DekResponseType : EnumObject<Tag>
{
    #region Static Metadata

    public static readonly DekResponseType Empty = new();
    public static readonly DekResponseType TagsToWriteBeforeGenAc = new(Ber.DataElements.TagsToWriteBeforeGeneratingApplicationCryptogram.Tag);
    public static readonly DekResponseType TagsToWriteAfterGenAc = new(Ber.DataElements.TagsToWriteAfterGeneratingApplicationCryptogram.Tag);
    public static readonly DekResponseType DataToSend = new(Ber.DataElements.DataToSend.Tag);
    public static readonly DekResponseType DataRecord = new(Ber.DataElements.DataRecord.Tag);
    public static readonly DekResponseType DiscretionaryData = new(Ber.DataElements.DiscretionaryData.Tag);
    public static readonly DekResponseType TornRecord = new(Ber.DataElements.TornRecord.Tag);

    private static readonly Dictionary<DekResponseType, Func<DataExchangeResponse>> _DefaultMap = new()
    {
        {TagsToWriteBeforeGenAc, () => new TagsToWriteBeforeGeneratingApplicationCryptogram()},
        {TagsToWriteAfterGenAc, () => new TagsToWriteAfterGeneratingApplicationCryptogram()},
        {DataToSend, () => new DataToSend()},
        {DataRecord, () => new DataRecord()},
        {DiscretionaryData, () => new DiscretionaryData()},
        {TornRecord, () => Ber.DataElements.TornRecord.Empty}
    };

    private static readonly Dictionary<Tag, DekResponseType> _ValueObjectMap = new()
    {
        {TagsToWriteBeforeGenAc, TagsToWriteBeforeGenAc},
        {TagsToWriteAfterGenAc, TagsToWriteAfterGenAc},
        {DataToSend, DataToSend},
        {DataRecord, DataRecord},
        {DiscretionaryData, DiscretionaryData}
    };

    #endregion

    #region Constructor

    public DekResponseType()
    { }

    public DekResponseType(Tag original) : base(original)
    { }

    private DekResponseType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DekResponseType[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(Tag value, out EnumObject<Tag>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DekResponseType? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    /// <exception cref="TerminalDataException"></exception>
    public static DekResponseType Get(Tag tag)
    {
        if (!_ValueObjectMap.ContainsKey(tag))
            throw new TerminalDataException($"The {nameof(Tag)} argument with the value {tag} could not be recognized for a {nameof(DekResponseType)}");

        return _ValueObjectMap[tag];
    }

    public static DataExchangeResponse GetDefaultList(DekResponseType listType) => _DefaultMap[listType].Invoke();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DekResponseType dekResponseType) => dekResponseType._Value;

    #endregion
}