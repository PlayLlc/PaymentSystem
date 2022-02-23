using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Specifies Terminal Category Codes for the PICC that are specific to <see cref="TerminalCategoryCode.TransitGate" />
///     and <see cref="TerminalCategoryCode.Loyalty" />. If the terminal does not belong to one of those categories, the
///     Terminal Category ID L V is not listed in this object
/// </summary>
public record PoiInformation : DataElement<byte[]>, IEqualityComparer<PoiInformation>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x8B;

    #endregion

    #region Constructor

    public PoiInformation(ReadOnlySpan<byte> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    // BUG: Double check this logic is correct. Is the Terminal Category Code derived from the Merchant Category Code?
    private bool IsTerminalMatch(MerchantCategoryCode merchantCategoryCode) => throw new NotImplementedException();

    public TerminalCategoryCode[] GetTerminalCategoryCodes()
    {
        HashSet<TerminalCategoryCode> buffer = new();
        ReadOnlySpan<byte> temp = _Value;

        while (true)
        {
            if (temp.Length < 2)
                break;

            buffer.Add(TerminalCategoryCode.Get(temp[..1]));
            temp = temp[..(2 + temp[2])];
        }

        return buffer.ToArray();
    }

    #endregion

    #region Serialization

    public static PoiInformation Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static PoiInformation Decode(ReadOnlySpan<byte> value)
    {
        const ushort maxByteLength = 64;

        if (value.Length > maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(PoiInformation)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {maxByteLength} bytes in length");
        }

        return new PoiInformation(value);
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
}