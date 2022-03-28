using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Emv.Ber.DataElements;
using Play.Encryption.Certificates;
using Play.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Static.Signed;

internal class SignedStaticApplicationDataDecoder
{
    #region Instance Values

    private readonly SignatureService _SignatureService;

    #endregion

    #region Constructor

    public SignedStaticApplicationDataDecoder(SignatureService signatureService)
    {
        _SignatureService = signatureService;
    }

    #endregion

    #region Instance Members

    private byte[] GetConcatenatedInput(
        DecodedSignedStaticApplicationData decodedSignedStaticApplicationData, ReadOnlySpan<byte> staticDataToBeAuthenticated)
    {
        // TODO : static tag list -> AIP is contingent on it existing here for validation
        ReadOnlySpan<byte> inputList = decodedSignedStaticApplicationData.GetConcatenatedInputList();
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(inputList.Length + staticDataToBeAuthenticated.Length);
        Span<byte> buffer = spanOwner.Span;

        inputList.CopyTo(buffer);
        staticDataToBeAuthenticated.CopyTo(buffer[^staticDataToBeAuthenticated.Length..]);

        return buffer.ToArray();
    }

    /// <summary>
    ///     IsValid
    /// </summary>
    /// <param name="issuerPublicKeyCertificate"></param>
    /// <param name="signedStaticApplicationData"></param>
    /// <param name="staticDataToBeAuthenticated"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsValid(
        PublicKeyCertificate issuerPublicKeyCertificate, SignedStaticApplicationData signedStaticApplicationData,
        ReadOnlySpan<byte> staticDataToBeAuthenticated)
    {
        if (signedStaticApplicationData.GetValueByteCount() != issuerPublicKeyCertificate.GetPublicKeyModulus().GetByteCount())
            return false;

        DecodedSignedStaticApplicationData decodedSignature =
            new(_SignatureService.Decrypt(signedStaticApplicationData.EncodeValue(), issuerPublicKeyCertificate));

        if (!_SignatureService.IsSignatureValid(decodedSignature.GetHashAlgorithmIndicator(), staticDataToBeAuthenticated,
                                                decodedSignature))
            return false;

        return true;
    }

    #endregion
}