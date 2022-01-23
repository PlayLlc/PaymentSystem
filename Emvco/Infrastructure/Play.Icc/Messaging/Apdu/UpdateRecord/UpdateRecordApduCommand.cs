using System;

namespace Play.Icc.Messaging.Apdu.UpdateRecord;

public class UpdateRecordApduCommand : ApduCommand
{
    #region Constructor

    private UpdateRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    private UpdateRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    private UpdateRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    private UpdateRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion
}