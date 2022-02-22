﻿using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Icc;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Contains the AC type indicated by the Terminal for which Integrated Data Storage data must be stored in the Card.
/// </summary>
public record DataStorageApplicationCryptogramType : DataElement<byte>, IEqualityComparer<DataStorageApplicationCryptogramType>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0xDF8108;

    #endregion

    #region Constructor

    public DataStorageApplicationCryptogramType(byte value) : base(value)
    {
        if (!CryptogramTypes.IsValid(value))
            throw new ArgumentException($"The argument {nameof(value)} was not recognized as a valid {nameof(CryptogramTypes)}");
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetBerEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DataStorageApplicationCryptogramType Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataStorageApplicationCryptogramType Decode(ReadOnlySpan<byte> value) => new(value[0]);

    #endregion

    #region Equality

    public bool Equals(DataStorageApplicationCryptogramType? x, DataStorageApplicationCryptogramType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataStorageApplicationCryptogramType obj) => obj.GetHashCode();

    #endregion
}