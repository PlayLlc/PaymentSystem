using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

public class ExchangeRelayResistanceDataCApduSignal : CApduSignal
{
    #region Constructor

    private ExchangeRelayResistanceDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) :
        base(@class, instruction, parameter1, parameter2)
    { }

    private ExchangeRelayResistanceDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    private ExchangeRelayResistanceDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data)
        : base(@class, instruction, parameter1, parameter2, data)
    { }

    private ExchangeRelayResistanceDataCApduSignal(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data,
        uint le) : base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ExchangeRelayResistanceDataCApduSignal Create(ReadOnlySpan<byte> terminalRelayResistanceEntropy)
    {
        if (terminalRelayResistanceEntropy.Length != 4)
            throw new ArgumentOutOfRangeException(nameof(terminalRelayResistanceEntropy));

        return new ExchangeRelayResistanceDataCApduSignal(new Class(SecureMessaging.Proprietary), Instruction.ExchangeRelayResistanceData,
                                                          0, 0, terminalRelayResistanceEntropy, 0);
    }

    #endregion
}