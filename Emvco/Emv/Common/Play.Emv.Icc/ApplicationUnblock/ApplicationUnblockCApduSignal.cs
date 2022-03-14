using Play.Emv.Ber.DataObjects;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

public class ApplicationUnBlockCApduSignal : CApduSignal
{
    #region Constructor

    public ApplicationUnBlockCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction,
     parameter1, parameter2)
    { }

    public ApplicationUnBlockCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    public ApplicationUnBlockCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    public ApplicationUnBlockCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le)
        : base(@class, instruction, parameter1, parameter2, data, le)
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
    public static ApplicationUnBlockCApduSignal Create(SecureMessaging secureMessaging, CommandTemplate messageAuthenticationCode)
    {
        if ((secureMessaging != SecureMessaging.Authenticated) && (secureMessaging != SecureMessaging.Proprietary))
        {
            throw new ArgumentOutOfRangeException(nameof(secureMessaging),
                                                  $"The argument {nameof(secureMessaging)} was an unexpected value. The valid values are {nameof(SecureMessaging.Authenticated)} and {nameof(SecureMessaging.Proprietary)}");
        }

        return new ApplicationUnBlockCApduSignal(new Class(secureMessaging), Instruction.ApplicationUnblock, 0, 0,
                                                 messageAuthenticationCode.EncodeValue());
    }

    #endregion
}