using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Command data field of the GENERATE AC command, coded according to CDOL1.
/// </summary>
public record CardRiskManagementDataObjectList1RelatedData : DataObjectList
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8107;

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    public CardRiskManagementDataObjectList1RelatedData(params TagLength[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static CardRiskManagementDataObjectList1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override CardRiskManagementDataObjectList1 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static CardRiskManagementDataObjectList1 Decode(ReadOnlySpan<byte> value) => new(Codec.DecodeTagLengthPairs(value));

    #endregion
}