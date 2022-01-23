using System;

namespace Play.Icc.Messaging.Apdu.InternalAuthenticate;

public class InternalAuthenticateApduCommand : ApduCommand
{
    #region Constructor

    private InternalAuthenticateApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction,
        parameter1, parameter2)
    { }

    private InternalAuthenticateApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class,
        instruction, parameter1, parameter2, le)
    { }

    private InternalAuthenticateApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    private InternalAuthenticateApduCommand(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data,
        uint le) : base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static InternalAuthenticateApduCommand Create(ReadOnlySpan<byte> dynamicAuthenticationDataObjectList) =>
        new(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.InternalAuthenticate, 0, 0,
            dynamicAuthenticationDataObjectList, 0);

    #endregion
}