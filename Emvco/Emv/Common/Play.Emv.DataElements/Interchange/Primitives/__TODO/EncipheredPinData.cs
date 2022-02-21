namespace Play.Emv.DataElements.Interchange.__TODO;

/// <summary>
///     The encrypted PIN Block
/// </summary>
//public record EncipheredPinData : InterchangeDataElement<ulong>
//{
//    #region Static Metadata

//    public static readonly DataFieldId DataFieldId = 52;
//    public static readonly InterchangeEncodingId EncodingId = BinaryInterchangeDataFieldCodec.Identifier;
//    private const ushort _ByteLength = 8;

//    #endregion

//    #region Constructor

//    public EncipheredPinData(ulong value) : base(value)
//    { }

//    #endregion

//    #region Instance Members

//    public override DataFieldId GetDataFieldId() => DataFieldId;
//    public override InterchangeEncodingId GetEncodingId() => EncodingId;

//    #endregion

//    #region Serialization

//    public static EncipheredPinData Decode(ReadOnlySpan<byte> value)
//    {
//        Check.DataField.ForExactLength(value, _ByteLength, DataFieldId);

//        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value).ToUInt64Result()
//            ?? throw new InterchangeDataFieldNullException(EncodingId);

//        return new EncipheredPinData(result.Value);
//    }

//    #endregion
//}