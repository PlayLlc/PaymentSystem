using System;
using System.Diagnostics.CodeAnalysis;

using Play.Core.Extensions;
using Play.Emv.Card.Account;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{
    // TODO: This isn't needed from the terminal side. will save this and maybe use for testing responses
    // TODO: from the ICC one day
    internal partial class SessionKeyDeriver
    {
        #region Instance Values

        private readonly IDeriveMasterKeys _MasterKeyDeriver;

        #endregion

        public SessionKeyDeriver(IDeriveMasterKeys masterKeyDeriver)
        {
            _MasterKeyDeriver = masterKeyDeriver;
        }

        #region Instance Members

        /// <summary>
        /// Derive a session key for a Secure Message
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="cryptogram"></param>
        /// <param name="accountNumber"></param>
        /// <param name="sequenceNumber"></param>
        /// <returns></returns>
        public byte[] Derive(
            IBlockCipher cipher,
            CryptographicChecksum cryptogram, 
            PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
        { 
            return SecureMessaging.DeriveSessionKey(_MasterKeyDeriver, cipher, cryptogram, accountNumber, sequenceNumber);
        }

        /// <summary>
        /// Derive a session key for Application Cryptogram validation
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="cryptogram"></param>
        /// <param name="applicationTransactionCount"></param>
        /// <param name="accountNumber"></param>
        /// <param name="sequenceNumber"></param>
        /// <returns></returns>
        public byte[] Derive(IBlockCipher cipher,
            CryptographicChecksum cryptogram,
            ushort applicationTransactionCount, // TODO: this should be encapsulated somewhere 
            PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
        {
            return ApplicationCryptograms.DeriveSessionKey(_MasterKeyDeriver, cipher, cryptogram, applicationTransactionCount,
                accountNumber, sequenceNumber);
        } 

        private static byte[] Get8BlockSessionKey(IBlockCipher cipher,
            ReadOnlySpan<byte> masterKey,
            ReadOnlySpan<byte> diversificationValue)
        {
            return cipher.Sign(diversificationValue, masterKey);
        }

        private static byte[] GetNBlockSessionKey(IBlockCipher cipher,
            ReadOnlySpan<byte> masterKey,
            ReadOnlySpan<byte> diversificationValue)
        {
            ReadOnlySpan<byte> f1 = GetF1(diversificationValue);
            ReadOnlySpan<byte> f2 = GetF2(diversificationValue);

            return cipher.Sign(f1, masterKey).ConcatArrays(cipher.Sign(f2, masterKey));
        }
        private static byte[] GetF1(ReadOnlySpan<byte> diversificationValue)
        {
            const byte padValue = 0xF0;
            Span<byte> result = stackalloc byte[diversificationValue.Length];

            diversificationValue.CopyTo(result);

            for (int i = 2; i < diversificationValue.Length; i++)
                result[i] = padValue;

            return result.ToArray();
        }

        private static byte[] GetF2(ReadOnlySpan<byte> diversificationValue)
        {
            const byte padValue = 0x0F;
            Span<byte> result = stackalloc byte[diversificationValue.Length];

            diversificationValue.CopyTo(result);

            for (int i = 2; i < diversificationValue.Length; i++)
                result[i] = padValue;

            return result.ToArray();
        }
        #endregion
    }

   


   
}
