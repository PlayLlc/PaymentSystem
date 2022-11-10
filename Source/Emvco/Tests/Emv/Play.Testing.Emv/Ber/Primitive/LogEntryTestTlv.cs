using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class LogEntryTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 24, 46 };

    public LogEntryTestTlv() : base(_DefaultContentOctets) { }

    public LogEntryTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => LogEntry.Tag;
}
