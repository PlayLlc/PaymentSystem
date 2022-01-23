using System;

namespace Play.Icc.Messaging.Apdu.GetData;

public class GetDataApduCommand : ApduCommand
{
    #region Constructor

    protected GetDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
     parameter2)
    { }

    protected GetDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
     parameter1, parameter2, le)
    { }

    protected GetDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    protected GetDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static GetDataApduCommand Create(ProprietaryMessageIdentifier proprietaryMessageIdentifier, byte tag) =>
        new(new Class(proprietaryMessageIdentifier), Instruction.GetData, 0, tag);

    public static GetDataApduCommand Create(ProprietaryMessageIdentifier proprietaryMessageIdentifier, ushort tag) =>
        new(new Class(proprietaryMessageIdentifier), Instruction.GetData, (byte) (tag >> 8), (byte) tag);

    #endregion
}