//using Play.Ber.Codecs;
//using Play.Emv.Ber.DataObjects;

//namespace Play.Emv.Acquirer.Elavon.DataElements;

//public abstract record ElavonDataElement<T> : DataElement<T>
//{
//    #region Constructor

//    protected ElavonDataElement(T value) : base(value)
//    { }

//    #endregion

//    #region Instance Members

//    public new int GetTagLengthValueByteCount() => base.GetTagLengthValueByteCount() + 1;

//    #endregion

//    #region Serialization

//    /// <summary>
//    ///     Encodes this objects content as the Value field of a Tag-Length-Value encoding
//    /// </summary>
//    /// <returns></returns>
//    public new byte[] EncodeTagLengthValue()
//    {
//        Span<byte> a = _Codec.EncodeValue(GetEncodingId(), _Value!).AsSpan();

//        // HACK: Extend the BerCodec and EmvCodecs to accept a Span<byte> buffer. This will allow us to populate the encoded value without creating a new byte array instance on the heap
//        byte[] result = new byte[a.Length + 1];
//        a.CopyTo(result[1..]);

//        return result;
//    }

//    public void EncodeTagLengthValue(Span<byte> buffer, ref int offset)
//    {
//        // HACK: Extend the BerCodec and EmvCodecs to accept a Span<byte> buffer. This will allow us to populate the encoded value without creating a new byte array instance on the heap
//        Span<byte> encodedValue = _Codec.EncodeValue(GetEncodingId(), _Value!).AsSpan();

//        encodedValue.CopyTo(buffer[(offset + 1)..]);
//        offset += encodedValue.Length + 2;
//    }

//    public new byte[] EncodeTagLengthValue(BerCodec berCodec) => EncodeTagLengthValue();

//    /// <summary>
//    ///     Encodes this objects content as the Value field of a Tag-Length-Value encoding
//    /// </summary>
//    /// <param name="length">This parameter determines the length of the TLV Value field</param>
//    /// <returns></returns>
//    public new byte[] EncodeValue(int length)
//    {
//        Span<byte> berResult = _Codec.EncodeValue(GetEncodingId(), _Value!, length).AsSpan();

//        // HACK: Extend the BerCodec and EmvCodecs to accept a Span<byte> buffer. This will allow us to populate the encoded value without creating a new byte array instance on the heap
//        byte[] result = new byte[berResult.Length + 1];
//        berResult.CopyTo(result[1..]);

//        return result;
//    }

//    #endregion
//}

