using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Provides the SFI of the Transaction Log file and its number of records
/// </summary>
public record LogEntry : DataElement<ushort>, IEqualityComparer<LogEntry>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F4D;
    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;

    #endregion

    #region Constructor

    public LogEntry(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static LogEntry Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static LogEntry Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 2;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(LogEntry)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ushort> result = codec.Decode(PlayEncodingId, value) as DecodedResult<ushort>
            ?? throw new InvalidOperationException(
                $"The {nameof(LogEntry)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new LogEntry(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(LogEntry? x, LogEntry? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(LogEntry obj) => obj.GetHashCode();

    #endregion
}