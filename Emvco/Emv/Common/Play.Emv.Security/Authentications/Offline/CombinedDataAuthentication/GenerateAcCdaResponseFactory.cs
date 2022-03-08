using System;

using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Ber;
using Play.Emv.DataElements.Emv;
using Play.Emv.Icc;
using Play.Emv.Security.Checksum;
using Play.Emv.Security.Cryptograms;

namespace Play.Emv.Security.Authentications.Offline.CombinedDataAuthentication;

public class GenerateAcCdaResponseFactory : TemplateFactory<GenerateAcCdaResponseMessage>
{
    #region Instance Values

    private readonly EmvCodec _Codec;

    #endregion

    #region Constructor

    public GenerateAcCdaResponseFactory(EmvCodec Codec)
    {
        _Codec = Codec;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Create
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerInternalException"></exception>
    public override GenerateAcCdaResponseMessage Create(RApduSignal value)
    {
        EncodedTlvSiblings encodedTlvSiblings = _Codec.DecodeSiblings(value.GetData());

        IssuerApplicationData? issuerApplicationData = null;
        PosCardholderInteractionInformation? posCardholderInteractionInformation = null;

        CryptogramInformationData cryptogramInformationData =
            _Codec.AsPrimitive(CryptogramInformationData.Decode, CryptogramInformationData.Tag, encodedTlvSiblings)
            ?? throw new InvalidOperationException(
                $"A problem occurred while decoding {nameof(GenerateAcCdaResponseMessage)}. A {nameof(CryptogramInformationData)} was expected but could not be found");

        ApplicationTransactionCounter applicationTransactionCounter =
            _Codec.AsPrimitive(ApplicationTransactionCounter.Decode, ApplicationTransactionCounter.Tag, encodedTlvSiblings)
            ?? throw new InvalidOperationException(
                $"A problem occurred while decoding {nameof(GenerateAcCdaResponseMessage)}. A {nameof(ApplicationTransactionCounter)} was expected but could not be found");

        SignedDynamicApplicationData signedDynamicApplicationData =
            _Codec.AsPrimitive(SignedDynamicApplicationData.Decode, SignedDynamicApplicationData.Tag, encodedTlvSiblings)
            ?? throw new InvalidOperationException(
                $"A problem occurred while decoding {nameof(GenerateAcCdaResponseMessage)}. A {nameof(SignedDynamicApplicationData)} was expected but could not be found");

        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(IssuerApplicationData.Tag,
            out ReadOnlyMemory<byte> rawCardholderVerificationCode3Track1))
        {
            issuerApplicationData =
                (_Codec.Decode(IssuerApplicationData.EncodingId, rawCardholderVerificationCode3Track1.Span) as
                    DecodedResult<IssuerApplicationData>)!.Value;
        }

        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(IssuerApplicationData.Tag,
            out ReadOnlyMemory<byte> rawPosCardholderInteractionInformation))
        {
            posCardholderInteractionInformation =
                (_Codec.Decode(PosCardholderInteractionInformation.EncodingId, rawPosCardholderInteractionInformation.Span) as
                    DecodedResult<PosCardholderInteractionInformation>)!.Value;
        }

        return new GenerateAcCdaResponseMessage(cryptogramInformationData, applicationTransactionCounter, signedDynamicApplicationData,
            issuerApplicationData, posCardholderInteractionInformation);
    }

    #endregion
}