using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     A data object in the Card that provides the Kernel with a list of data objects that must be passed to the Card in
///     the data field of the GENERATE AC command after the CDOL1 Related Data. An example of value for DSDOL is
///     'DF6008DF6108DF6201DF63A0', representing TLDS Input (Card) || TLDS Digest H || TLDS ODS Info || TLDS ODS Term. The
///     Kernel must not presume that this is a given though, as the sequence and presence of data objects can vary. The
///     presence of TL DS ODS Info is mandated and the processing of the last TL entry in DSDOL is different from normal TL
///     processing as described in section 4.1.4.
/// </summary>
public record DataStorageDataObjectList : DataObjectList
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F5B;
    private static readonly byte _MaxByteLength = 250;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public DataStorageDataObjectList(params TagLength[] value) : base(value)
    {
        Check.Primitive.ForMaximumLength(value.Sum(a => a.GetTagLengthByteCount()), _MaxByteLength, Tag);
    }

    #endregion

    #region Serialization

    public static DataStorageDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override DataStorageDataObjectList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataStorageDataObjectList Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodeTagLengths(value.ToArray()));

    #endregion

    #region Equality

    public bool Equals(DataStorageDataObjectList? x, DataStorageDataObjectList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataStorageDataObjectList obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => base.GetValueByteCount();

    #endregion
}