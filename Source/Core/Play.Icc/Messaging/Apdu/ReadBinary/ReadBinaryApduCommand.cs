using System;

namespace Play.Icc.Messaging.Apdu.ReadBinary;

public class ReadBinaryApduCommand : ApduCommand
{
    #region Constructor

    private ReadBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1, parameter2)
    { }

    private ReadBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction, parameter1, parameter2,
        le)
    { }

    private ReadBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class, instruction,
        parameter1, parameter2, data)
    { }

    private ReadBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) : base(@class, instruction,
        parameter1, parameter2, data, le)
    { }

    #endregion
}