using Play.Codecs;
using Play.Core.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes.Card;

namespace Play.Emv.Ber.ValueTypes;

public partial record MessageTableEntry
{
    #region Static Metadata

    private const byte _ByteCount = 8;

    #endregion

    #region Instance Values

    private readonly PciiMask _Mask;
    private readonly PciiValue _Value;
    private readonly MessageIdentifier _MessageIdentifier;
    private readonly Statuses _Statuses;

    #endregion

    #region Constructor

    /// <exception cref="PlayInternalException"></exception>
    public MessageTableEntry(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteCount)
            throw new PlayInternalException($"The {nameof(MessageTableEntry)} must be initialized with an 8 byte value");

        _Mask = new PciiMask(PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(value[..3]));
        _Value = new PciiValue(PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(value[3..6]));
        _MessageIdentifier = new MessageIdentifier(value[6]);
        _Statuses = new Statuses(value[7]);
    }

    internal MessageTableEntry(PciiMask mask, PciiValue value, MessageIdentifier messageIdentifier, Statuses statuses)
    {
        _Mask = mask;
        _Value = value;
        _MessageIdentifier = messageIdentifier;
        _Statuses = statuses;
    }

    #endregion

    #region Instance Members

    public void Encode(Span<byte> buffer, ref int offset)
    {
        _Mask.Decode(buffer, ref offset);
        _Value.Decode(buffer, ref offset);
        _MessageIdentifier.Decode(buffer, ref offset);
        _Statuses.Decode(buffer, ref offset);
    }

    public bool IsMessageMatch(PosCardholderInteractionInformation pcii) => pcii.GetMaskedValue(this) == _Value;
    internal PciiMask GetPciiMask() => _Mask;
    public MessageIdentifier GetMessageIdentifier() => _MessageIdentifier;
    public Statuses GetStatus() => _Statuses;

    #endregion

    #region Serialization

    /// <exception cref="PlayInternalException"></exception>
    public static MessageTableEntry[] Decode(ReadOnlySpan<byte> value)
    {
        MessageTableEntry[] result = new MessageTableEntry[value.Length / 8];

        for (int i = 0, j = 0; i < result.Length; i++)
            result[i] = new MessageTableEntry(value[j..(j + 8)]);

        return result;
    }

    #endregion
}