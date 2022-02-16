using System;

namespace Play.Icc.Messaging.Apdu.PutData;

public class PutDataApduCommand : ApduCommand
{
    #region Constructor

    protected PutDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    protected PutDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    protected PutDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    protected PutDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) : base(
        @class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static PutDataApduCommand Create(ProprietaryMessageIdentifier proprietaryMessageIdentifier, byte tag) =>
        new(new Class(proprietaryMessageIdentifier), Instruction.PutData, 0, tag);

    public static PutDataApduCommand Create(ProprietaryMessageIdentifier proprietaryMessageIdentifier, ushort tag) =>
        new(new Class(proprietaryMessageIdentifier), Instruction.GetData, (byte) (tag >> 8), (byte) tag);

    #endregion
}