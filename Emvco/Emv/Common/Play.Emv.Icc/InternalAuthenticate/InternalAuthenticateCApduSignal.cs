using Play.Emv.Ber.DataObjects;
using Play.Icc.Messaging.Apdu.InternalAuthenticate;

namespace Play.Emv.Icc.InternalAuthenticate;

public class InternalAuthenticateCApduSignal : CApduSignal
{
    #region Constructor

    public InternalAuthenticateCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction,
        parameter1, parameter2)
    { }

    public InternalAuthenticateCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(@class,
        instruction, parameter1, parameter2, le)
    { }

    public InternalAuthenticateCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(
        @class, instruction, parameter1, parameter2, data)
    { }

    public InternalAuthenticateCApduSignal(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data,
        uint? le) : base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static InternalAuthenticateCApduSignal Create(DataObjectListResult dynamicAuthenticationDataObjectListResult)
    {
        InternalAuthenticateApduCommand cApdu =
            InternalAuthenticateApduCommand.Create(dynamicAuthenticationDataObjectListResult.AsByteArray());

        return new InternalAuthenticateCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(), cApdu.GetParameter2(),
            cApdu.GetData(), cApdu.GetLe());
    }

    #endregion
}