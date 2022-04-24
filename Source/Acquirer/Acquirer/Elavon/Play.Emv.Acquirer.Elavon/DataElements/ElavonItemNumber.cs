//using Play.Ber.Codecs;
//using Play.Ber.Exceptions;
//using Play.Ber.Identifiers;
//using Play.Ber.InternalFactories;
//using Play.Emv.Ber.Codecs;
//using Play.Emv.Ber.DataObjects;
//using Play.Emv.DataElements.Exceptions;

//namespace Play.Emv.Acquirer.Elavon.DataElements;

///// <summary>
/////     In a Single Message configuration, Tag 001 (Item Number) will increase in value incrementally from 000 to 999 with
/////     each approved transaction in a given batch. Once the maximum value of 999 is reached the batch must be closed
/////     resulting in the batch number increasing by 1 and the Item Number reverting to 000. The Item Number should be set
/////     to 000 for the first item in a batch
///// </summary>
//public record ElavonItemNumber : DataElement<ushort>
//{
//    #region Static Metadata

//    /// <value>Hex: 0x0001 Decimal: 1</value>
//    public static readonly Tag Tag = 0x0001;

//    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
//    private const byte _ByteLength = 2;

//    #endregion

//    #region Constructor

//    public ElavonItemNumber(ushort value) : base(value)
//    { }

//    #endregion

//    #region Instance Members

//    public override BerEncodingId GetEncodingId() => BerEncodingId;
//    public override Tag GetTag() => Tag;

//    #endregion

//    #region Serialization

//    public static ElavonItemNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerException"></exception>
//    public static ElavonItemNumber Decode(ReadOnlySpan<byte> value)
//    {
//        const byte charLength = 3;

//        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

//        DecodedResult<ushort> result = _Codec.Decode(BerEncodingId, value).ToUInt16Result()
//            ?? throw new DataElementNullException(BerEncodingId);

//        Check.Primitive.ForCharLength(result.CharCount, charLength, Tag);

//        return new ElavonItemNumber(result.Value);
//    }

//    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, (ushort) _Value, _ByteLength);

//    #endregion

//    #region Equality

//    public bool Equals(ElavonItemNumber? x, ElavonItemNumber? y)
//    {
//        if (x is null)
//            return y is null;

//        if (y is null)
//            return false;

//        return x.Equals(y);
//    }

//    public int GetHashCode(ElavonItemNumber obj) => obj.GetHashCode();

//    #endregion
//}

