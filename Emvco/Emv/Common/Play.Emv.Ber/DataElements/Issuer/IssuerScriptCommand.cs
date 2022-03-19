using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains a command for transmission to the ICC
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptCommand : DataElement<BigInteger>, IEqualityComparer<IssuerScriptCommand>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x86;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const ushort _MaxByteLength = 261;

    #endregion

    #region Constructor

    public IssuerScriptCommand(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IssuerScriptCommand Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override IssuerScriptCommand Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IssuerScriptCommand Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new IssuerScriptCommand(result);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerScriptCommand? x, IssuerScriptCommand? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerScriptCommand obj) => obj.GetHashCode();

    #endregion
}