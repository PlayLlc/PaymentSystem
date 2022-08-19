using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Specifies Terminal Category Codes for the PICC that are specific to
///     <see cref="TerminalCategoryCodes.TransitGate" />
///     and <see cref="TerminalCategoryCodes.Loyalty" />. If the terminal does not belong to one of those categories, the
///     Terminal Category ID L V is not listed in this object
/// </summary>
public record PoiInformation : DataElement<BigInteger>, IEqualityComparer<PoiInformation>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x8B;
    private const byte _MaxByteLength = 64;

    #endregion

    #region Constructor

    public PoiInformation(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PoiInformation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PoiInformation Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PoiInformation Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new PoiInformation(result);
    }

    #endregion

    #region Equality

    public bool Equals(PoiInformation? x, PoiInformation? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PoiInformation obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    // BUG: Double check this logic is correct. Is the Terminal Category Code derived from the Merchant Category Code?
    private bool IsTerminalMatch(MerchantCategoryCode merchantCategoryCode) => throw new NotImplementedException();

    public TerminalCategoryCodes[] GetTerminalCategoryCodes()
    {
        HashSet<TerminalCategoryCodes> buffer = new();
        ReadOnlySpan<byte> temp = _Value.ToByteArray();

        while (true)
        {
            if (temp.Length < 2)
                break;

            buffer.Add(TerminalCategoryCodes.Get(temp[..1]));
            temp = temp[..(2 + temp[2])];
        }

        return buffer.ToArray();
    }

    #endregion
}