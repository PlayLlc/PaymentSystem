using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the transaction amount above which the transaction is not allowed. This data object is instantiated with
///     Reader Contactless Transaction Limit (On-device CVM) if on device cardholder verification is supported by the Card
///     and with Reader Contactless Transaction Limit (No On-device CVM)otherwise.
/// </summary>
public abstract record ReaderContactlessTransactionLimit : DataElement<ulong>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    protected const byte _ByteLength = 12;

    #endregion

    #region Constructor

    protected ReaderContactlessTransactionLimit(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public Money AsMoney(NumericCurrencyCode numericCurrencyCode) => new(_Value, numericCurrencyCode);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public abstract override Tag GetTag();
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ReaderContactlessTransactionLimit? x, ReaderContactlessTransactionLimit? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ReaderContactlessTransactionLimit obj) => obj.GetHashCode();

    #endregion
}