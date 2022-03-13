using Play.Core;

namespace Play.Emv.DataElements;

public record PinEntryCapability : EnumObject<byte>
{
    #region Static Metadata

    public static readonly PosEntryMode Unknown = new PosEntryMode(0);
    public static readonly PosEntryMode PinPadCapable = new PosEntryMode(1);
    public static readonly PosEntryMode PinPadNotCapable = new PosEntryMode(2);

    #endregion

    #region Constructor

    public PinEntryCapability(byte value) : base(value)
    { }

    #endregion
}