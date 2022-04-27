using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;

namespace Play.Emv.Ber.DataElements;

public abstract record DataExchangeResponse : DataExchangeList<PrimitiveValue>
{
    #region Static Metadata

    protected static readonly Tag[] _KnownPutDataTags =
    {
        UnprotectedDataEnvelope1.Tag, UnprotectedDataEnvelope2.Tag, UnprotectedDataEnvelope3.Tag, UnprotectedDataEnvelope4.Tag, UnprotectedDataEnvelope5.Tag
    };

    #endregion

    #region Constructor

    protected DataExchangeResponse(PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public void Enqueue(DataExchangeResponse item)
    {
        Enqueue(item._Value.ToArray());
    }

    public PrimitiveValue[] AsPrimitiveValues() => _Value.ToArray();
    public bool TryPeek(out PrimitiveValue? result) => _Value.TryPeek(out result);

    public bool TryGet(Tag tag, out PrimitiveValue? result)
    {
        result = _Value.FirstOrDefault(a => a.GetTag() == tag);

        return result is null;
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     EncodeValue
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public override byte[] EncodeValue()
    {
        return _Value.ToArray().SelectMany(a => a.EncodeTagLengthValue(_Codec)).ToArray();
    }

    /// <exception cref="OverflowException"></exception>
    public override byte[] EncodeTagLengthValue()
    {
        int byteCount = (int) _Value.Sum(a => a.GetTagLengthValueByteCount(_Codec));

        Span<byte> result = stackalloc byte[byteCount];

        for (int i = 0, j = 0; i < _Value.Count; i++)
        {
            _Value.ElementAt(i).EncodeTagLengthValue(_Codec).AsSpan().CopyTo(result[j..]);
            j += (int) _Value.ElementAt(i).GetTagLengthValueByteCount(_Codec);
        }

        return result.ToArray();
    }

    #endregion
}