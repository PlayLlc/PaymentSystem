using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

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
    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;

    #endregion

    #region Constructor

    public IssuerScriptTemplate2(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;

    #endregion

    #region Serialization

    public static IssuerScriptTemplate2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerScriptTemplate2 Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<BigInteger> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerScriptTemplate2)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

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
}