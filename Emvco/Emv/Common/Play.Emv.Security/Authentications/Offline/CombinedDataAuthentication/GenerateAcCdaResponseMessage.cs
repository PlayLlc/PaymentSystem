using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.Security.Checksum;
using Play.Emv.Security.Cryptograms;
using Play.Emv.Templates.ResponseMessages;

namespace Play.Emv.Security.Authentications.Offline.CombinedDataAuthentication;

public class GenerateAcCdaResponseMessage : ResponseMessageTemplate
{
    #region Static Metadata

    private static readonly Tag[] _ChildTags =
    {
        ApplicationTransactionCounter.Tag, CryptogramInformationData.Tag, IssuerApplicationData.Tag,
        PosCardholderInteractionInformation.Tag, SignedDynamicApplicationData.Tag
    };

    #endregion

    #region Instance Values

    private readonly ApplicationTransactionCounter _ApplicationTransactionCounter;
    private readonly CryptogramInformationData _CryptogramInformationData;
    private readonly IssuerApplicationData? _IssuerApplicationData;
    private readonly PosCardholderInteractionInformation? _PosCardholderInteractionInformation;
    private readonly SignedDynamicApplicationData _SignedDynamicApplicationData;

    #endregion

    #region Constructor

    public GenerateAcCdaResponseMessage(
        CryptogramInformationData cryptogramInformationData,
        ApplicationTransactionCounter applicationTransactionCounter,
        SignedDynamicApplicationData signedDynamicApplicationData,
        IssuerApplicationData? issuerApplicationData,
        PosCardholderInteractionInformation? posCardholderInteractionInformation)
    {
        _CryptogramInformationData = cryptogramInformationData;
        _ApplicationTransactionCounter = applicationTransactionCounter;
        _SignedDynamicApplicationData = signedDynamicApplicationData;
        _IssuerApplicationData = issuerApplicationData;
        _PosCardholderInteractionInformation = posCardholderInteractionInformation;
    }

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags() => _ChildTags;

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[]
        {
            _ApplicationTransactionCounter, _CryptogramInformationData, _IssuerApplicationData, _PosCardholderInteractionInformation,
            _SignedDynamicApplicationData
        };
    }

    public CryptogramInformationData GetCryptogramInformationData() => _CryptogramInformationData;
    public SignedDynamicApplicationData GetSignedDynamicApplicationData() => _SignedDynamicApplicationData;
    public override Tag GetTag() => Tag;

    public byte[] GetTransactionDataHashData(BerCodec codec)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((_CryptogramInformationData.GetValueByteCount(codec)
                + _ApplicationTransactionCounter.GetValueByteCount(codec)
                + _SignedDynamicApplicationData.GetValueByteCount(codec)
                + _IssuerApplicationData?.GetValueByteCount(codec))
            ?? 0);
        Span<byte> buffer = spanOwner.Span;

        int offset = 0;

        ReadOnlySpan<byte> encodedCryptogramInformationData = _CryptogramInformationData.EncodeValue(codec);
        ReadOnlySpan<byte> encodedApplicationTransactionCounter = _ApplicationTransactionCounter.EncodeValue(codec);

        for (int i = 0; i < encodedCryptogramInformationData.Length; i++, offset++)
            buffer[offset] = encodedCryptogramInformationData[i];

        for (int i = 0; i < encodedApplicationTransactionCounter.Length; i++, offset++)
            buffer[offset] = encodedApplicationTransactionCounter[i];

        if (_IssuerApplicationData is not null)
            _IssuerApplicationData!.EncodeValue(codec).CopyTo(buffer[^_IssuerApplicationData.GetValueByteCount(codec)..]);

        return buffer.ToArray();
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        checked
        {
            return (ushort) ((_CryptogramInformationData.GetValueByteCount(codec)
                    + _ApplicationTransactionCounter.GetValueByteCount(codec)
                    + _SignedDynamicApplicationData.GetValueByteCount(codec)
                    + _IssuerApplicationData?.GetValueByteCount(codec))
                ?? (0 + _PosCardholderInteractionInformation?.GetValueByteCount(codec)) ?? 0);
        }
    }

    #endregion

    #region Serialization

    public override byte[] EncodeTagLengthValue(BerCodec codec) => throw new NotImplementedException();

    public override byte[] EncodeValue(BerCodec codec) =>
        _IssuerApplicationData == null
            ? codec.EncodeTagLengthValue(this, _CryptogramInformationData, _ApplicationTransactionCounter, _SignedDynamicApplicationData)
            : codec.EncodeTagLengthValue(this, _CryptogramInformationData, _ApplicationTransactionCounter, _SignedDynamicApplicationData,
                _IssuerApplicationData);

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is GenerateAcCdaResponseMessage generateAcResponse && Equals(generateAcResponse);
    public override bool Equals(ConstructedValue? other) => other is GenerateAcCdaResponseMessage message && Equals(message);

    public bool Equals(GenerateAcCdaResponseMessage other) =>
        (GetTag() == other.GetTag())
        && _CryptogramInformationData.Equals(other._CryptogramInformationData)
        && _ApplicationTransactionCounter.Equals(other._ApplicationTransactionCounter)
        && _SignedDynamicApplicationData.Equals(other._SignedDynamicApplicationData)
        && _IssuerApplicationData!.Equals(other._IssuerApplicationData)
        && PosCardholderInteractionInformation.EqualsStatic(_PosCardholderInteractionInformation,
            other._PosCardholderInteractionInformation)
        && IssuerApplicationData.EqualsStatic(_IssuerApplicationData, other._IssuerApplicationData);

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(GenerateAcCdaResponseMessage x, GenerateAcCdaResponseMessage y) => x.Equals(y);

    public override int GetHashCode()
    {
        unchecked
        {
            return (GetTag().GetHashCode()
                    + _CryptogramInformationData.GetHashCode()
                    + _ApplicationTransactionCounter.GetHashCode()
                    + _SignedDynamicApplicationData.GetHashCode()
                    + _IssuerApplicationData?.GetHashCode())
                ?? (0 + _PosCardholderInteractionInformation?.GetHashCode()) ?? 0;
        }
    }

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}