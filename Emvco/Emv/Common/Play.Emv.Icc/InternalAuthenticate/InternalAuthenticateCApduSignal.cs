using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Icc.Messaging.Apdu.InternalAuthenticate;

namespace Play.Emv.Icc;

public class InternalAuthenticateCApduSignal : CApduSignal
{
    #region Constructor

    public InternalAuthenticateCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction,
     parameter1, parameter2)
    { }

    public InternalAuthenticateCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    public InternalAuthenticateCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
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

    /// <summary>
    ///     Create
    /// </summary>
    /// <param name="dynamicAuthenticationDataObjectListResult"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public static InternalAuthenticateCApduSignal Create(DataObjectListResult dynamicAuthenticationDataObjectListResult)
    {
        InternalAuthenticateApduCommand cApdu =
            InternalAuthenticateApduCommand.Create(dynamicAuthenticationDataObjectListResult.AsByteArray());

        return new InternalAuthenticateCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(), cApdu.GetParameter2(),
                                                   cApdu.GetData(), cApdu.GetLe());
    }

    #endregion
}