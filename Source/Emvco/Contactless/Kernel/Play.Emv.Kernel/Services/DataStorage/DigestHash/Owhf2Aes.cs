﻿using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Encryption.Ciphers.Symmetric;

namespace Play.Emv.Kernel.Services;

public class Owhf2Aes
{
    #region Static Metadata

    private readonly IBlockCipher _Codec;

    #endregion

    #region Constructor

    public Owhf2Aes(BlockCipherConfiguration configuration)
    {
        _Codec = new AesCodec(configuration);
    }

    public Owhf2Aes()
    {
        _Codec = new AesCodec(new BlockCipherConfiguration(BlockCipherMode.Cbc, BlockPaddingMode.None, KeySize._128, BlockSize._16,
        new Iso7816PlainTextPreprocessor(BlockSize._16), null));
    }

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    public byte[] Hash(IReadTlvDatabase database, ReadOnlySpan<byte> message)
    {
        if (message.Length != 8)
            throw new TerminalDataException($"The argument {nameof(message)} must be 8 bytes in length");

        return CreateR(database, message);
    }

    #endregion

    #region Private Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    private byte[] CreateR(IReadTlvDatabase database, ReadOnlySpan<byte> inputC)
    {
        DataStorageId dataStorageId = database.Get<DataStorageId>(DataStorageId.Tag);
        DataStorageRequestedOperatorId operatorId = database.Get<DataStorageRequestedOperatorId>(DataStorageRequestedOperatorId.Tag);
        DataStorageOperatorDataSetInfo info = database.Get<DataStorageOperatorDataSetInfo>(DataStorageOperatorDataSetInfo.Tag);
        database.TryGet(DataStorageSlotManagementControl.Tag, out DataStorageSlotManagementControl? control);

        using SpanOwner<byte> owner = SpanOwner<byte>.Allocate(67);
        Span<byte> buffer = owner.Span;

        Span<byte> objectId = buffer[..8];
        Span<byte> message = buffer[8..24];
        Span<byte> y = buffer[24..35];
        Span<byte> key = buffer[35..51];
        Span<byte> t = buffer[51..67];

        ResolveObjectId(operatorId, info, control, objectId);
        CreateMessage(objectId, inputC, message);
        CreateY(dataStorageId, y);
        CreateKey(y, objectId, key);
        CreateT(key, message, t);

        return t[^8..].ToArray();
    }

    private static void CreateMessage(ReadOnlySpan<byte> objectId, ReadOnlySpan<byte> inputC, Span<byte> buffer)
    {
        inputC.CopyTo(buffer);
        objectId.CopyTo(buffer[^8..]);
    }

    private static void CreateY(DataStorageId dataStorageId, Span<byte> buffer)
    {
        dataStorageId.EncodeValue().CopyTo(buffer[^dataStorageId.GetValueByteCount()..]);
    }

    private static void CreateKey(ReadOnlySpan<byte> y, ReadOnlySpan<byte> objectId, Span<byte> buffer)
    {
        y.CopyTo(buffer);
        objectId[5..8].CopyTo(buffer[y.Length..]);
        buffer[14] = 0x3F;
    }

    private void CreateT(ReadOnlySpan<byte> key, ReadOnlySpan<byte> message, Span<byte> buffer)
    {
        byte[] signedMessage = _Codec.Encrypt(message, key);
        signedMessage.CopyTo(buffer);

        for(int i = 0; i < buffer.Length; i++)
        {
            unchecked
            {
                buffer[i] = ((byte)(buffer[i] ^ message[i]));
            }
        }
    }

    /// <exception cref="PlayInternalException"></exception>
    private static void ResolveObjectId(
        DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info, DataStorageSlotManagementControl? control, Span<byte> buffer)
    {
        if (control is null || (!control?.IsPermanent() ?? false) || !info.IsVolatile())
            operatorId.EncodeValue().CopyTo(buffer);
    }

    #endregion
}