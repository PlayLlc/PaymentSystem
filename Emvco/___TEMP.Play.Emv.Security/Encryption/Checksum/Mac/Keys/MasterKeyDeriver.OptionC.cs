using System;
using System.Diagnostics.CodeAnalysis;

using Play.Emv.Card.Account;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{
    internal partial class MasterKeyDeriver
    {

        private static class OptionC
        {
            public static byte[] GetMasterKey(
                IBlockCipher aesCipher,
                CryptographicChecksum cryptogram,
                PrimaryAccountNumber accountNumber,
                [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
            {
                ReadOnlySpan<byte> y = ConcatAuxiliaryData(accountNumber, sequenceNumber);
                if (cryptogram.GetByteCount() == 8)
                    return Create8BlockSessionKey(aesCipher, cryptogram, y);

                return CreateNon8BlockSessionKey();
            }

            private static byte[] Create8BlockSessionKey(
                IBlockCipher aesCipher,
                CryptographicChecksum applicationCryptogram,
                ReadOnlySpan<byte> y)
            {
                return aesCipher.Sign(y, applicationCryptogram.AsByteArray());
            }

            // TODO: Book 2 Section A1.4.3 describes Option C as allowing 128, 192 or 256 bit keys, but
            // TODO: CryptographicChecksum in ISO 7816-4 only allows 4 - 8 bytes and ApplicationCryptogram
            // TODO: is a strict 8 bytes in length..
            private static byte[] CreateNon8BlockSessionKey()
            {
                throw new NotImplementedException();
            }

        }
    }
}
