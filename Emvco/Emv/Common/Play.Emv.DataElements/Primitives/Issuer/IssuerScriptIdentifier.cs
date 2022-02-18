using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     Identification of the Issuer Script
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptIdentifier : DataElement<uint>, IEqualityComparer<IssuerScriptIdentifier>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F18;
    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;

    #endregion

    #region Constructor

    public IssuerScriptIdentifier(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;

    #endregion

    #region Serialization

    public static IssuerScriptIdentifier Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerScriptIdentifier Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 4;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IssuerScriptIdentifier)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerScriptIdentifier)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new IssuerScriptIdentifier(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerScriptIdentifier? x, IssuerScriptIdentifier? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerScriptIdentifier obj) => obj.GetHashCode();

    #endregion
}