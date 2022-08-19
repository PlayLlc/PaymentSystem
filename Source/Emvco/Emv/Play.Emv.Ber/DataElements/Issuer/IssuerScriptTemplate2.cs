using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains proprietary issuer data for transmission to the ICC after the second GENERATE AC command
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptTemplate2 : DataElement<BigInteger>, IEqualityComparer<IssuerScriptTemplate2>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x72;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;

    #endregion

    #region Constructor

    public IssuerScriptTemplate2(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    public static IssuerScriptTemplate2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override IssuerScriptTemplate2 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static IssuerScriptTemplate2 Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new DataElementParsingException(
                $"The {nameof(IssuerScriptTemplate2)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IssuerScriptTemplate2(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerScriptTemplate2? x, IssuerScriptTemplate2? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerScriptTemplate2 obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}