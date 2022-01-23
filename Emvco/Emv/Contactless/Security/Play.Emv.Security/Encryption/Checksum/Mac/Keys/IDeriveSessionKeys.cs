using System.Diagnostics.CodeAnalysis;

using Play.Emv.Card.Account;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{
    public interface IDeriveSessionKeys
    {
        public byte[] DeriveForSecureMessaging(IBlockCipher cipher, CryptographicChecksum cryptogram,
            PrimaryAccountNumber accountNumber, [AllowNull] SequenceTraceAuditNumber? sequenceNumber);

        public byte[] DeriveForApplicationCryptogram(IBlockCipher cipher,
            CryptographicChecksum cryptogram,
            ushort applicationTransactionCount,
            PrimaryAccountNumber accountNumber,
            [AllowNull] SequenceTraceAuditNumber? sequenceNumber);

    }
}