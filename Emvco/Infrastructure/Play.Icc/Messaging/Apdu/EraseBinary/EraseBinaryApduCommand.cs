using System;

namespace Play.Icc.Messaging.Apdu.EraseBinary;

public class EraseBinaryApduCommand : ApduCommand
{
    #region Constructor

    private EraseBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    private EraseBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    private EraseBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    private EraseBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion
}