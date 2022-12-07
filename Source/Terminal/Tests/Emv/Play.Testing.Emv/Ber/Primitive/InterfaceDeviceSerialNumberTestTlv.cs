using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class InterfaceDeviceSerialNumberTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { (byte)'A', (byte)'B', (byte)'C', (byte)'d', (byte)'e', (byte)'f', (byte)'0', (byte)'9' };

    public InterfaceDeviceSerialNumberTestTlv() : base(_DefaultContentOctets) { }

    public InterfaceDeviceSerialNumberTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => InterfaceDeviceSerialNumber.Tag;
}
