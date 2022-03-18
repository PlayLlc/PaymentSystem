﻿using System;

using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Ber;
using Play.Emv.DataElements;
using Play.Emv.Icc;

namespace Play.Emv.Security.Checksum;

public class GetChallengeResponseFactory : TemplateFactory<GetChallengeResponseMessage>
{
    #region Instance Values

    private readonly EmvCodec _Codec;

    #endregion

    #region Constructor

    public GetChallengeResponseFactory(EmvCodec codec)
    {
        _Codec = codec;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Create
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public override GetChallengeResponseMessage Create(RApduSignal value)
    {
        EncodedTlvSiblings encodedTlvSiblings = _Codec.DecodeSiblings(value.GetData());

        CardholderVerificationCode3Track1? cardholderVerificationCode3Track1 = null;
        CardholderVerificationCode3Track2? cardholderVerificationCode3Track2 = null;
        PosCardholderInteractionInformation? posCardholderInteractionInformation = null;

        ApplicationTransactionCounter applicationTransactionCounter =
            _Codec.AsPrimitive(ApplicationTransactionCounter.Decode, ApplicationTransactionCounter.Tag, encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(GetChallengeResponseMessage)}. A {nameof(ApplicationTransactionCounter)} was expected but could not be found");

        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(CardholderVerificationCode3Track1.Tag,
                                                         out ReadOnlyMemory<byte> rawCardholderVerificationCode3Track1))
        {
            cardholderVerificationCode3Track1 =
                (_Codec.Decode(CardholderVerificationCode3Track1.EncodingId, rawCardholderVerificationCode3Track1.Span) as
                    DecodedResult<CardholderVerificationCode3Track1>)!.Value;
        }

        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(CardholderVerificationCode3Track2.Tag,
                                                         out ReadOnlyMemory<byte> rawCardholderVerificationCode3Track2))
        {
            cardholderVerificationCode3Track2 =
                (_Codec.Decode(CardholderVerificationCode3Track2.EncodingId, rawCardholderVerificationCode3Track2.Span) as
                    DecodedResult<CardholderVerificationCode3Track2>)!.Value;
        }

        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(PosCardholderInteractionInformation.Tag,
                                                         out ReadOnlyMemory<byte> rawPosCardholderInteractionInformation))
        {
            posCardholderInteractionInformation =
                (_Codec.Decode(PosCardholderInteractionInformation.EncodingId, rawPosCardholderInteractionInformation.Span) as
                    DecodedResult<PosCardholderInteractionInformation>)!.Value;
        }

        return new GetChallengeResponseMessage(applicationTransactionCounter, cardholderVerificationCode3Track1,
                                               cardholderVerificationCode3Track2, posCardholderInteractionInformation);
    }

    #endregion
}