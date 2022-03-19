using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Identifies the application as described in ISO/IEC 7816-5
/// </summary>
public record ApplicationIdentifier : DataElement<BigInteger>, IEqualityComparer<ApplicationIdentifier>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F06;
    private const byte _MinByteLength = 5;
    private const byte _MaxByteLength = 16;

    #endregion

    #region Constructor

    public ApplicationIdentifier(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.ToByteArray();
    public override PlayEncodingId GetEncodingId() => EncodingId;

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIndicator() =>
        new(PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(_Value.ToByteArray()[..5]));

    public override Tag GetTag() => Tag;

    public bool IsPartialMatch(ApplicationIdentifier other)
    {
        int comparisonLength = GetValueByteCount() < other.GetValueByteCount() ? GetValueByteCount() : other.GetValueByteCount();

        Span<byte> thisBuffer = _Value.ToByteArray();
        Span<byte> otherBuffer = other.AsByteArray();

        for (int i = 0; i < comparisonLength; i++)
        {
            if (thisBuffer[i] != otherBuffer[i])
                return false;
        }

        return true;
    }

    #endregion

    #region Serialization

    public static ApplicationIdentifier Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static ApplicationIdentifier Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new ApplicationIdentifier(result);
    }

    /// <exception cref="BerParsingException"></exception>
    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    #endregion

    #region Equality

    public bool Equals(ApplicationIdentifier? x, ApplicationIdentifier? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationIdentifier obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(ApplicationIdentifier left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ApplicationIdentifier right) => left == right._Value;
    public static bool operator !=(ApplicationIdentifier left, byte right) => !(left == right);
    public static bool operator !=(byte left, ApplicationIdentifier right) => !(left == right);

    #endregion
}