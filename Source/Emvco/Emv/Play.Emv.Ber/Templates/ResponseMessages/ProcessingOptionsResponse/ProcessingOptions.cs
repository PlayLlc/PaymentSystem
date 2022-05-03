using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Templates;

public record ProcessingOptions : ResponseMessageTemplate
{
    #region Static Metadata

    public static Tag[] ChildTags = {ApplicationFileLocator.Tag, ApplicationInterchangeProfile.Tag};

    #endregion

    #region Instance Values

    private readonly ApplicationFileLocator _ApplicationFileLocator;
    private readonly ApplicationInterchangeProfile _ApplicationInterchangeProfile;

    #endregion

    #region Constructor

    public ProcessingOptions(ApplicationFileLocator applicationFileLocator, ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        _ApplicationFileLocator = applicationFileLocator;
        _ApplicationInterchangeProfile = applicationInterchangeProfile;
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="values"></param>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ProcessingOptions(TagLengthValue[] values)
    {
        _ApplicationFileLocator = ApplicationFileLocator.Decode(values.First(a => a.GetTag() == ApplicationFileLocator.Tag).EncodeValue().AsMemory());
        _ApplicationInterchangeProfile =
            ApplicationInterchangeProfile.Decode(values.First(a => a.GetTag() == ApplicationInterchangeProfile.Tag).EncodeValue().AsMemory());
    }

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;
    public override Tag[] GetChildTags() => ChildTags;

    protected override IEncodeBerDataObjects[] GetChildren()
    {
        return new IEncodeBerDataObjects[] {_ApplicationFileLocator, _ApplicationInterchangeProfile};
    }

    #endregion

    #region Serialization

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static ProcessingOptions Decode(ReadOnlyMemory<byte> value)
    {
        if (_Codec.DecodeFirstTag(value.Span) == Tag)
            return Decode(_Codec.DecodeChildren(value));

        return Decode(_Codec.DecodeSiblings(value));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private static ProcessingOptions Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        ApplicationFileLocator applicationFileLocator = _Codec.AsPrimitive(ApplicationFileLocator.Decode, ApplicationFileLocator.Tag, encodedTlvSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(ProcessingOptions)}. A {nameof(ApplicationFileLocator)} was expected but could not be found");

        ApplicationInterchangeProfile applicationInterchangeProfile =
            _Codec.AsPrimitive(ApplicationInterchangeProfile.Decode, ApplicationInterchangeProfile.Tag, encodedTlvSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(ProcessingOptions)}. A {nameof(ApplicationInterchangeProfile)} was expected but could not be found");

        return new ProcessingOptions(applicationFileLocator, applicationInterchangeProfile);
    }

    #endregion

    #region Equality

    public bool Equals(ProcessingOptions x, ProcessingOptions y) => x.Equals(y);

    public override int GetHashCode()
    {
        unchecked
        {
            return _ApplicationFileLocator.GetHashCode() + _ApplicationInterchangeProfile.GetHashCode();
        }
    }

    #endregion
}