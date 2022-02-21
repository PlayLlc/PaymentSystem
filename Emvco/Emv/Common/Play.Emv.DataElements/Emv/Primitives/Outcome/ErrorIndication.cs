using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Icc;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: Contains information regarding the nature of the error that has been encountered during the
///     transaction processing. This data object is part of the Discretionary Data.
/// </summary>
public record ErrorIndication : DataElement<ulong>, IEqualityComparer<ErrorIndication>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly ErrorIndication Default = new(0);
    public static readonly Tag Tag = 0xDF8115;

    #endregion

    #region Constructor

    public ErrorIndication() : base(0)
    { }

    public ErrorIndication(ErrorIndication errorIndication, Level1Error level1Error) : base(errorIndication | SetLevel1Error(level1Error))
    { }

    public ErrorIndication(ErrorIndication errorIndication, Level2Error level2Error) : base(errorIndication | SetLevel2Error(level2Error))
    { }

    public ErrorIndication(ErrorIndication errorIndication, Level3Error level3Error) : base(errorIndication | SetLevel3Error(level3Error))
    { }

    public ErrorIndication(ulong value) : base(value)
    { }

    public ErrorIndication(Level1Error level1Error, StatusWords statusWords) : base(SetLevel1Error(level1Error)
        | SetStatusWords(statusWords))
    { }

    public ErrorIndication(Level2Error level2Error, StatusWords statusWords) : base(SetLevel2Error(level2Error)
        | SetStatusWords(statusWords))
    { }

    public ErrorIndication(StatusWords statusWords) : base(SetStatusWords(statusWords))
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public Level1Error GetL1() => Level1Error.Get((byte) (_Value >> 40));
    public Level2Error GetL2() => Level2Error.Get((byte) (_Value >> 32));
    public Level3Error GetL3() => Level3Error.Get((byte) (_Value >> 24));
    public MessageIdentifier GetMessageIdentifier() => MessageIdentifier.Get((byte) _Value);
    public StatusWords GetStatusWords() => new(new StatusWord((byte) (_Value >> 16)), new StatusWord((byte) (_Value >> 8)));
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public bool IsErrorPresent() => (GetL1() != Level1Error.Ok) || (GetL2() != Level2Error.Ok) || (GetL3() != Level3Error.Ok);

    private static ulong SetLevel1Error(Level1Error error)
    {
        const byte offset = 40;

        return (ulong) error >> offset;
    }

    private static ulong SetLevel2Error(Level2Error error)
    {
        const byte offset = 32;

        return (ulong) error >> offset;
    }

    private static ulong SetLevel3Error(Level3Error error)
    {
        const byte offset = 24;

        return (ulong) error >> offset;
    }

    private static ulong SetStatusWords(StatusWords statusWords)
    {
        const byte offset = 8;

        return (ulong) statusWords >> offset;
    }

    #endregion

    #region Serialization

    public static ErrorIndication Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ErrorIndication Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 6;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ErrorIndication)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(ErrorIndication)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new ErrorIndication(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(ErrorIndication? x, ErrorIndication? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ErrorIndication obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator ulong(ErrorIndication value) => value._Value;

    #endregion
}