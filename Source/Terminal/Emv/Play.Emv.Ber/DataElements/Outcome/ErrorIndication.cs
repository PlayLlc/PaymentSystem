using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: Contains information regarding the nature of the error that has been encountered during the
///     transaction processing. This data object is part of the Discretionary Data.
/// </summary>
public record ErrorIndication : DataElement<ulong>, IEqualityComparer<ErrorIndication>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly ErrorIndication Default = new();
    public static readonly Tag Tag = 0xDF8115;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public ErrorIndication() : base(0)
    { }

    //public ErrorIndication(Level1Error[] level1Errors, Level2Error[] level2Errors, Level3Error[] level3Errors) : base(
    //    BuildErrorIndication(level1Errors, level2Errors, level3Errors))
    //{ }

    //public ErrorIndication(ErrorIndication errorIndication, Level1Error level1Error) : base(errorIndication | SetLevel1Error(level1Error))
    //{ }

    //public ErrorIndication(ErrorIndication errorIndication, Level2Error level2Error) : base(errorIndication | SetLevel2Error(level2Error))
    //{ }

    //public ErrorIndication(ErrorIndication errorIndication, Level3Error level3Error) : base(errorIndication | SetLevel3Error(level3Error))
    //{ }

    private ErrorIndication(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    //private static ulong BuildErrorIndication(Level1Error[] level1Errors, Level2Error[] level2Errors, Level3Error[] level3Errors)
    //{
    //    ulong result = 0;

    //    for (nint i = 0; i < level1Errors?.Length; i++)
    //        result |= SetLevel1Error(level1Errors![i]);

    //    for (nint i = 0; i < level2Errors?.Length; i++)
    //        result |= SetLevel2Error(level2Errors![i]);

    //    for (nint i = 0; i < level3Errors?.Length; i++)
    //        result |= SetLevel3Error(level3Errors![i]);

    //    return result;
    //}
    public static Builder GetBuilder() => new();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public Level1Error GetL1() => !Level1Error.Empty.TryGet((byte) (_Value >> 40), out EnumObject<byte>? result) ? Level1Error.Ok : (Level1Error) result!;
    public Level2Error GetL2() => !Level2Error.Empty.TryGet((byte) (_Value >> 32), out EnumObject<byte>? result) ? Level2Error.Ok : (Level2Error) result!;
    public Level3Error GetL3() => !Level3Error.Empty.TryGet((byte) (_Value >> 24), out EnumObject<byte>? result) ? Level3Error.Ok : (Level3Error) result!;
    public DisplayMessageOnErrorIdentifiers GetMessageIdentifier() => (DisplayMessageOnErrorIdentifiers) DisplayMessageOnErrorIdentifiers.Get((byte) _Value);
    public StatusWords GetStatusWords() => new(new StatusWord((byte) (_Value >> 8)), new StatusWord((byte) (_Value >> 8)));
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsErrorPresent() => (GetL1() != Level1Error.Ok) || (GetL2() != Level2Error.Ok) || (GetL3() != Level3Error.Ok);
    public bool IsErrorPresent(Level1Error value) => ((byte) (_Value >> 40)).AreBitsSet(value);
    public bool IsErrorPresent(Level2Error value) => ((byte) (_Value >> 40)).AreBitsSet(value);
    public bool IsErrorPresent(Level3Error value) => ((byte) (_Value >> 40)).AreBitsSet(value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ErrorIndication Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ErrorIndication Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ErrorIndication Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

        return new ErrorIndication(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

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

    //public ErrorIndication(Level1Error level1Error, StatusWords statusWords) : base(SetLevel1Error(level1Error)
    //    | SetStatusWords(statusWords))
    //{ }

    //public ErrorIndication(Level2Error level2Error, StatusWords statusWords) : base(SetLevel2Error(level2Error)
    //    | SetStatusWords(statusWords))
    //{ }

    public class Builder : PrimitiveValueBuilder<ulong>
    {
        #region Constructor

        internal Builder(ErrorIndication value)
        {
            _Value = value._Value;
        }

        internal Builder()
        {
            _Value = 0;
        }

        #endregion

        #region Instance Members

        public void Reset(ErrorIndication value)
        {
            _Value = value._Value;
        }

        public void Set(Level1Error error)
        {
            const byte offset = 40;
            _Value = _Value.ClearBits(byte.MaxValue << offset);
            _Value |= (ulong) error << offset;
        }

        public void Set(Level2Error error)
        {
            const byte offset = 32;
            _Value = _Value.ClearBits(byte.MaxValue << offset);
            _Value |= (ulong) error << offset;
        }

        public void Set(Level3Error error)
        {
            const byte offset =  4;
            _Value = _Value.ClearBits(byte.MaxValue << offset);
            _Value |= (ulong) error << offset;
        }

        public void Set(StatusWords statusWords)
        {
            const byte offset = 8;
            _Value = _Value.ClearBits(ushort.MaxValue << offset);
            _Value |= (ulong) statusWords << offset;
        }

        public void Set(DisplayMessageOnErrorIdentifiers value)
        {
            _Value.ClearBits(byte.MaxValue);
            _Value |= (ulong) value;
        }

        public override ErrorIndication Complete() => new(_Value);

        protected override void Set(ulong bitsToSet)
        {
            _Value |= bitsToSet;
        }

        #endregion
    }
}