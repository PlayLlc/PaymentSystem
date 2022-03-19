using Play.Core;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber.Enums;

public record PinEntryCapability : EnumObject<byte>
{
    #region Static Metadata

    public static readonly PosEntryMode Unknown = new(0);
    public static readonly PosEntryMode PinPadCapable = new(1);
    public static readonly PosEntryMode PinPadNotCapable = new(2);

    #endregion

    #region Constructor

    public PinEntryCapability(byte value) : base(value)
    { }

    #endregion
}