//using System.Numerics;

//using Play.Ber.Codecs;
//using Play.Ber.Identifiers;
//using Play.Ber.InternalFactories;
//using Play.Emv.Ber.Codecs;
//using Play.Emv.DataElements.Exceptions;

//namespace Play.Emv.Acquirer.Elavon.DataElements;

//public record ElavonForexData : ElavonDataElement<BigInteger>
//{
//    #region Static Metadata

//    /// <value>Hex: 0x0012 Decimal: 18</value>
//    public static readonly Tag Tag = 0x0012;

//    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;

//    #endregion

//    #region Constructor

//    public ElavonForexData(BigInteger value) : base(value)
//    { }

//    #endregion

//    #region Instance Members

//    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
//    public override Tag GetTag() => Tag;

//    #endregion

//    #region Serialization

//    public static ElavonForexData Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

//    public static ElavonForexData Decode(ReadOnlySpan<byte> value)
//    {
//        DecodedResult<BigInteger> result = _Codec.Decode(AlphaNumericCodec.Identifier, value).ToBigInteger()
//            ?? throw new DataElementNullException(BerEncodingId);

//        return new ElavonForexData(result.Value);
//    }

//    #endregion

//    #region Equality

//    public bool Equals(ElavonForexData? x, ElavonForexData? y)
//    {
//        if (x is null)
//            return y is null;

//        if (y is null)
//            return false;

//        return x.Equals(y);
//    }

//    public int GetHashCode(ElavonForexData obj) => obj.GetHashCode();

//    #endregion
//}

