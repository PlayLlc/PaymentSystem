using Play.Emv.Ber;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

public class ExchangeRelayResistanceDataRApduSignal : RApduSignal
{
    #region Static Metadata

    private const byte _ByteLength = 10;

    #endregion

    #region Constructor

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ExchangeRelayResistanceDataRApduSignal(ReadOnlySpan<byte> value) : base(value.ToArray())
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The expected length was {_ByteLength} but the value provided was {value.Length} bytes in length");
        }
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ExchangeRelayResistanceDataRApduSignal(ReadOnlySpan<byte> value, Level1Error level1Error) : base(value.ToArray(), level1Error)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The expected length was {_ByteLength} but the value provided was {value.Length} bytes in length");
        }
    }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => GetStatusWords() == StatusWords._9000;

    #endregion
}