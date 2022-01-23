using System;

using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Encryption.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Offline.CombinedDataAuthentication;

internal class DecodedSignedDynamicApplicationDataCda : DecodedSignature
{
    #region Constructor

    public DecodedSignedDynamicApplicationDataCda(DecodedSignature decodedSignature) : base(decodedSignature.GetLeadingByte(),
                                                                                            decodedSignature.GetMessage1(),
                                                                                            decodedSignature.GetHash(),
                                                                                            decodedSignature.GetTrailingByte())
    { }

    #endregion

    #region Instance Members

    public HashAlgorithmIndicator GetHashAlgorithmIndicator()
    {
        if (!HashAlgorithmIndicator.TryGet(_Message1[1], out HashAlgorithmIndicator? result))
            return HashAlgorithmIndicator.NotAvailable;

        return result;
    }

    public IccDynamicData GetIccDynamicData()
    {
        return new IccDynamicData(_Message1[2..GetIccDynamicDataLength()].ToArray());
    }

    public byte GetIccDynamicDataLength()
    {
        return _Message1[2];
    }

    public byte[] GetPadPattern()
    {
        return _Message1[(GetIccDynamicDataLength() + 2)..].ToArray();
    }

    public byte[] GetSignatureHashPlainText(UnpredictableNumber unpredictableNumber)
    {
        Span<byte> result = stackalloc byte[_Message1.GetByteCount() + unpredictableNumber.GetByteCount()];

        _Message1.AsSpan().CopyTo(result);
        unpredictableNumber.EncodeValue().AsSpan().CopyTo(result[^unpredictableNumber.GetByteCount()..]);

        return result.ToArray();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public SignedDataFormat GetSignedDataFormat()
    {
        if (!SignedDataFormat.TryGet(_Message1[0], out SignedDataFormat? result))
            return SignedDataFormat.NotAvailable;

        return result!;
    }

    #endregion
}