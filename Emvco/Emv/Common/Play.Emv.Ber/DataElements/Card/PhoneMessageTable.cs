using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The Phone Message Table is a variable length list of entries of eight bytes each, and defines for the selected AID
///     the message and status identifiers as a function of the POS Cardholder Interaction Information. Each entry in the
///     Phone Message Table contains the fields shown in the table below.
/// </summary>
public record PhoneMessageTable : DataElement<byte>, IEqualityComparer<PhoneMessageTable>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8131;

    #endregion

    #region Constructor

    public PhoneMessageTable(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PhoneMessageTable Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PhoneMessageTable Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PhoneMessageTable Decode(ReadOnlySpan<byte> value)
    {
        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new PhoneMessageTable(result);
    }

    #endregion

    #region Equality

    public bool Equals(PhoneMessageTable? x, PhoneMessageTable? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PhoneMessageTable obj) => obj.GetHashCode();

    #endregion
}