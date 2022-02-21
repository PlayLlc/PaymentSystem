using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Version number assigned by the payment system for the application
/// </summary>
public record ApplicationVersionNumberTerminal : DataElement<ushort>, IEqualityComparer<ApplicationVersionNumberTerminal>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x9F09;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public ApplicationVersionNumberTerminal(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool IsCardholderVerificationIsSupported() => _Value.IsBitSet(5);
    public bool IsCdaSupported() => _Value.IsBitSet(1);
    public bool IsDdaSupported() => _Value.IsBitSet(6);
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public bool IsIssuerAuthenticationIsSupported19() => _Value.IsBitSet(3);
    public bool IsSdaSupported() => _Value.IsBitSet(7);
    public bool TerminalRiskManagementIsToBePerformed() => _Value.IsBitSet(4);

    #endregion

    #region Serialization

    public static ApplicationVersionNumberTerminal Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationVersionNumberTerminal Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(BerEncodingId, value).ToUInt16Result()
            ?? throw new DataElementNullException(BerEncodingId);

        return new ApplicationVersionNumberTerminal(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Equality

    public bool Equals(ApplicationVersionNumberTerminal? x, ApplicationVersionNumberTerminal? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationVersionNumberTerminal obj) => obj.GetHashCode();

    #endregion
}