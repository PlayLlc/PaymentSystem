using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: List of data objects that contains the accumulated data sent by the Kernel to the Terminal or visa
///     versa. The data  objects are a concatenation of BER TLV encoded values. These may correspond to Terminal reading
///     requests, obtained  from the Card by means of GET DATA or READ RECORD commands, or may correspond to data that the
///     Kernel posts to the Terminal as part of its own processing. The Terminal also uses this value in response to a DEK
///     Query command. The Terminal will use this object to update the Reader TLV Database with the data objects requested
///     by the Kernel
/// </summary>
public sealed record DataToSend : DataExchangeResponse, IEqualityComparer<DataToSend>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BerEncodingIdType.VariableCodec;
    public static readonly Tag Tag = 0xFF8104;

    #endregion

    #region Constructor

    public DataToSend() : base(Array.Empty<PrimitiveValue>())
    { }

    public DataToSend(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataToSend Decode(ReadOnlyMemory<byte> value) => new(_Codec.DecodePrimitiveValuesAtRuntime(value.Span).ToArray());

    /// <exception cref="BerParsingException"></exception>
    public static DataToSend Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodePrimitiveValuesAtRuntime(value.ToArray()).ToArray());

    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public override DataToSend Decode(TagLengthValue value) => Decode(value.EncodeTagLengthValue().AsSpan());

    #endregion

    #region Equality

    public bool Equals(DataToSend? x, DataToSend? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataToSend obj) => obj.GetHashCode();

    #endregion
}