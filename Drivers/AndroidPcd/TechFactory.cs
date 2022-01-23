using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.Nfc;
using Android.Nfc.Tech;

namespace AndroidPcd
{
    internal class TechFactory
    {
        public BasicTagTechnology GetTech(Tag tag)
        {
            if (TryGetIsoDep(tag, out IsoDep? isoDep))
                return isoDep!;
            if (TryGetNfcA(tag, out NfcA? nfcA))
                return nfcA!;
            if (TryGetNfcA(tag, out NfcB? nfcB))
                return nfcB!;

            throw new ArgumentOutOfRangeException($"Could not recognize an {nameof(ITagTechnology)} from the {nameof(tag)}");
        }

        private bool TryGetIsoDep(Tag tag, out IsoDep? isoDep)
        {

            isoDep = IsoDep.Get(tag);

            if (isoDep == null)
                return false;

            return true;
        }

        private bool TryGetNfcA(Tag tag, out NfcA? nfcA)
        {
            nfcA = NfcA.Get(tag);

            if (nfcA == null)
                return false;

            return true;
        }

        private bool TryGetNfcA(Tag tag, out NfcB? nfcB)
        {
            nfcB = NfcB.Get(tag);

            if (nfcB == null)
                return false;

            return true;
        }
    }
}
