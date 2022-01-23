using System;
using System.Collections.Generic;

using Play.Ber.Emv.DataObjects;
using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.DataElements;

namespace Play.Emv.Kernel.DataExchange;

public record DekResponseType : EnumObject<Tag>
{
    #region Static Metadata

    public static readonly DekResponseType TagsToWriteBeforeGenAc = new(DataElements.TagsToWriteBeforeGenAc.Tag);
    public static readonly DekResponseType TagsToWriteAfterGenAc = new(DataElements.TagsToWriteAfterGenAc.Tag);
    public static readonly DekResponseType DataToSend = new(DataElements.DataToSend.Tag);
    public static readonly DekResponseType DataRecord = new(DataElements.DataRecord.Tag);
    public static readonly DekResponseType DiscretionaryData = new(DataElements.DiscretionaryData.Tag);
    public static readonly DekResponseType TornRecord = new(DataElements.TornRecord.Tag);

    private static readonly Dictionary<DekResponseType, Func<DataExchangeResponse>> _Defaults = new()
    {
        {TagsToWriteBeforeGenAc, () => new TagsToWriteBeforeGenAc()},
        {TagsToWriteAfterGenAc, () => new TagsToWriteAfterGenAc()},
        {DataToSend, () => new DataToSend()},
        {DataRecord, () => new DataRecord()},
        {DiscretionaryData, () => new DiscretionaryData()},
        {TornRecord, () => new TornRecord()}
    };

    #endregion

    #region Constructor

    public DekResponseType(Tag original) : base(original)
    { }

    private DekResponseType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static DataExchangeResponse GetDefault(DekResponseType listType) => _Defaults[listType].Invoke();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DekResponseType dekResponseType) => dekResponseType._Value;

    #endregion
}