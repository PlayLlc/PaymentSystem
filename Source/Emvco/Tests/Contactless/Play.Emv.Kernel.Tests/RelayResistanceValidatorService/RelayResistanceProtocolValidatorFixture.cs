using AutoFixture;

using Play.Emv.Identifiers;

namespace Play.Emv.Kernel.Tests.RelayResistanceValidatorService;

public class RelayResistanceProtocolValidatorFixture
{
    public static void RegisterTransactionSessionId(IFixture fixture)
    {
        TransactionSessionId transactionSessionId = fixture.Create<TransactionSessionId>();
        fixture.Freeze<TransactionSessionId>();
    }

    public static void RegisterTerminalExpectedRelayResistanceProtocolValues()
    {

    }
}
