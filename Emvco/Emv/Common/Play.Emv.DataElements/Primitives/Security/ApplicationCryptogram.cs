using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElementss;

public record ApplicationCryptogram : DataElement<ulong>, IEqualityComparer<ApplicationCryptogram>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F26;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    public ApplicationCryptogram(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static ApplicationCryptogram Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationCryptogram Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new DataElementNullException(BerEncodingId);

        return new ApplicationCryptogram(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Equality

    public bool Equals(ApplicationCryptogram? x, ApplicationCryptogram? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationCryptogram obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator ApplicationCryptogram(byte value) => new(value);

    #endregion
}