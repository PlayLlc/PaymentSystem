using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record Level1Error : EnumObject<byte>
{
    #region Static Metadata

    public static readonly Level1Error Empty = new();
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

    public Level1Error()
    { }

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

    protected Level1Error(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out Level1Error? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}