using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Emv.Card.Account;
using Play.Emv.Security.Contracts;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{
    // TODO: This isn't needed from the terminal side. will save this and maybe use for testing responses
    // TODO: from the ICC one day
    internal partial class MasterKeyDeriver
    {
        private readonly IBlockCipher _TripleDesCodec;
        private readonly IBlockCipher _AesCodec;
        private readonly IHashGenerator _HashGenerator;

        public MasterKeyDeriver(
            IBlockCipher tripleDesCodec,
            IBlockCipher aesCodec,
            IHashGenerator hashGenerator)
        {
            if (tripleDesCodec.GetAlgorithm() != BlockCipherAlgorithm.TripleDes)
                throw new ArgumentOutOfRangeException(nameof(tripleDesCodec));
            if (aesCodec.GetAlgorithm() != BlockCipherAlgorithm.Aes)
                throw new ArgumentOutOfRangeException(nameof(aesCodec));
            if (hashGenerator.GetHashAlgorithmIndicator() != HashAlgorithmIndicator.Sha1)
                throw new ArgumentOutOfRangeException(nameof(hashGenerator));

            _TripleDesCodec = tripleDesCodec;
            _AesCodec = aesCodec;
            _HashGenerator = hashGenerator;
        }

        #region Instance members
        public byte[] GetMasterKey(
            BlockCipherAlgorithm cipherAlgorithm,
            CryptographicChecksum cryptogram,
            PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
        {
            if (cipherAlgorithm == BlockCipherAlgorithm.Aes)
                return GetAesSessionKey(_AesCodec, cryptogram, accountNumber, sequenceNumber);

            return GetTripleDesMasterKey(_HashGenerator, _TripleDesCodec, cryptogram, accountNumber, sequenceNumber);

        }

        private static byte[] GetTripleDesMasterKey(
            IHashGenerator sha1Hasher,
            IBlockCipher tripleDesCipher,
            CryptographicChecksum cryptogram,
            PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
        {
            if (accountNumber.GetNumberOfDigits() <= 16)
                return MasterKeyDeriver.OptionA.GetMasterKey(tripleDesCipher, cryptogram, accountNumber, sequenceNumber);

            return MasterKeyDeriver.OptionB.GetMasterKey(sha1Hasher, tripleDesCipher, cryptogram, accountNumber, sequenceNumber);
        }

        private static byte[] GetAesSessionKey(
            IBlockCipher aesCipher,
            CryptographicChecksum cryptogram,
            PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
        {
            return MasterKeyDeriver.OptionC.GetMasterKey(aesCipher, cryptogram, accountNumber, sequenceNumber);
        } 

        internal static byte[] ConcatAuxiliaryData(PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
        {
            const byte concatenatedResultByteLength = 8;

            SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(accountNumber.GetNumberOfDigits() + accountNumber.GetNumberOfDigits() % 2
                + sequenceNumber?.GetNumberOfDigits() ?? 1);
            Span<byte> buffer = spanOwner.Span; 

            accountNumber.AsNumericByteArray().CopyTo(buffer);  
            sequenceNumber?.AsByteArray().CopyTo(buffer[^sequenceNumber!.Value.GetNumberOfDigits()..]);

            if (buffer.Length >= concatenatedResultByteLength) 
                return buffer[^concatenatedResultByteLength..].ToArray(); 

            byte[] result = new byte[8];

            for (int i = concatenatedResultByteLength, j = buffer.Length; j > 0; i--, j--)
                result[i] = buffer[j];

            return result; 
        }
        #endregion
    } 
}