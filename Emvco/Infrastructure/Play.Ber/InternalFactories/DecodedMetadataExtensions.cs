using System;
using System.Numerics;

using Play.Ber.Exceptions;

namespace Play.Ber.InternalFactories;

public static class DecodedMetadataExtensions
{
    #region Instance Members

    public static DecodedResult<BigInteger>? ToBigInteger(this DecodedMetadata? value)
    {
        if (value == null)
            return null;

        if (value is DecodedResult<byte> decodedResultByte)
            return new DecodedResult<BigInteger>(decodedResultByte.Value, decodedResultByte.CharCount);
        if (value is DecodedResult<ushort> decodedResultUShort)
            return new DecodedResult<BigInteger>(decodedResultUShort.Value, decodedResultUShort.CharCount);
        if (value is DecodedResult<uint> decodedResultUInt)
            return new DecodedResult<BigInteger>(decodedResultUInt.Value, decodedResultUInt.CharCount);
        if (value is DecodedResult<ulong> decodedResultULong)
            return new DecodedResult<BigInteger>(decodedResultULong.Value, decodedResultULong.CharCount);

        throw new BerInternalException("This error should never be thrown");
    }

    /// <summary>
    ///     ToUInt64Result
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static DecodedResult<ulong>? ToUInt64Result(this DecodedMetadata? value)
    {
        if (value == null)
            return null;

        if (value is DecodedResult<byte> decodedResultByte)
            return new DecodedResult<ulong>(decodedResultByte.Value, decodedResultByte.CharCount);
        if (value is DecodedResult<ushort> decodedResultUShort)
            return new DecodedResult<ulong>(decodedResultUShort.Value, decodedResultUShort.CharCount);
        if (value is DecodedResult<uint> decodedResultUInt)
            return new DecodedResult<ulong>(decodedResultUInt.Value, decodedResultUInt.CharCount);

        throw new BerInternalException("This error should never be thrown");
    }

    /// <summary>
    ///     ToUInt32Result
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static DecodedResult<uint>? ToUInt32Result(this DecodedMetadata? value)
    {
        if (value == null)
            return null;

        if (value is DecodedResult<byte> decodedResultByte)
            return new DecodedResult<uint>(decodedResultByte.Value, decodedResultByte.CharCount);
        if (value is DecodedResult<ushort> decodedResultUShort)
            return new DecodedResult<uint>(decodedResultUShort.Value, decodedResultUShort.CharCount);

        throw new BerInternalException("This error should never be thrown");
    }

    /// <summary>
    ///     ToUInt16Result
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static DecodedResult<ushort>? ToUInt16Result(this DecodedMetadata? value)
    {
        if (value == null)
            return null;

        if (value is DecodedResult<byte> decodedResultByte)
            return new DecodedResult<ushort>(decodedResultByte.Value, decodedResultByte.CharCount);

        throw new BerInternalException("This error should never be thrown");
    }

    public static DecodedResult<byte>? ToByteResult(this DecodedMetadata? value)
    {
        if (value == null)
            return null;

        if (value is DecodedResult<byte> decodedResultByte)
            return new DecodedResult<byte>(decodedResultByte.Value, decodedResultByte.CharCount);

        throw new BerInternalException("This error should never be thrown");
    }

    #endregion
}