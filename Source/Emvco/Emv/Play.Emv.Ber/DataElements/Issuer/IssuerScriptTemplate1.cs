using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains proprietary issuer data for transmission to the ICC before the second GENERATE AC command
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptTemplate1 : DataElement<BigInteger>, IEqualityComparer<IssuerScriptTemplate1>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x71;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;

    #endregion

    #region Constructor

    public IssuerScriptTemplate1(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => PlayCodec.BinaryCodec.GetByteCount(_Value);
    public override ushort GetValueByteCount() => PlayCodec.BinaryCodec.GetByteCount(_Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static IssuerScriptTemplate1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IssuerScriptTemplate1 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static IssuerScriptTemplate1 Decode(ReadOnlySpan<byte> value)
    {
        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new IssuerScriptTemplate1(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);

    #endregion

    #region Equality

    public bool Equals(IssuerScriptTemplate1? x, IssuerScriptTemplate1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerScriptTemplate1 obj) => obj.GetHashCode();

    #endregion
}