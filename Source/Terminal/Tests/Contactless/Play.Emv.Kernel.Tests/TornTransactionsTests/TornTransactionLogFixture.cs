using AutoFixture;

using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Kernel.Tests.TornTransactionsTests;

public class TornTransactionLogFixture
{
    private static Nibble[] _TpanEncodedValue = new Nibble[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x0, 0x1, 0x2, 0x3 };

    //This is temporary until the ApplicationPanBuilder will be merged from TerminalRiskManager Tests PR.
    public static void RegisterApplicationPan(IFixture fixture)
    {
        TrackPrimaryAccountNumber value = new TrackPrimaryAccountNumber(_TpanEncodedValue);

        ApplicationPan applicationPan = new ApplicationPan(value);
        fixture.Register(() => applicationPan);
    }

    public static void RegisterApplicationPanSequenceNumber(IFixture fixture)
    {
        ApplicationPanSequenceNumber applicationPanSequenceNumber = new ApplicationPanSequenceNumber(27);

        fixture.Register(() => applicationPanSequenceNumber);
    }
}
