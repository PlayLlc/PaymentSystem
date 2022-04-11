using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Emv.Ber.DataElements;
using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Hashing;
using Play.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Sda;

internal class DecodedSignedStaticApplicationData : DecodedSignature
{
    #region Instance Values

    private readonly HashAlgorithmIndicator _HashAlgorithmIndicator;
    private readonly PublicKeyAlgorithmIndicator _PublicKeyAlgorithmIndicator;

    #endregion

    #region Constructor

    public DecodedSignedStaticApplicationData(DecodedSignature decodedSignature) : base(decodedSignature.GetLeadingByte(),
        decodedSignature.GetMessage1(), decodedSignature.GetHash(), decodedSignature.GetTrailingByte())
    { }

    private DecodedSignedStaticApplicationData(byte leadingByte, Message1 message1, Hash hash, byte trailingByte) : base(leadingByte,
        message1, hash, trailingByte)
    { }

    #endregion

    #region Instance Members

    public byte[] GetConcatenatedInputList()
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(GetByteCount() - 22);
        Span<byte> buffer = spanOwner.Span;
        Span<byte> dataAuthenticationCode = stackalloc byte[2];
        dataAuthenticationCode = GetDataAuthenticationCode().AsBytes();

        buffer[0] = GetSignedDataFormat();
        buffer[1] = (byte) GetHashAlgorithmIndicator();
        buffer[2] = dataAuthenticationCode[0];
        buffer[3] = dataAuthenticationCode[1];
        GetPadPattern().AsSpan().CopyTo(buffer[4..]);

        return buffer.ToArray();
    }

    public DataAuthenticationCode GetDataAuthenticationCode() =>
        new(PlayCodec.UnsignedIntegerCodec.DecodeToUInt16(_Message1.AsByteArray()[3..4]));

    public HashAlgorithmIndicator GetHashAlgorithmIndicator()
    {
        if (!HashAlgorithmIndicator.TryGet(_Message1[16], out HashAlgorithmIndicator? result))
            return HashAlgorithmIndicator.NotAvailable;

        return result!;
    }

    public byte[] GetPadPattern() => _Message1.AsByteArray()[5..(GetByteCount() - 26)];
    public byte GetSignedDataFormat() => _Message1[1];

    #endregion
}