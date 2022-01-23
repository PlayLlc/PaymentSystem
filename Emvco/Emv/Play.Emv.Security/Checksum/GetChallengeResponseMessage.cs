using System;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.Templates.ResponseMessages;

namespace Play.Emv.Security.Checksum;

public class GetChallengeResponseMessage : ResponseMessageTemplate
{
    #region Static Metadata

    private static readonly Tag[] _ChildTags =
    {
        ApplicationTransactionCounter.Tag, CardholderVerificationCode3Track1.Tag, CardholderVerificationCode3Track2.Tag,
        PosCardholderInteractionInformation.Tag
    };

    #endregion

    #region Instance Values

    private readonly ApplicationTransactionCounter _ApplicationTransactionCounter;
    private readonly CardholderVerificationCode3Track1? _CardholderVerificationCode3Track1;
    private readonly CardholderVerificationCode3Track2? _CardholderVerificationCode3Track2;
    private readonly PosCardholderInteractionInformation? _PosCardholderInteractionInformation;

    #endregion

    #region Constructor

    public GetChallengeResponseMessage(
        ApplicationTransactionCounter applicationTransactionCounter,
        CardholderVerificationCode3Track1? cardholderVerificationCode3Track1,
        CardholderVerificationCode3Track2? cardholderVerificationCode3Track2,
        PosCardholderInteractionInformation? posCardholderInteractionInformation)
    {
        _ApplicationTransactionCounter = applicationTransactionCounter;
        _CardholderVerificationCode3Track1 = cardholderVerificationCode3Track1;
        _CardholderVerificationCode3Track2 = cardholderVerificationCode3Track2;
        _PosCardholderInteractionInformation = posCardholderInteractionInformation;
    }

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags() => _ChildTags;

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[]
        {
            _ApplicationTransactionCounter, _CardholderVerificationCode3Track1, _CardholderVerificationCode3Track2,
            _PosCardholderInteractionInformation
        };
    }

    public override Tag GetTag() => Tag;

    public override ushort GetValueByteCount(BerCodec codec)
    {
        checked
        {
            return (ushort) ((_ApplicationTransactionCounter.GetValueByteCount(codec)
                    + _CardholderVerificationCode3Track1?.GetValueByteCount(codec))
                ?? (0 + _CardholderVerificationCode3Track2?.GetValueByteCount(codec))
                ?? (0 + _PosCardholderInteractionInformation?.GetValueByteCount(codec)) ?? 0);
        }
    }

    #endregion

    #region Serialization

    public override byte[] EncodeTagLengthValue(BerCodec codec) => throw new NotImplementedException();

    public override byte[] EncodeValue(BerCodec codec) =>
        codec.EncodeTagLengthValue(this, _ApplicationTransactionCounter, _CardholderVerificationCode3Track1,
                                   _CardholderVerificationCode3Track2, _PosCardholderInteractionInformation);

    #endregion

    #region Equality

    public override bool Equals(ConstructedValue? other) => throw new NotImplementedException();

    public override bool Equals(object? obj) =>
        obj is GetChallengeResponseMessage getChallengeResponseMessage && Equals(getChallengeResponseMessage);

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(GetChallengeResponseMessage x, GetChallengeResponseMessage y) => x.Equals(y);

    public bool Equals(GetChallengeResponseMessage other)
    {
        if (GetTag() != other.GetTag())
            return false;

        if (!_ApplicationTransactionCounter.Equals(other._ApplicationTransactionCounter))
            return false;

        if (!CardholderVerificationCode3Track1.EqualsStatic(_CardholderVerificationCode3Track1, other._CardholderVerificationCode3Track1))
            return false;

        if (!CardholderVerificationCode3Track2.EqualsStatic(_CardholderVerificationCode3Track2, other._CardholderVerificationCode3Track2))
            return false;

        if (!PosCardholderInteractionInformation.EqualsStatic(_PosCardholderInteractionInformation,
                                                              other._PosCardholderInteractionInformation))
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (GetTag().GetHashCode()
                    + _ApplicationTransactionCounter.GetHashCode()
                    + _CardholderVerificationCode3Track1?.GetHashCode())
                ?? (0 + _CardholderVerificationCode3Track2?.GetHashCode())
                ?? (0 + _PosCardholderInteractionInformation?.GetHashCode()) ?? 0;
        }
    }

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}