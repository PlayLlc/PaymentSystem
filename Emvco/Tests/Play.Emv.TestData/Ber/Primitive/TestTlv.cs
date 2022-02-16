using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.TestData.Ber.Primitive;

public abstract class TestTlv
{
    #region Instance Values

    protected byte[] _ContentOctets;

    #endregion

    #region Constructor

    protected TestTlv(byte[] contentOctets)
    {
        _ContentOctets = contentOctets;
    }

    protected TestTlv(Tag[] childRank, params TestTlv[] children)
    {
        _ContentOctets = ParseChildren(childRank, children);
    }

    protected TestTlv()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Instance Members

    public abstract Tag GetTag();
    protected TagLength GetTagLength() => new(GetTag(), EncodeValue());
    public int GetTagLengthValueByteCount() => new TagLength(GetTag(), EncodeValue()).GetTagLengthValueByteCount();
    public int GetValueByteCount() => _ContentOctets.Length;
    public TagLengthValue AsTagLengthValue() => new(GetTag(), EncodeValue());

    private static byte[] ParseChildren(Tag[] childIndex, TestTlv[] children)
    {
        if (children.Count() > childIndex.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(childIndex),
                $"The argument {nameof(childIndex)} has fewer items than argument {nameof(children)}");
        }

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

    #region Serialization

    public byte[] EncodeTagLengthValue()
    {
        TagLength tagLength = GetTagLength();
        Span<byte> result = stackalloc byte[tagLength.GetTagLengthValueByteCount()];

        tagLength.Encode().AsSpan().CopyTo(result);
        EncodeValue().AsSpan().CopyTo(result[tagLength.GetValueOffset()..]);

        return result.ToArray();
    }

    public byte[] EncodeValue() => _ContentOctets;

    #endregion
}