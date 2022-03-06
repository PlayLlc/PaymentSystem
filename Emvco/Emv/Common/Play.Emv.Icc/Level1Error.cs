using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Icc;

public record Level1Error : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Level1Error> _ValueObjectMap;
    public static readonly Level1Error Ok;
    public static readonly Level1Error ProtocolError;
    public static readonly Level1Error TimeOutError;

    /// <summary>
    ///     An error occurred during transmission such as a Card Collision the card was removed too quickly
    /// </summary>
    public static readonly Level1Error TransmissionError;

    #endregion

    #region Constructor

    static Level1Error()
    {
        const byte ok = 0;
        const byte protocolError = 3;
        const byte timeOutError = 1;
        const byte transmissionError = 2;

        Ok = new Level1Error(ok);
        ProtocolError = new Level1Error(protocolError);
        TimeOutError = new Level1Error(timeOutError);
        TransmissionError = new Level1Error(transmissionError);
        _ValueObjectMap = new Dictionary<byte, Level1Error>
        {
            {ok, Ok}, {protocolError, ProtocolError}, {timeOutError, TimeOutError}, {transmissionError, TransmissionError}
        }.ToImmutableSortedDictionary();
    }

    private Level1Error(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static Level1Error Get(byte value) => _ValueObjectMap[value];

    #endregion
}