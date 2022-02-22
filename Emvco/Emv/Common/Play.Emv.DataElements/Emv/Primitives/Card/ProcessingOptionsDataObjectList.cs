using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

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

    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F38;

    #endregion

    #region Constructor

    public ProcessingOptionsDataObjectList(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    public static bool StaticEquals(ProcessingOptionsDataObjectList? x, ProcessingOptionsDataObjectList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    public static ProcessingOptionsDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ProcessingOptionsDataObjectList Decode(ReadOnlySpan<byte> value) => new(value.ToArray());

    #endregion

    #region Equality

    public bool Equals(ProcessingOptionsDataObjectList? x, ProcessingOptionsDataObjectList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ProcessingOptionsDataObjectList obj) => obj.GetHashCode();

    #endregion
}