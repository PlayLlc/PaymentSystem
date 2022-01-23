using System;

using Play.Icc.Messaging.Apdu.GetChallenge;

namespace Play.Icc.Emv.GetChallenge;

public class GetChallengeCApduSignal : CApduSignal
{
    #region Constructor

    private GetChallengeCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    private GetChallengeCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    private GetChallengeCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    private GetChallengeCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static GetChallengeCApduSignal Create()
    {
        GetChallengeApduCommand cApdu = GetChallengeApduCommand.Create();

        return new GetChallengeCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(), cApdu.GetParameter2(),
            cApdu.GetData(), cApdu.GetLe());
    }

    #endregion
}