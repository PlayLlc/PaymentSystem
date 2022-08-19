using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs.Exceptions;

namespace Play.Emv.Ber.Templates;

public abstract record Template : ConstructedValue
{
    #region Static Metadata

    protected static readonly EmvCodec _Codec = new();

    #endregion

    #region Instance Members

    // HACK: We should try to avoid using LINQ and extra collection allocations when possible. Let's only allocate the necessary amount of memory from the heap and use an array instead of a list here
    public PrimitiveValue[] GetPrimitiveDescendants()
    {
        IEncodeBerDataObjects?[] children = GetChildren();

        List<PrimitiveValue> primitiveDescendents = GetChildren().OfType<PrimitiveValue>().ToList();

        primitiveDescendents.AddRange(children.OfType<Template>().SelectMany(b => b.GetPrimitiveDescendants()));

        return primitiveDescendents.ToArray();
    }

    /// <exception cref="BerParsingException"></exception>
    public virtual uint GetTagLengthValueByteCount() => GetTagLengthValueByteCount(_Codec);

    //public TagLengthValue AsTagLengthValue() => new(GetTag(), GetChildren().SelectMany(a => a.EncodeValue(_Codec)).ToArray());
    public TagLengthValue AsTagLengthValue() => AsTagLengthValue(_Codec);

    /// <exception cref="OverflowException"></exception>
    public virtual ushort GetValueByteCount() => _Codec.GetValueByteCount(GetChildren());

    /// <exception cref="OverflowException"></exception>
    public override ushort GetValueByteCount(BerCodec codec) => GetValueByteCount();

    public abstract Tag[] GetChildTags();
    protected abstract IEncodeBerDataObjects?[] GetChildren();

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public virtual byte[] EncodeValue() => _Codec.EncodeValue(this, GetChildTags(), GetChildren());

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public virtual byte[] EncodeTagLengthValue() => _Codec.EncodeTagLengthValue(this, GetChildTags(), GetChildren());

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public override byte[] EncodeTagLengthValue(BerCodec codec) => EncodeTagLengthValue();

    #endregion
}