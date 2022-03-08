using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.Templates.ResponseMessages;

public class ProcessingOptions : ResponseMessageTemplate
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

    public ProcessingOptions(TagLengthValue[] values)
    {
        _ApplicationFileLocator =
            ApplicationFileLocator.Decode(values.First(a => a.GetTag() == ApplicationFileLocator.Tag).EncodeValue().AsMemory());
        _ApplicationInterchangeProfile =
            ApplicationInterchangeProfile.Decode(
                values.First(a => a.GetTag() == ApplicationInterchangeProfile.Tag).EncodeValue().AsMemory());
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

    public static ProcessingOptions Decode(ReadOnlyMemory<byte> rawBer) => Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    private static ProcessingOptions Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        ApplicationFileLocator applicationFileLocator =
            _Codec.AsPrimitive(ApplicationFileLocator.Decode, ApplicationFileLocator.Tag, encodedTlvSiblings)
            ?? throw new InvalidOperationException(
                $"A problem occurred while decoding {nameof(ProcessingOptions)}. A {nameof(ApplicationFileLocator)} was expected but could not be found");

        ApplicationInterchangeProfile applicationInterchangeProfile =
            _Codec.AsPrimitive(ApplicationInterchangeProfile.Decode, ApplicationInterchangeProfile.Tag, encodedTlvSiblings)
            ?? throw new InvalidOperationException(
                $"A problem occurred while decoding {nameof(ProcessingOptions)}. A {nameof(ApplicationInterchangeProfile)} was expected but could not be found");

        return new ProcessingOptions(applicationFileLocator, applicationInterchangeProfile);
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is ProcessingOptions processingOptions && Equals(processingOptions);

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(ProcessingOptions x, ProcessingOptions y) => x.Equals(y);

    public bool Equals(ProcessingOptions other) =>
        _ApplicationFileLocator.Equals(other._ApplicationFileLocator)
        && _ApplicationInterchangeProfile.Equals(other._ApplicationInterchangeProfile);

    public override bool Equals(ConstructedValue? other) => other is ProcessingOptions processingOptions && Equals(processingOptions);

    public override int GetHashCode()
    {
        unchecked
        {
            return _ApplicationFileLocator.GetHashCode() + _ApplicationInterchangeProfile.GetHashCode();
        }
    }

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}