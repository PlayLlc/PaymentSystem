using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains a list of terminal resident data objects (tags and lengths) needed by the card in processing the SEND POI
///     INFORMATION ((Send Point of Interaction) command. The SDOL can be used to request the following terminal data
///     objects:
///     • Amount, Authorized NumericCodec (tag'9F02')
///     • POI Information (tag '8B')
///     • Terminal Country Code (tag '9F1A')
///     • Transaction Currency Code (tag '5F2A')
/// </summary>
public record SelectionDataObjectList : DataObjectList, IEqualityComparer<SelectionDataObjectList>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F3F;

    #endregion

    #region Constructor

    public SelectionDataObjectList(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => checked((ushort) GetValueByteCount());

    // TODO: AmountAuthorizedNumeric could also be requested by the PICC. Need to account for that if the current transaction
    // TODO: already went online or some shit

    private TagLengthValue GetRequestedDataObject(in TagLength requestedDataObject, TagLengthValue[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].GetTag() == requestedDataObject.GetTag())
                return values[i];
        }

        return new UnknownPrimitiveValue(requestedDataObject.GetTag(), requestedDataObject.GetLength()).AsTagLengthValue();
    }

    #endregion

    #region Serialization

    public override SelectionDataObjectList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    public static SelectionDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static SelectionDataObjectList Decode(ReadOnlySpan<byte> value) => new(new BigInteger(value.ToArray()));

    #endregion

    #region Equality

    public bool Equals(SelectionDataObjectList? x, SelectionDataObjectList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(SelectionDataObjectList obj) => obj.GetHashCode();

    #endregion
}