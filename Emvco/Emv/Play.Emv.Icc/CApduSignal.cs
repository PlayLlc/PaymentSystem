using Play.Icc.Messaging.Apdu;

namespace Play.Icc.Emv;

public abstract class CApduSignal : ApduCommand
{
    #region Constructor

    protected CApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
     parameter2)
    { }

    protected CApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(@class, instruction, parameter1,
     parameter2, le)
    { }

    protected CApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    protected CApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion
}