using Play.Core.Exceptions;
using Play.Encryption.Ciphers.Hashing;

namespace Play.Encryption.Signing;

public class DecodedSignature
{
    #region Instance Values

    protected readonly Hash _Hash;
    protected readonly byte _LeadingByte;
    protected readonly Message1 _Message1;
    protected readonly byte _TrailingByte;

    #endregion

    #region Constructor

    public DecodedSignature(byte[] value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        _LeadingByte = value[0];
        _Message1 = new Message1(value[1..^21]);
        _Hash = new Hash(value[^21..^1]);
        _TrailingByte = value[^1];
    }

    public DecodedSignature(byte leadingByte, Message1 message1, Hash hash, byte trailingByte)
    {
        Validate(leadingByte, trailingByte);
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
        _Hash.Encode(result, 1);
        result[^1] = _TrailingByte;

        return result.ToArray();
    }

    public int GetByteCount() => _Message1.GetByteCount() + Hash.Length + 2;
    public Hash GetHash() => _Hash;
    public byte GetLeadingByte() => _LeadingByte;
    public Message1 GetMessage1() => _Message1;
    public byte GetTrailingByte() => _TrailingByte;

    private static void Validate(byte leadingByte, byte trailingByte)
    {
        ValidateLeadingByte(leadingByte);
        ValidateTrailingByte(trailingByte);
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