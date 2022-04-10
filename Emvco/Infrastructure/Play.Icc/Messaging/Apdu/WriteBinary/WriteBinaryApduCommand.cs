using System;

namespace Play.Icc.Messaging.Apdu.WriteBinary;

public class WriteBinaryApduCommand : ApduCommand
{
    #region Constructor

    private WriteBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
                                                                                                           parameter2)
    { }

    private WriteBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
                                                                                                                    parameter1, parameter2,
                                                                                                                    le)
    { }

    private WriteBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    private WriteBinaryApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion
}