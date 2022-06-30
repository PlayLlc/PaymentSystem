using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Ber.ValueTypes.Card;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The Phone Message Table is a variable length list of entries of eight bytes each, and defines for the selected AID
///     the message and status identifiers as a function of the POS Cardholder Interaction Information. Each entry in the
///     Phone Message Table contains the fields shown in the table below.
/// </summary>
public record PhoneMessageTable : DataElement<MessageTableEntry[]>, IEqualityComparer<PhoneMessageTable>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8131;

    #endregion

    #region Constructor

    public PhoneMessageTable(MessageTableEntry[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount() => (ushort) _Value.Sum(a => a.GetByteCount());

    // HACK: This is a technology specific implementation and should move to Kernel 2
    /// <exception cref="PlayInternalException"></exception>
    public static PhoneMessageTable CreateKernel2Default()
    {
        return new PhoneMessageTable(new MessageTableEntry[]
        {
            new(new PciiMask(0x000001), new PciiValue(000001), (MessageIdentifier) MessageIdentifiers.SeePhoneForInstructions, Statuses.NotReady),
            new(new PciiMask(0x000800), new PciiValue(000800), (MessageIdentifier) MessageIdentifiers.SeePhoneForInstructions, Statuses.NotReady),
            new(new PciiMask(0x000400), new PciiValue(000400), (MessageIdentifier) MessageIdentifiers.SeePhoneForInstructions, Statuses.NotReady),
            new(new PciiMask(0x000100), new PciiValue(000100), (MessageIdentifier) MessageIdentifiers.SeePhoneForInstructions, Statuses.NotReady),
            new(new PciiMask(0x000200), new PciiValue(000200), (MessageIdentifier) MessageIdentifiers.SeePhoneForInstructions, Statuses.NotReady),
            new(new PciiMask(0x000000), new PciiValue(000000), (MessageIdentifier) MessageIdentifiers.Declined, Statuses.NotReady)
        });
    }

    public bool TryGetMatch(PosCardholderInteractionInformation pcii, out MessageTableEntry? messageTableEntry)
    {
        for (int i = 0; i < _Value.Length; i++)
        {
            if (_Value[i].IsMessageMatch(pcii))
            {
                messageTableEntry = _Value[i];

                return true;
            }
        }

        messageTableEntry = null;

        return false;
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PhoneMessageTable Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PhoneMessageTable Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    public static PhoneMessageTable Decode(ReadOnlySpan<byte> value)
    {
        MessageTableEntry[] result = MessageTableEntry.Decode(value);

        return new PhoneMessageTable(result);
    }

    public override byte[] EncodeValue()
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(_Value.Length * 8);
        Span<byte> buffer = spanOwner.Span;
        int offset = 0;

        for (int i = 0, j = 0; i < buffer.Length; i++)
            _Value[i].Encode(buffer, ref offset);

        return buffer.ToArray();
    }

    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(EncodeValue(), length);

    #endregion

    #region Equality

    public bool Equals(PhoneMessageTable? x, PhoneMessageTable? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PhoneMessageTable obj) => obj.GetHashCode();

    #endregion
}