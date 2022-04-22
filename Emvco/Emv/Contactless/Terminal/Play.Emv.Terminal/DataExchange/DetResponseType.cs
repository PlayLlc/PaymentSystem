using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Terminal.DataExchange;

public record DetResponseType : EnumObject<Tag> { public override DetResponseType[] GetAll() => _ValueObjectMap.Values.ToArray(); public override bool TryGet(byte value, out EnumObject<Tag>? result) { if (_ValueObjectMap.TryGetValue(value, out DetResponseType? enumResult)) { result = enumResult; return true; } result = null; return false; }
 public DetResponseType() : base() { } public static readonly DetResponseType Empty = new(); 
#region Static Metadata

    public static readonly DetResponseType DataToSend = new(Ber.DataElements.DataToSend.Tag);

    //public static readonly DetResponseType TornRecord = new(DataElements.TornRecord.Tag);

    private static readonly Dictionary<DetResponseType, Func<DataExchangeResponse>> _Default = new()
    {
        {DataToSend, () => new DataToSend()}

        //{TornRecord, () => new TornRecord()}
    };

    private static readonly Dictionary<Tag, DetResponseType> _ValueObjectMap = new()
    {
        {DataToSend, DataToSend}

        //{TornRecord, () => new TornRecord()}
    };

    #endregion

    #region Constructor

    public DetResponseType(Tag original) : base(original)
    { }

    private DetResponseType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static DetResponseType[] GetAll() => _ValueObjectMap.Values.ToArray();

    /// <summary>
    ///     GetName
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetName(DetResponseType value)
    {
        if (value == DataToSend)
            return nameof(DataToSend);

        throw new InvalidOperationException();
    }

    public static DataExchangeResponse GetDefault(DetResponseType listType) => _Default[listType].Invoke();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DetResponseType detResponseType) => detResponseType._Value;

    #endregion
}