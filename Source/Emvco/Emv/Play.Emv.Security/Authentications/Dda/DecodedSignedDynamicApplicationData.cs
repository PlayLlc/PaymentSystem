using Play.Core;
using Play.Encryption.Ciphers.Hashing;
using Play.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Dda;

internal class DecodedSignedDynamicApplicationData : DecodedSignature
{
    #region Constructor

    public DecodedSignedDynamicApplicationData(DecodedSignature decodedSignature) : base(decodedSignature.GetLeadingByte(), decodedSignature.GetMessage1(),
        decodedSignature.GetHash(), decodedSignature.GetTrailingByte())
    { }

    #endregion

    #region Instance Members

    public HashAlgorithmIndicators GetHashAlgorithmIndicator()
    {
        if (!HashAlgorithmIndicators.Empty.TryGet(_Message1[1], out EnumObject<byte>? result))
            return HashAlgorithmIndicators.NotAvailable;

        return (HashAlgorithmIndicators) result!;
    }

    public IccDynamicData GetIccDynamicData() => new(_Message1[3..GetIccDynamicDataLength()].ToArray());
    public byte GetIccDynamicDataLength() => _Message1[2];
    public byte[] GetPadPattern() => _Message1[(GetIccDynamicDataLength() + 2)..].ToArray();

    public SignedDataFormat GetSignedDataFormat()
    {
        if (!SignedDataFormat.TryGet(_Message1[0], out SignedDataFormat? result))
            return SignedDataFormat.NotAvailable;

        return result!;
    }

    #endregion
}