﻿using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Ber.Templates;

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
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static TagLengthValue[] DecodeData(ApduResponse value)
    {
        Tag tag = _Codec.GetFirstTag(value.GetData().AsSpan());

        if (tag == ResponseMessageTemplateFormat1.Tag)
            return ResponseMessageTemplateFormat1.Decode(_Codec.GetContentOctets(value.GetData().AsSpan()).AsSpan()).DecodeValue();

        if (tag == ResponseMessageTemplateFormat2.Tag)
            return ResponseMessageTemplateFormat2.DecodeValue(_Codec, value.GetData().AsMemory());

        throw new
            InvalidOperationException($"The {nameof(ResponseMessageTemplate)} could not parse the argument because the tag with value {tag} was not valid");
    }

    #endregion
}