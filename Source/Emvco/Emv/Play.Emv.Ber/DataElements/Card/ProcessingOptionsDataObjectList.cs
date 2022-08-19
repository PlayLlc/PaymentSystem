using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Tags;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains a list of terminal resident data objects (tags and lengths) needed by the ICC in processing the GET
///     PROCESSING OPTIONS command
/// </summary>
/// ///
/// <remarks>
///     Only data elements that have a source of Terminal are allowed to be included in this Data Object List
/// </remarks>
public record ProcessingOptionsDataObjectList : DataObjectList
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F38;

    #endregion

    #region Constructor

    public ProcessingOptionsDataObjectList(params TagLength[] value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public static ProcessingOptionsDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ProcessingOptionsDataObjectList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static ProcessingOptionsDataObjectList Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodeTagLengths(value.ToArray()));

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}