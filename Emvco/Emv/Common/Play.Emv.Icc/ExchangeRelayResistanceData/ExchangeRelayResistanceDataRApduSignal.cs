using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc.ComputeCryptographicChddecksum;

public class ExchangeRelayResistanceDataRApduSignal : RApduSignal
{
    #region Static Metadata

    private const byte _ByteLength = 10;

    #endregion

    #region Constructor

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ExchangeRelayResistanceDataRApduSignal(byte[] value) : base(value)
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
    public override Level1Error GetLevel1Error() => throw new NotImplementedException();

    #endregion
}