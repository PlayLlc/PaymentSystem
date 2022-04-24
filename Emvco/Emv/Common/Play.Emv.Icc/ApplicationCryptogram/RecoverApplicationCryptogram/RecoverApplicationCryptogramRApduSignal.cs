using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;

namespace Play.Emv.Icc;

public class RecoverApplicationCryptogramRApduSignal : RApduSignal
{
    #region Constructor

    public RecoverApplicationCryptogramRApduSignal(byte[] value) : base(value)
    { }

    public RecoverApplicationCryptogramRApduSignal(byte[] value, Level1Error level1Error) : base(value, level1Error)
    { }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();

    public Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    /// <exception cref="OverflowException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public PrimitiveValue[] ParseDataObjects()
    {
        EmvCodec codec = EmvCodec.GetCodec();

        TagLengthValue[] dataObjects = ResponseMessageTemplateFormat2.Decode(codec, _Data).GetTlvChildren();
        EncodedTlvSiblings siblings = codec.DecodeSiblings(GetData());
        Tag[] knownTags = GetKnownTags(dataObjects).ToArray();
        PrimitiveValue[] result = new PrimitiveValue[knownTags.Count()];

        for (int i = 0; i < knownTags.Length; i++)
            result[i] = DecodePrimitiveValue(knownTags[i], siblings.GetValueOctetsOfSibling(knownTags[i]));

        return result;
    }

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private static PrimitiveValue DecodePrimitiveValue(Tag tag, ReadOnlySpan<byte> value)
    {
        if (tag == CryptogramInformationData.Tag)
            return CryptogramInformationData.Decode(value);
        if (tag == ApplicationTransactionCounter.Tag)
            return ApplicationTransactionCounter.Decode(value);
        if (tag == ApplicationCryptogram.Tag)
            return ApplicationCryptogram.Decode(value);
        if (tag == IssuerApplicationData.Tag)
            return IssuerApplicationData.Decode(value);
        if (tag == PosCardholderInteractionInformation.Tag)
            return PosCardholderInteractionInformation.Decode(value);
        if (tag == SignedDynamicApplicationData.Tag)
            return SignedDynamicApplicationData.Decode(value);

        throw new TerminalDataException(
            $"The {nameof(RecoverApplicationCryptogramRApduSignal)} tried to decode an unknown data object with the {nameof(Tag)} value of [{tag}]");
    }

    private IEnumerable<Tag> GetKnownTags(TagLengthValue[] values)
    {
        if (values.Any(a => a.GetTag() == CryptogramInformationData.Tag))
            yield return CryptogramInformationData.Tag;
        if (values.Any(a => a.GetTag() == ApplicationTransactionCounter.Tag))
            yield return ApplicationTransactionCounter.Tag;
        if (values.Any(a => a.GetTag() == ApplicationCryptogram.Tag))
            yield return ApplicationCryptogram.Tag;
        if (values.Any(a => a.GetTag() == IssuerApplicationData.Tag))
            yield return IssuerApplicationData.Tag;
        if (values.Any(a => a.GetTag() == PosCardholderInteractionInformation.Tag))
            yield return PosCardholderInteractionInformation.Tag;

        if (values.Any(a => a.GetTag() == SignedDynamicApplicationData.Tag))
            yield return SignedDynamicApplicationData.Tag;
    }

    #endregion
}