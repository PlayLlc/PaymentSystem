using System;
using System.Diagnostics.CodeAnalysis;

using Play.Emv.Card.Account;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{
    // TODO: This isn't needed from the terminal side. will save this and maybe use for testing responses
    // TODO: from the ICC one day
    internal partial class MacGenerator
    {
        private readonly IFormatPlainText _Iso7816Padder; 
        private readonly IDeriveSessionKeys _SessionKeyDeriver;

        private readonly IBlockCipher _TripleDesCbcCipher;
        private readonly IBlockCipher _AesCbcCipher;

        public MacGenerator(IFormatPlainText iso7816Padder,
            IDeriveSessionKeys sessionKeyDeriver,
            IBlockCipher tripleDesCbcCipher,
            IBlockCipher aesCbcCipher)
        {
            // TODO: this is rediculous, update the cipher to at least overload methods to pass options 
            if (tripleDesCbcCipher.GetAlgorithm() != BlockCipherAlgorithm.TripleDes)
                throw new ArgumentOutOfRangeException(nameof(tripleDesCbcCipher));
            if(tripleDesCbcCipher.GetCipherMode() != BlockCipherMode.Cbc) 
                throw new ArgumentOutOfRangeException(nameof(tripleDesCbcCipher));
            //... Aes Check - outdated when overload method

            _Iso7816Padder = iso7816Padder;
            _SessionKeyDeriver = sessionKeyDeriver;
            _TripleDesCbcCipher = tripleDesCbcCipher;
            _AesCbcCipher = aesCbcCipher;
        } 


        public MessageAuthenticationCode Generate(
            CryptographicChecksum cryptogram,
            ushort applicationTransactionCount,
            PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
        {

        }

        public MessageAuthenticationCode Generate(
            CryptographicChecksum cryptogram,
            PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
        {

        }

    }

    internal partial class MacGenerator
    {
        internal static class EightByteBlock
        {
            public static byte[] PadMessage(IFormatPlainText iso7816PadFormatter, ReadOnlySpan<byte> value)
            {
                return iso7816PadFormatter.Format(value);
            }

            public static byte[] DeriveSessionKey(IDeriveSessionKeys sessionKeyDeriver,
                MessageAuthenticationCodeType macType)
            {

            }
            { 
        }
    }

    internal partial class MacGenerator
    {
        public static class SixteenByteBlock
        {

        }
    }
}
