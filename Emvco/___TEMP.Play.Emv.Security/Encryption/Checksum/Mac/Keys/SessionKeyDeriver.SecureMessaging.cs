using System;
using System.Diagnostics.CodeAnalysis;

using Play.Emv.Card.Account;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{
    internal partial class SessionKeyDeriver
    {
        internal static class SecureMessaging
        {
            public static byte[] DeriveSessionKey(
                IDeriveMasterKeys masterKeyDeriver,
                IBlockCipher cipher,
                CryptographicChecksum cryptogram,
                PrimaryAccountNumber accountNumber,
                [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
            {
                ReadOnlySpan<byte> masterKey = masterKeyDeriver.GetMasterKey(cipher.GetAlgorithm(), cryptogram, accountNumber, sequenceNumber);

                //return cipher.Sign(GetDiversificationValue(cryptogram), masterKey);
                if (cipher.GetKeySize() == KeySize._128)
                    return Get8BlockSessionKey(cipher, masterKey, GetDiversificationValue(cryptogram, cipher.GetKeySize()));

                return GetNBlockSessionKey(cipher, masterKey, GetDiversificationValue(cryptogram, cipher.GetKeySize()));
            }

            private static byte[] GetDiversificationValue(CryptographicChecksum cryptogram, KeySize keySize)
            {
                Span<byte> result = stackalloc byte[keySize.GetByteSize()];
                cryptogram.AsByteArray().CopyTo(result);
                return result.ToArray();
            }
        }
    }
}
