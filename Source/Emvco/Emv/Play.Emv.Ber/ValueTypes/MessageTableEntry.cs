using Play.Codecs;
using Play.Core.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes.Card;

namespace Play.Emv.Ber.ValueTypes;

public record MessageTableEntry
{
    #region Static Metadata

    private const byte _ByteCount = 8;

    #endregion

    #region Instance Values

    private readonly PciiMask _Mask;
    private readonly PciiValue _Value;
    private readonly DisplayMessageIdentifier _DisplayMessageIdentifier;
    private readonly DisplayStatuses _DisplayStatuses;

    #endregion

    #region Constructor

    /// <exception cref="PlayInternalException"></exception>
    public MessageTableEntry(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteCount)
            throw new PlayInternalException($"The {nameof(MessageTableEntry)} must be initialized with an 8 byte value");

        _Mask = new PciiMask(PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(value[..3]));
        _Value = new PciiValue(PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(value[3..6]));
        _DisplayMessageIdentifier = new DisplayMessageIdentifier(value[6]);
        _DisplayStatuses = new DisplayStatuses(value[7]);
    }

    internal MessageTableEntry(PciiMask mask, PciiValue value, DisplayMessageIdentifier displayMessageIdentifier, DisplayStatuses displayStatuses)
    {
        _Mask = mask;
        _Value = value;
        _DisplayMessageIdentifier = displayMessageIdentifier;
        _DisplayStatuses = displayStatuses;
    }

    #endregion

    #region Instance Members

    public ushort GetByteCount() => _ByteCount;

    public void Encode(Span<byte> buffer, ref int offset)
    {
        _Mask.Decode(buffer, ref offset);
        _Value.Decode(buffer, ref offset);
        _DisplayMessageIdentifier.Decode(buffer, ref offset);
        _DisplayStatuses.Decode(buffer, ref offset);
    }

    public bool IsMessageMatch(PosCardholderInteractionInformation pcii) => pcii.GetMaskedValue(this) == _Value;
    internal PciiMask GetPciiMask() => _Mask;
    public DisplayMessageIdentifier GetMessageIdentifier() => _DisplayMessageIdentifier;
    public DisplayStatuses GetStatus() => _DisplayStatuses;

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