using Play.Emv.DataElements.Emv;

namespace Play.Emv.Templates.ResponseMessages;

public abstract class ResponseMessageTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0x77;

    #endregion

    #region Instance Members

    /// <summary>
    ///     DecodeData
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public static TagLengthValue[] DecodeData(ApduResponse value)
    {
        Tag tag = _Codec.GetFirstTag(value.GetData().AsSpan());

        if (tag == ResponseMessageTemplateFormat1.Tag)
            return ResponseMessageTemplateFormat1.Decode(_Codec.GetContentOctets(value.GetData().AsSpan()).AsSpan()).DecodeValue();

        if (tag == ResponseMessageTemplateFormat2.Tag)
            return ResponseMessageTemplateFormat2.DecodeValue(_Codec, value.GetData().AsMemory());

        throw new InvalidOperationException(
            $"The {nameof(ResponseMessageTemplate)} could not parse the argument because the tag with value {tag} was not valid");
    }

    #endregion
}