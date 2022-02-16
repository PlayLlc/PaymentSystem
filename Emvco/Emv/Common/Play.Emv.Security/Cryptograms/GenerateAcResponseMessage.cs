using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.Security.Checksum;
using Play.Emv.Templates.ResponseMessages;

namespace Play.Emv.Security.Cryptograms;

public class GenerateAcResponseMessage : ResponseMessageTemplate
{
    #region Static Metadata

    private static readonly Tag[] _ChildTags =
    {
        ApplicationCryptogram.Tag, ApplicationTransactionCounter.Tag, CryptogramInformationData.Tag, IssuerApplicationData.Tag,
        PosCardholderInteractionInformation.Tag
    };

    #endregion

    #region Instance Values

    private readonly ApplicationCryptogram _ApplicationCryptogram;
    private readonly ApplicationTransactionCounter _ApplicationTransactionCounter;
    private readonly CryptogramInformationData _CryptogramInformationData;
    private readonly IssuerApplicationData? _IssuerApplicationData;
    private readonly PosCardholderInteractionInformation? _PosCardholderInteractionInformation;

    #endregion

    #region Constructor

    public GenerateAcResponseMessage(
        CryptogramInformationData cryptogramInformationData,
        ApplicationTransactionCounter applicationTransactionCounter,
        ApplicationCryptogram applicationCryptogram,
        IssuerApplicationData? issuerApplicationData,
        PosCardholderInteractionInformation? posCardholderInteractionInformation)
    {
        _CryptogramInformationData = cryptogramInformationData;
        _ApplicationTransactionCounter = applicationTransactionCounter;
        _ApplicationCryptogram = applicationCryptogram;
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
            _ApplicationCryptogram, _ApplicationTransactionCounter, _CryptogramInformationData, _IssuerApplicationData,
            _PosCardholderInteractionInformation
        };
    }

    public override Tag GetTag() => Tag;

    public override ushort GetValueByteCount(BerCodec codec)
    {
        checked
        {
            return (ushort) ((_CryptogramInformationData.GetValueByteCount(codec)
                    + _ApplicationTransactionCounter.GetValueByteCount(codec)
                    + _ApplicationCryptogram.GetValueByteCount(codec)
                    + _IssuerApplicationData?.GetValueByteCount(codec))
                ?? (0 + _PosCardholderInteractionInformation?.GetValueByteCount(codec)) ?? 0);
        }
    }

    #endregion

    #region Serialization

    public override byte[] EncodeTagLengthValue(BerCodec codec) => throw new NotImplementedException();

    public override byte[] EncodeValue(BerCodec codec) =>
        _IssuerApplicationData == null
            ? codec.EncodeTagLengthValue(this, _CryptogramInformationData, _ApplicationTransactionCounter, _ApplicationCryptogram)
            : codec.EncodeTagLengthValue(this, _CryptogramInformationData, _ApplicationTransactionCounter, _ApplicationCryptogram,
                _IssuerApplicationData);

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is GenerateAcResponseMessage generateAcResponse && Equals(generateAcResponse);

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(GenerateAcResponseMessage x, GenerateAcResponseMessage y) => x.Equals(y);

    public bool Equals(GenerateAcResponseMessage other) =>
        (GetTag() == other.GetTag())
        && _CryptogramInformationData.Equals(other._CryptogramInformationData)
        && _ApplicationTransactionCounter.Equals(other._ApplicationTransactionCounter)
        && _ApplicationCryptogram.Equals(other._ApplicationCryptogram)
        && _IssuerApplicationData!.Equals(other._IssuerApplicationData)
        && PosCardholderInteractionInformation.EqualsStatic(_PosCardholderInteractionInformation,
            other._PosCardholderInteractionInformation)
        && IssuerApplicationData.EqualsStatic(_IssuerApplicationData, other._IssuerApplicationData);

    public override bool Equals(ConstructedValue? other) => other is GenerateAcResponseMessage m && Equals(m);

    public override int GetHashCode()
    {
        unchecked
        {
            return (GetTag().GetHashCode()
                    + _CryptogramInformationData.GetHashCode()
                    + _ApplicationTransactionCounter.GetHashCode()
                    + _ApplicationCryptogram.GetHashCode()
                    + _IssuerApplicationData?.GetHashCode())
                ?? (0 + _PosCardholderInteractionInformation?.GetHashCode()) ?? 0;
        }
    }

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}