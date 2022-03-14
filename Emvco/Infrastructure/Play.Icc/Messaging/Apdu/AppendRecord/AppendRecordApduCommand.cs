using System;

namespace Play.Icc.Messaging.Apdu.AppendRecord;

public class AppendRecordApduCommand : ApduCommand
{
    #region Constructor

    private AppendRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
     parameter2)
    { }

    private AppendRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
     parameter1, parameter2, le)
    { }

    private AppendRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    private AppendRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion
}