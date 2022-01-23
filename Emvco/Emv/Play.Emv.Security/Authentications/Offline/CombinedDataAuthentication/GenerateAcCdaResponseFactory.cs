using System;

using ___TEMP.Play.Emv.Security.Checksum;
using ___TEMP.Play.Emv.Security.Cryptograms;

using Play.Ber.Emv;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements;
using Play.Icc.Emv;

namespace ___TEMP.Play.Emv.Security.Authentications.Dynamic.CombinedDataAuthentication;

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
    public override GenerateAcCdaResponseMessage Create(RApduSignal value)
    {
        EncodedTlvSiblings encodedTlvSiblings = _Codec.DecodeSiblings(value.GetData());

        IssuerApplicationData? issuerApplicationData = null;
        PosCardholderInteractionInformation? posCardholderInteractionInformation = null;

        CryptogramInformationData cryptogramInformationData =
            _Codec.AsPrimitive(CryptogramInformationData.Decode, CryptogramInformationData.Tag, encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(GenerateAcCdaResponseMessage)}. A {nameof(CryptogramInformationData)} was expected but could not be found");

        ApplicationTransactionCounter applicationTransactionCounter =
            _Codec.AsPrimitive(ApplicationTransactionCounter.Decode, ApplicationTransactionCounter.Tag, encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(GenerateAcCdaResponseMessage)}. A {nameof(ApplicationTransactionCounter)} was expected but could not be found");

        SignedDynamicApplicationData signedDynamicApplicationData =
            _Codec.AsPrimitive(SignedDynamicApplicationData.Decode, SignedDynamicApplicationData.Tag, encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(GenerateAcCdaResponseMessage)}. A {nameof(SignedDynamicApplicationData)} was expected but could not be found");

        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(IssuerApplicationData.Tag,
                                                         out ReadOnlyMemory<byte> rawCardholderVerificationCode3Track1))
        {
            issuerApplicationData =
                (_Codec.Decode(IssuerApplicationData.BerEncodingId, rawCardholderVerificationCode3Track1.Span) as
                    DecodedResult<IssuerApplicationData>)!.Value;
        }

        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(IssuerApplicationData.Tag,
                                                         out ReadOnlyMemory<byte> rawPosCardholderInteractionInformation))
        {
            posCardholderInteractionInformation =
                (_Codec.Decode(PosCardholderInteractionInformation.BerEncodingId, rawPosCardholderInteractionInformation.Span) as
                    DecodedResult<PosCardholderInteractionInformation>)!.Value;
        }

        return new GenerateAcCdaResponseMessage(cryptogramInformationData, applicationTransactionCounter, signedDynamicApplicationData,
                                                issuerApplicationData, posCardholderInteractionInformation);
    }

    #endregion
}