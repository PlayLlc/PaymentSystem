using System;

using Play.Icc.Exceptions;

namespace Play.Icc.Messaging.Apdu.ExternalAuthenticate;

public class ExternalAuthenticateApduCommand : ApduCommand
{
    #region Constructor

    private ExternalAuthenticateApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction,
                                                                                                                    parameter1, parameter2)
    { }

    private ExternalAuthenticateApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    private ExternalAuthenticateApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    private ExternalAuthenticateApduCommand(
        byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) : base(@class, instruction,
                                                                                                                  parameter1, parameter2,
                                                                                                                  data, le)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    /// </summary>
    /// <param name="issuerAuthenticationData">
    ///     The data field of the command message contains the value field of tag '91' coded as follows:
    ///     • mandatory first 8 bytes containing the cryptogram
    ///     • optional additional 1-8 bytes are proprietary
    /// </param>
    /// <returns></returns>
    /// <exception cref="IccProtocolException"></exception>
    public static ExternalAuthenticateApduCommand Create(ReadOnlySpan<byte> issuerAuthenticationData)
    {
        if ((issuerAuthenticationData.Length < 8) || (issuerAuthenticationData.Length > 16))
        {
            throw new IccProtocolException(new ArgumentOutOfRangeException(nameof(issuerAuthenticationData),
                                                                           $"The argument {nameof(issuerAuthenticationData)} had an unexpected byte count. The {nameof(issuerAuthenticationData)} must be between 8 and 16 bytes in length"));
        }

        return new ExternalAuthenticateApduCommand(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel),
                                                   Instruction.ExternalAuthenticate, 0, 0, issuerAuthenticationData.ToArray());
    }

    #endregion
}