using System;

namespace Play.Icc.Messaging.Apdu.UpdateBinary;

public class UpdateBinaryApduCommand : ApduCommand
{
    #region Constructor

    private UpdateBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1, parameter2)
    { }

    private UpdateBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction, parameter1,
        parameter2, le)
    { }

    private UpdateBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class, instruction,
        parameter1, parameter2, data)
    { }

    private UpdateBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) : base(@class,
        instruction, parameter1, parameter2, data, le)
    { }

    #endregion
}