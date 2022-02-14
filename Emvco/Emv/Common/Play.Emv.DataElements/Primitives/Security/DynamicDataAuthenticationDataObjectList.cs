using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     List of data objects (tag and length) to be passed to the ICC in the INTERNAL AUTHENTICATE command
/// </summary>
public record DynamicDataAuthenticationDataObjectList : DataElement<byte[]>, IEqualityComparer<DynamicDataAuthenticationDataObjectList>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F49;

    #endregion

    #region Constructor

    public DynamicDataAuthenticationDataObjectList(ReadOnlySpan<byte> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static DynamicDataAuthenticationDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DynamicDataAuthenticationDataObjectList Decode(ReadOnlySpan<byte> value)
    {
        const ushort maxByteLength = 252;

        Check.Primitive.ForMaximumLength(value, maxByteLength, Tag);

        return new DynamicDataAuthenticationDataObjectList(value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(DynamicDataAuthenticationDataObjectList? x, DynamicDataAuthenticationDataObjectList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DynamicDataAuthenticationDataObjectList obj) => obj.GetHashCode();

    #endregion
}