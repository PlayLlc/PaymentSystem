using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     A data object in the Card that provides the Kernel with a list of data objects that must be passed to the Card in
///     the data field of the RECOVER AC command.
/// </summary>
public record DataRecoveryDataObjectList : DataObjectList
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F51;
    private static readonly byte _MaxByteLength = 250;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public DataRecoveryDataObjectList(BigInteger value) : base(value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);
    }

    #endregion

    #region Instance Members

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    public DataRecoveryDataObjectListRelatedData AsRelatedData(IReadTlvDatabase database) =>
        DataRecoveryDataObjectListRelatedData.Decode(AsCommandTemplate(database).EncodeValue().AsSpan());

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public static DataRecoveryDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DataRecoveryDataObjectList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataRecoveryDataObjectList Decode(ReadOnlySpan<byte> value) => new(new BigInteger(value));

    #endregion
}