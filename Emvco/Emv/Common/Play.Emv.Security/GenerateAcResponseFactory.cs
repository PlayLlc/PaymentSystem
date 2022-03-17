using System;

using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Security.Cryptograms;

namespace Play.Emv.Security;

internal class GenerateAcResponseFactory : TemplateFactory<GenerateAcResponseMessage>
{
    #region Instance Values

    protected readonly EmvCodec _Codec;

    #endregion

    #region Constructor

    public GenerateAcResponseFactory(EmvCodec codec)
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
    public override GenerateAcResponseMessage Create(RApduSignal value)
    {
        EncodedTlvSiblings encodedTlvSiblings = _Codec.DecodeSiblings(value.GetData());

        IssuerApplicationData? issuerApplicationData = null;

        CryptogramInformationData cryptogramInformationData =
            _Codec.AsPrimitive(CryptogramInformationData.Decode, CryptogramInformationData.Tag, encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(GenerateAcResponseMessage)}. A {nameof(CryptogramInformationData)} was expected but could not be found");

        ApplicationTransactionCounter applicationTransactionCounter =
            _Codec.AsPrimitive(ApplicationTransactionCounter.Decode, ApplicationTransactionCounter.Tag, encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(GenerateAcResponseMessage)}. A {nameof(ApplicationTransactionCounter)} was expected but could not be found");

        ApplicationCryptogram applicationCryptogram =
            _Codec.AsPrimitive(ApplicationCryptogram.Decode, ApplicationCryptogram.Tag, encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(GenerateAcResponseMessage)}. A {nameof(ApplicationCryptogram)} was expected but could not be found");

        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(IssuerApplicationData.Tag,
                                                         out ReadOnlyMemory<byte> rawCardholderVerificationCode3Track1))
        {
            issuerApplicationData =
                (_Codec.Decode(IssuerApplicationData.EncodingId, rawCardholderVerificationCode3Track1.Span) as
                    DecodedResult<IssuerApplicationData>)!.Value;
        }

        // TODO: I'm not sure why this last argument is null - looks like this didn't get completed
        return new GenerateAcResponseMessage(cryptogramInformationData, applicationTransactionCounter, applicationCryptogram,
                                             issuerApplicationData, null);
    }

    #endregion
}