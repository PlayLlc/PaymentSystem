using System;

namespace Play.Icc.Messaging.Apdu.GetChallenge;

public class GetChallengeApduCommand : ApduCommand
{
    #region Constructor

    private GetChallengeApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1, parameter2)
    { }

    private GetChallengeApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction, parameter1,
        parameter2, le)
    { }

    private GetChallengeApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class, instruction,
        parameter1, parameter2, data)
    { }

    private GetChallengeApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) : base(@class,
        instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static GetChallengeApduCommand Create() =>
        new(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.GetChallenge, 0, 0, 0);

    #endregion
}