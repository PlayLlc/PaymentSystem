using Play.Core.Exceptions;

namespace Play.Encryption.Encryption.Signing;

public class DecodedSignature
{
    #region Instance Values

    protected readonly byte[] _Hash;
    protected readonly byte _LeadingByte;
    protected readonly Message1 _Message1;
    protected readonly byte _TrailingByte;

    #endregion

    #region Constructor

    public DecodedSignature(byte[] value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        _LeadingByte = value[0];
        _Message1 = new Message1(value[2..^23]);
        _Hash = value[^22..^2];
        _TrailingByte = value[^1];
    }

    public DecodedSignature(byte leadingByte, Message1 message1, byte[] hash, byte trailingByte)
    {
        Validate(leadingByte, hash, trailingByte);
        _LeadingByte = leadingByte;
        _Message1 = message1;
        _Hash = hash;
        _TrailingByte = trailingByte;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray()
    {
        Span<byte> result = stackalloc byte[_Message1.GetByteCount() + 20 + 1 + 1];
        result[0] = _LeadingByte;
        _Message1.AsSpan().CopyTo(result[2..(2 + _Message1.GetByteCount())]);
        _Hash.AsSpan().CopyTo(result[^21..2]);
        result[^1] = _TrailingByte;

        return result.ToArray();
    }

    public int GetByteCount()
    {
        return _Message1.GetByteCount() + _Hash.Length + 2;
    }

    public byte[] GetHash()
    {
        return _Hash;
    }

    public byte GetLeadingByte()
    {
        return _LeadingByte;
    }

    public Message1 GetMessage1()
    {
        return _Message1;
    }

    public byte GetTrailingByte()
    {
        return _TrailingByte;
    }

    private static void Validate(byte leadingByte, ReadOnlySpan<byte> hash, byte trailingByte)
    {
        ValidateLeadingByte(leadingByte);
        ValidateHash(hash);
        ValidateTrailingByte(trailingByte);
    }

    private static void ValidateHash(ReadOnlySpan<byte> hash)
    {
        CheckCore.ForEmptySequence(hash, nameof(hash));

        if (hash.Length != SignatureSpecifications.HashLength)
            throw new ArgumentOutOfRangeException(nameof(hash), "A SHA-1 hash with 20 bytes was expected");
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void ValidateLeadingByte(byte leadingByte)
    {
        if (leadingByte != SignatureSpecifications.LeadingByte)
        {
            throw new ArgumentOutOfRangeException(nameof(leadingByte),
                                                  $"The value {SignatureSpecifications.LeadingByte} was expected but {leadingByte} was received instead");
        }
    }

    private static void ValidateTrailingByte(byte trailing)
    {
        if (trailing != SignatureSpecifications.TrailingByte)
        {
            throw new ArgumentOutOfRangeException(nameof(trailing),
                                                  $"The value {SignatureSpecifications.TrailingByte} was expected but {trailing} was received instead");
        }
    }

    #endregion
}