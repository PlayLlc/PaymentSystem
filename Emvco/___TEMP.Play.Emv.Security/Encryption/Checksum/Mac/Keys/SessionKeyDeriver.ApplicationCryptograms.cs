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
        internal static class ApplicationCryptograms
        {
            internal static byte[] DeriveSessionKey(
                IDeriveMasterKeys masterKeyDeriver,
                IBlockCipher cipher,
                CryptographicChecksum cryptogram,
                ushort applicationTransactionCount, // TODO: this should be encapsulated somewhere 
                PrimaryAccountNumber accountNumber,
                [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
            {
                ReadOnlySpan<byte> masterKey = masterKeyDeriver.GetMasterKey(cipher.GetAlgorithm(), cryptogram, accountNumber, sequenceNumber);

                if (cipher.GetKeySize() == KeySize._128)
                    return Get8BlockSessionKey(cipher, masterKey, GetDiversificationValue(applicationTransactionCount, cipher.GetKeySize()));

                return GetNBlockSessionKey(cipher, masterKey, GetDiversificationValue(applicationTransactionCount, cipher.GetKeySize()));
            }

            private static byte[] GetDiversificationValue(ushort applicationTransactionCounter, KeySize keySize)
            {
                byte[] result = new byte[keySize.GetByteSize()];

                result[0] = (byte)(applicationTransactionCounter >> 8);
                result[1] = (byte)applicationTransactionCounter;

                return result;
            }
        }
    }
}
