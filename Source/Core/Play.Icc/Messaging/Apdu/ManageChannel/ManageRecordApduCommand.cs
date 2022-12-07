using System;

namespace Play.Icc.Messaging.Apdu.ManageChannel;

public class ManageRecordApduCommand : ApduCommand
{
    #region Constructor

    private ManageRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1, parameter2)
    { }

    private ManageRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction, parameter1,
        parameter2, le)
    { }

    private ManageRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class, instruction,
        parameter1, parameter2, data)
    { }

    private ManageRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) : base(@class,
        instruction, parameter1, parameter2, data, le)
    { }

    #endregion
}