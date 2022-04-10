using System;

namespace Play.Icc.Messaging.Apdu.Verify;

public class VerifyApduCommand : ApduCommand
{
    #region Constructor

    private VerifyApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
                                                                                                      parameter2)
    { }

    private VerifyApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
                                                                                                               parameter1, parameter2, le)
    { }

    private VerifyApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    private VerifyApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion
}