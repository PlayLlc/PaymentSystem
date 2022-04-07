using Play.Codecs;
using Play.Core.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber;

public partial record MessageTableEntry
{
    #region Static Metadata

    private const byte _ByteCount = 8;

    #endregion

    #region Instance Values

    private readonly PciiMask _Mask;
    private readonly PciiValue _Value;
    private readonly MessageIdentifier _MessageIdentifier;
    private readonly Status _Status;

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
        _Status = new Status(value[7]);
    }

    #endregion

    #region Instance Members

    public void Encode(Span<byte> buffer, ref int offset)
    {
        _Mask.Decode(buffer, ref offset);
        _Value.Decode(buffer, ref offset);
        _MessageIdentifier.Decode(buffer, ref offset);
        _Status.Decode(buffer, ref offset);
    }

    public bool IsMessageMatch(PosCardholderInteractionInformation pcii) => pcii.GetMaskedValue(this) == _Value;
    internal PciiMask GetPciiMask() => _Mask;
    public MessageIdentifier GetMessageIdentifier() => _MessageIdentifier;
    public Status GetStatus() => _Status;

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