using System;
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.Terminal.DataExchange;

public record DetResponseType : EnumObject<Tag>
{
    #region Static Metadata

    public static readonly DetResponseType DataToSend = new(Ber.DataObjects.DataToSend.Tag);

    //public static readonly DetResponseType TornRecord = new(DataElements.TornRecord.Tag);

    private static readonly Dictionary<DetResponseType, Func<DataExchangeResponse>> _Defaults = new()
    {
        {DataToSend, () => new DataToSend()}

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

    /// <summary>
    /// GetName
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

    public static DataExchangeResponse GetDefault(DetResponseType listType) => _Defaults[listType].Invoke();

    #endregion

    #region Operator Overrides

    public static explicit operator Tag(DetResponseType detResponseType) => detResponseType._Value;

    #endregion
}