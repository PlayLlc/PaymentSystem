using System.Collections.Immutable;

using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains a binary number that indicates which of the application�s certification authority public keys and its
///     associated
///     algorithm is to be used
/// </summary>
public record CaPublicKeyIndex : DataElement<byte>, IEqualityComparer<CaPublicKeyIndex>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CaPublicKeyIndex> _ValueObjectMap;
    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly CaPublicKeyIndex Five;
    public static readonly CaPublicKeyIndex Four;
    public static readonly CaPublicKeyIndex One;
    public static readonly CaPublicKeyIndex Six;
    public static readonly Tag Tag = 0x9F22;
    public static readonly CaPublicKeyIndex Three;
    public static readonly CaPublicKeyIndex Two;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    static CaPublicKeyIndex()
    {
        const byte one = 1;
        const byte two = 2;
        const byte three = 3;
        const byte four = 4;
        const byte five = 5;
        const byte six = 6;

        One = new CaPublicKeyIndex(one);
        Two = new CaPublicKeyIndex(two);
        Three = new CaPublicKeyIndex(three);
        Four = new CaPublicKeyIndex(four);
        Five = new CaPublicKeyIndex(five);
        Six = new CaPublicKeyIndex(six);
        _ValueObjectMap =
            new Dictionary<byte, CaPublicKeyIndex> {{one, One}, {two, Two}, {three, Three}, {four, Four}, {five, Five}, {six, Six}}
                .ToImmutableSortedDictionary();
    }

    public CaPublicKeyIndex(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static CaPublicKeyIndex Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CaPublicKeyIndex Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<byte>
            ?? throw new DataElementNullException(BerEncodingId);

        return new CaPublicKeyIndex(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Equality

    public bool Equals(CaPublicKeyIndex? x, CaPublicKeyIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CaPublicKeyIndex obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CaPublicKeyIndex value) => value._Value;
    public static explicit operator CaPublicKeyIndex(byte value) => new(value);

    #endregion
}