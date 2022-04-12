using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Encryption.Ciphers.Symmetric;

namespace Play.Emv.Kernel.Services;

/// <summary>
///     OWHF2 is the DES-based variant of the one-way function for computing the digest. OWHF2 computes an 8-byte output R
///     based on an 8-byte input PD.
/// </summary>
/// <remarks>EMVco Book C-2 Section 8.2</remarks>
public class Owhf2
{
    #region Static Metadata

    private static readonly TripleDesCodec _Codec = new(new BlockCipherConfiguration(BlockCipherMode.Cbc, BlockPaddingMode.None,
        KeySize._128, BlockSize._8, new Iso7816PlainTextPreprocessor(BlockSize._8)));

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static byte[] Sign(IReadTlvDatabase database, ReadOnlySpan<byte> message)
    {
        using SpanOwner<byte> owner = SpanOwner<byte>.Allocate(32);
        Span<byte> buffer = owner.Span;

        Span<byte> objectId = buffer[..16];
        Span<byte> key = buffer[16..];

        ResolveKey(database, objectId, key);

        return _Codec.Sign(message, key);
    }

    #endregion

    #region Key Generation

    /// <remarks>EMVco Book C-2 Section 8.2 </remarks>
    public static int GetPermanentSlotIdLength(DataStorageId id) => id.GetValueByteCount();

    /// <exception cref="PlayInternalException"></exception>
    public static void ResolveObjectId(
        DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info, DataStorageSlotManagementControl? control,
        Span<byte> buffer)
    {
        if (control is not null)
            operatorId.EncodeValue().CopyTo(buffer);

        if (!control?.IsPermanent() ?? false)
            operatorId.EncodeValue().CopyTo(buffer);

        if (info.IsVolatile())
            operatorId.EncodeValue().CopyTo(buffer);
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void ResolveLeftKey(ReadOnlySpan<byte> objectId, DataStorageId dataStorageId, Span<byte> buffer)
    {
        ReadOnlySpan<byte> dataStorageIdContentOctets = dataStorageId.EncodeValue();

        for (int i = 0; i < 6; i++)
        {
            unchecked
            {
                buffer[i] = (byte) ((((dataStorageIdContentOctets[i - 1] / 16) * 10) + (dataStorageIdContentOctets[i - 1] % 16)) * 2);
            }
        }

        objectId[4..6].CopyTo(buffer[^2..]);
    }

    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static void ResolveRightKey(ReadOnlySpan<byte> objectId, DataStorageId dataStorageId, Span<byte> buffer)
    {
        ReadOnlySpan<byte> dataStorageIdContentOctets = dataStorageId.EncodeValue();
        int permanentSlotIdLength = GetPermanentSlotIdLength(dataStorageId);

        for (int i = 0; i < buffer.Length; i++)
        {
            unchecked
            {
                buffer[i] = (byte) ((((dataStorageIdContentOctets[(permanentSlotIdLength - 6) + i] / 16) * 10)
                        + (dataStorageIdContentOctets[(permanentSlotIdLength - 6) + i] % 16))
                    * 2);
            }
        }

        objectId[^2..].CopyTo(buffer[^2..]);
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="OverflowException"></exception>
    private static void ResolveKey(IReadTlvDatabase database, Span<byte> objectId, Span<byte> buffer)
    {
        DataStorageId dataStorageId = database.Get<DataStorageId>(DataStorageId.Tag);
        DataStorageRequestedOperatorId operatorId = database.Get<DataStorageRequestedOperatorId>(DataStorageRequestedOperatorId.Tag);
        DataStorageOperatorDataSetInfo info = database.Get<DataStorageOperatorDataSetInfo>(DataStorageOperatorDataSetInfo.Tag);
        database.TryGet(DataStorageSlotManagementControl.Tag, out DataStorageSlotManagementControl? control);

        ResolveObjectId(operatorId, info, control, objectId);

        ResolveLeftKey(objectId, dataStorageId, buffer[..8]);
        ResolveRightKey(objectId, dataStorageId, buffer[8..]);
    }

    #endregion
}