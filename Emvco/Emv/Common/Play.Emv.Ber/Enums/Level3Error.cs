using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record Level3Error : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Level3Error> _ValueObjectMap;
    public static readonly Level3Error AmountNotPresent;
    public static readonly Level3Error Ok;
    public static readonly Level3Error Stop;
    public static readonly Level3Error TimeOut;

    #endregion

    #region Constructor

    static Level3Error()
    {
        const byte amountNotPresent = 3;
        const byte ok = 0;
        const byte stop = 2;
        const byte timeOut = 1;

        AmountNotPresent = new Level3Error(amountNotPresent);
        Ok = new Level3Error(ok);
        Stop = new Level3Error(stop);
        TimeOut = new Level3Error(timeOut);
        _ValueObjectMap = new Dictionary<byte, Level3Error> {{amountNotPresent, AmountNotPresent}, {ok, Ok}, {stop, Stop}, {timeOut, TimeOut}}
            .ToImmutableSortedDictionary();
    }

    private Level3Error(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static Level3Error[] GetAll() => _ValueObjectMap.Values.ToArray();
    public static Level3Error Get(byte value) => _ValueObjectMap[value];

    #endregion
}