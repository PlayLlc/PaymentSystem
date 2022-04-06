using Play.Core.Extensions;

namespace Play.Emv.Ber
{
    internal readonly record struct PciiValue
    {
        private readonly uint _Value;
        private const uint _UnrelatedBits = 0xFF000000;

        public PciiValue(uint value)
        {
            _Value = value.ClearBits(_UnrelatedBits);
        }

        public void Decode(Span<byte> buffer, ref int offset)
        {
            buffer[offset++] = (byte) (_Value >> 16);
            buffer[offset++] = (byte) (_Value >> 8);
            buffer[offset++] = (byte) _Value;
        }

        public static explicit operator uint(PciiValue value) => value._Value;
        public static bool operator ==(PciiValue left, uint right) => left._Value == right;
        public static bool operator !=(PciiValue left, uint right) => left._Value != right;
        public static bool operator ==(uint left, PciiValue right) => left == right._Value;
        public static bool operator !=(uint left, PciiValue right) => left != right._Value;
    }
}