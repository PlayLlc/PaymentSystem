using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.Nfc.Tech;

namespace AndroidPcd
{
    internal class TechTransceiver
    {
        public byte[]? Transceive(BasicTagTechnology tech, byte[] value)
        {
            if (tech is IsoDep isoDep)
                return Transceive(isoDep, value);
            if (tech is NfcA nfcA)
                return Transceive(nfcA, value);
            if (tech is NfcB nfcB)
                return Transceive(nfcB, value);
            if (tech is NfcF nfcF)
                return Transceive(nfcF, value);

            throw new ArgumentOutOfRangeException($"Could not recognize the {nameof(BasicTagTechnology)}");

        }

        private byte[]? Transceive(IsoDep tech, byte[] value)
        {
            return tech.Transceive(value);
        }

        private byte[]? Transceive(NfcA tech, byte[] value)
        {
            return tech.Transceive(value);
        }

        private byte[]? Transceive(NfcB tech, byte[] value)
        {
            return tech.Transceive(value);
        }

        private byte[]? Transceive(NfcF tech, byte[] value)
        {
            return tech.Transceive(value);
        }
    }
}
