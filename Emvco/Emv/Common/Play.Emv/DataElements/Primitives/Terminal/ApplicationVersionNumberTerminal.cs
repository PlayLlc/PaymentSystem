using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Version number assigned by the payment system for the application
/// </summary>
public record ApplicationVersionNumberTerminal : DataElement<ushort>, IEqualityComparer<ApplicationVersionNumberTerminal>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F09;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public ApplicationVersionNumberTerminal(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool IsCardholderVerificationIsSupported() => _Value.IsBitSet(5);
    public bool IsCdaSupported() => _Value.IsBitSet(1);
    public bool IsDdaSupported() => _Value.IsBitSet(6);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsIssuerAuthenticationIsSupported19() => _Value.IsBitSet(3);
    public bool IsSdaSupported() => _Value.IsBitSet(7);
    public bool TerminalRiskManagementIsToBePerformed() => _Value.IsBitSet(4);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationVersionNumberTerminal Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationVersionNumberTerminal Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new ApplicationVersionNumberTerminal(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ApplicationVersionNumberTerminal? x, ApplicationVersionNumberTerminal? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationVersionNumberTerminal obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator ushort(ApplicationVersionNumberTerminal value) => value._Value;

    #endregion
}