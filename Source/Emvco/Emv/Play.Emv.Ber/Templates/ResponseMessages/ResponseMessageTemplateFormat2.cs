using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;

namespace Play.Emv.Ber.Templates;

public record ResponseMessageTemplateFormat2 : ConstructedValue
{
    #region Static Metadata

    public static readonly Tag Tag = 0x77;

    #endregion

    #region Instance Values

    private readonly TagLengthValue[] _Value;

    #endregion

    #region Constructor

    public ResponseMessageTemplateFormat2(TagLengthValue[] value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;
    public TagLengthValue[] GetTlvChildren() => _Value;
    public int GetTlvChildrenCount() => _Value.Length;

    public override ushort GetValueByteCount(BerCodec codec)
    {
        checked
        {
            return (ushort) _Value.Sum(a => a.GetTagLengthValueByteCount());
        }
    }

    public static TagLengthValue[] DecodeValue(BerCodec codec, ReadOnlyMemory<byte> rawBer) => codec.DecodeSiblings(rawBer).AsTagLengthValues();

    #endregion

    #region Serialization

    public static ResponseMessageTemplateFormat2 Decode(BerCodec codec, ReadOnlyMemory<byte> rawBer) =>
        new(codec.DecodeTagLengthValues(rawBer));

    public override byte[] EncodeTagLengthValue(BerCodec codec) => throw new NotImplementedException();

    /// <summary>
    ///     EncodeValue
    /// </summary>
    /// <param name="codec"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public override byte[] EncodeValue(BerCodec codec)
    {
        return _Value.SelectMany(a => a.EncodeTagLengthValue()).ToArray();
    }

    #endregion

    #region Equality

    public bool Equals(ResponseMessageTemplateFormat2 x, ResponseMessageTemplateFormat2 y) => x.Equals(y);

    public override int GetHashCode()
    {
        unchecked
        {
            const int hash = 5059;
            int result = hash * Tag.GetHashCode();
            result += checked(_Value.Sum(a => a.GetHashCode() * hash));

            return result;
        }
    }

    #endregion
}