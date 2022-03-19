using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     A data object in the Card that provides the Kernel with a list of data objects that must be passed to the Card in
///     the data field of the GENERATE AC command.
/// </summary>
public record CardRiskManagementDataObjectList1 : DataObjectList
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x8C;
    private static readonly byte _MaxByteLength = 250;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public CardRiskManagementDataObjectList1(byte[] value) : base(value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public static CardRiskManagementDataObjectList1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static CardRiskManagementDataObjectList1 Decode(ReadOnlySpan<byte> value) => new(value.ToArray());

    #endregion
}