using System;
using System.Diagnostics.CodeAnalysis;

using Play.Core.Extensions;
using Play.Emv.Card.Account;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{

    internal partial class MasterKeyDeriver
    { 
        private static class OptionA
        {
            public static byte[] GetMasterKey(
                IBlockCipher tripleDesCipher,
                CryptographicChecksum cryptogram,
                PrimaryAccountNumber accountNumber,
                [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
            {
                ReadOnlySpan<byte> y = ConcatAuxiliaryData(accountNumber, sequenceNumber);
                ReadOnlySpan<byte> leftKey = tripleDesCipher.Sign(y, cryptogram.AsByteArray());
                ReadOnlySpan<byte> rightKey = tripleDesCipher.Sign(y.ReverseBits(), cryptogram.AsByteArray());

                return leftKey.ConcatArrays(rightKey);
            }
             

        }
    }
}
