using Play.Ber.Tags;

namespace Play.Testing.Emv.Ber.Primitive
{
    internal class IccPinEnciphermentPublicKeyCertificateTestTlv : TestTlv
    {
        private static readonly byte[] _DefaultContentOctets = { };

        public IccPinEnciphermentPublicKeyCertificateTestTlv(byte[] contentOctets) : base(contentOctets)
        {
        }

        public override Tag GetTag() => throw new NotImplementedException();
    }
}
