using Play.Ber.Exceptions;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

public class RecoverApplicationCryptogramCApduSignal : CApduSignal
{
    #region Constructor

    protected RecoverApplicationCryptogramCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class,
        instruction, parameter1, parameter2)
    { }

    protected RecoverApplicationCryptogramCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(
        @class, instruction, parameter1, parameter2, le)
    { }

    protected RecoverApplicationCryptogramCApduSignal(
        byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class, instruction, parameter1,
        parameter2, data)
    { }

    protected RecoverApplicationCryptogramCApduSignal(
        byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) : base(@class, instruction,
        parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="BerParsingException"></exception>
    public static RecoverApplicationCryptogramCApduSignal Create(ReadOnlySpan<byte> ddolRelatedData) =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.RecoverApplicationCryptogram, 0, 0,
            ddolRelatedData, 0);

    #endregion
}