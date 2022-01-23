using Play.Ber.Emv.DataObjects;

namespace Play.Icc.Emv.CardBlock;

public class CardBlockCApduSignal : CApduSignal
{
    #region Constructor

    public CardBlockCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
     parameter2)
    { }

    public CardBlockCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
     parameter1, parameter2, le)
    { }

    public CardBlockCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    public CardBlockCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    /// </summary>
    /// <param name="secureMessaging">
    ///     Valid values are <see cref="SecureMessaging.Authenticated" /> and <see cref="SecureMessaging.Proprietary" />
    /// </param>
    /// <param name=""></param>
    /// <param name="messageAuthenticationCode">
    ///     Message Authentication Code (MAC) data component; coding according to the secure messaging specified in Book 2
    /// </param>
    /// <returns></returns>
    public static CardBlockCApduSignal Create(Messaging.Apdu.SecureMessaging secureMessaging, CommandTemplate messageAuthenticationCode)
    {
        if ((secureMessaging != Messaging.Apdu.SecureMessaging.Authenticated)
            && (secureMessaging != Messaging.Apdu.SecureMessaging.Proprietary))
        {
            throw new ArgumentOutOfRangeException(nameof(secureMessaging),
                                                  $"The argument {nameof(secureMessaging)} was an unexpected value. The valid values are {nameof(Messaging.Apdu.SecureMessaging.Authenticated)} and {nameof(Messaging.Apdu.SecureMessaging.Proprietary)}");
        }

        return new CardBlockCApduSignal(new Class(secureMessaging), Instruction.CardBlock, 0, 0, messageAuthenticationCode.EncodeValue());
    }

    #endregion
}