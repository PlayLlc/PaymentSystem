using System.Diagnostics.CodeAnalysis;

using Play.Emv.Card.Account;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{
    internal interface IDeriveMasterKeys
    {
        public byte[] GetMasterKey(BlockCipherAlgorithm cipherAlgorithm, CryptographicChecksum cryptogram,
            PrimaryAccountNumber accountNumber, [AllowNull] SequenceTraceAuditNumber? sequenceNumber);
    }
}