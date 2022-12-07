using System.Runtime.Serialization;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Tags;

namespace Play.Testing.Emv.Ber.Primitive;

public abstract class TestTlv : IDecodeDataElement
{
    #region Instance Values

    protected byte[] _ContentOctets;

    #endregion

    #region Constructor

    protected TestTlv(byte[] contentOctets)
    {
        _ContentOctets = contentOctets;
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="childRank"></param>
    /// <param name="children"></param>
    /// <exception cref="BerParsingException"></exception>
    protected TestTlv(Tag[] childRank, params TestTlv[] children)
    {
        _ContentOctets = ParseChildren(childRank, children);
    }

    #endregion

    #region Serialization

    public PrimitiveValue Decode(TagLengthValue value) => GetDefaultPrimitiveValue(GetType()).Decode(value);

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public virtual byte[] EncodeTagLengthValue()
    {
        TagLength tagLength = GetTagLength();
        Span<byte> result = stackalloc byte[tagLength.GetTagLengthValueByteCount()];
        tagLength.Encode().AsSpan().CopyTo(result);
        byte[]? a = EncodeValue().AsSpan().ToArray();
        a.CopyTo(result[tagLength.GetValueOffset()..]);

        return result.ToArray();
    }

    public byte[] EncodeValue() => _ContentOctets;

    #endregion

    #region Instance Members

    protected static PrimitiveValue GetDefaultPrimitiveValue(Type primitiveValue) => (PrimitiveValue) FormatterServices.GetUninitializedObject(primitiveValue);
    public abstract Tag GetTag();
    protected TagLength GetTagLength() => new(GetTag(), EncodeValue());
    public int GetTagLengthValueByteCount() => new TagLength(GetTag(), EncodeValue()).GetTagLengthValueByteCount();
    public virtual int GetValueByteCount() => _ContentOctets.Length;

    public TagLengthValue AsTagLengthValue() => new(GetTag(), _ContentOctets);

    /// <summary>
    ///     ParseChildren
    /// </summary>
    /// <param name="childIndex"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    private static byte[] ParseChildren(Tag[] childIndex, TestTlv[] children)
    {
        if (children.Length > childIndex.Length)
            throw new ArgumentOutOfRangeException(nameof(childIndex), $"The argument {nameof(childIndex)} has fewer items than argument {nameof(children)}");

        Span<byte> buffer = stackalloc byte[children.Sum(a => a.GetTagLengthValueByteCount())];

        for (int i = 0, j = 0; i < childIndex.Length; i++)
        {
            if (children.All(x => x.GetTag() != childIndex[i]))
                continue;

            foreach (TestTlv child in children.Where(a => a.GetTag() == childIndex[i]))
            {
                child.EncodeTagLengthValue().CopyTo(buffer[j..]);
                j += child.GetTagLengthValueByteCount();
            }
        }

        return buffer.ToArray();
    }

    #endregion
}