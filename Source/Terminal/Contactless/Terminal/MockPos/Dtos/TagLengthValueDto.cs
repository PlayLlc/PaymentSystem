using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;

namespace MockPos.Dtos;

public class TagLengthValueDto
{
    #region Instance Values

    public string? Name { get; set; }
    public string? Tag { get; set; }
    public string? Value { get; set; }

    #endregion

    #region Serialization

    public TagLengthValue Decode()
    {
        byte[] value = PlayCodec.HexadecimalCodec.Encode(Value);
        ;
        Tag tag = new(PlayCodec.HexadecimalCodec.Encode(Tag));

        return new TagLengthValue(tag, value);
    }

    #endregion
}