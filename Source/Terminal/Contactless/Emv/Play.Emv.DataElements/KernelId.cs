using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: Contains a value that uniquely identifies each Kernel. There is one occurrence of this data object for
///     each Kernel in the Reader.
/// </summary>
public record KernelId : DataElement<byte>, IEqualityComparer<KernelId>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0xDF810C;
    public static readonly KernelId Unavailable = new(0);

    #endregion

    #region Constructor

    public KernelId(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public ShortKernelId GetShortKernelId() => ShortKernelId.Get(_Value);
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static KernelId Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static KernelId Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 1;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(KernelId)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = codec.Decode(BerEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(KernelId)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new KernelId(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(KernelId? x, KernelId? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(KernelId obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(ShortKernelId left, KernelId right) => left.Equals(right);
    public static bool operator ==(KernelId left, ShortKernelId right) => right.Equals(left);
    public static explicit operator byte(KernelId value) => value._Value;
    public static bool operator !=(ShortKernelId left, KernelId right) => !left.Equals(right);
    public static bool operator !=(KernelId left, ShortKernelId right) => !right.Equals(left);

    #endregion
}