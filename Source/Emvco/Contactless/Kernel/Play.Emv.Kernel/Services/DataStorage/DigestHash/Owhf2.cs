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

    private static readonly IBlockCipher _Codec;

    #endregion

    #region Constructor

    static Owhf2()
    {
        _Codec = new TripleDesCodec(new BlockCipherConfiguration(BlockCipherMode.Cbc, BlockPaddingMode.None, KeySize._128, BlockSize._8,
        new Iso7816PlainTextPreprocessor(BlockSize._8)));
    }

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static byte[] Hash(IReadTlvDatabase database, ReadOnlySpan<byte> message)
    {
        if (message.Length != 8)
            throw new TerminalDataException($"The argument {nameof(message)} must be 8 bytes in length");

        using SpanOwner<byte> owner = SpanOwner<byte>.Allocate(24);
        Span<byte> buffer = owner.Span;

        Span<byte> objectId = buffer[..8];
        Span<byte> key = buffer[8..];

        ResolveKey(database, objectId, key);

        return EncryptTripleDes(message, objectId, key);
    }

    public static void UpdateInitializationVector(byte[] initializationVector)
    {
        _Codec.SetInitializationVector(initializationVector);
    }

    #endregion

    #region TripleDES

    private static byte[] EncryptTripleDes(ReadOnlySpan<byte> message, Span<byte> objectId, Span<byte> key)
    {
        ////OID = OID ⊕ PD
        ResolveMessage(objectId, message);

        ////Triple DES(K)[OID ⊕ PD]
        byte[] tripleDesEncryption = _Codec.Encrypt(objectId, key);

        ////R := DES(KL)[DES-1(KR)[DES(KL)[OID ⊕ PD]]] ⊕ PD
        ResolveMessage(tripleDesEncryption, message);

        return tripleDesEncryption;
    }

    private static void ResolveMessage(Span<byte> input, ReadOnlySpan<byte> message)
    {
        for (int i = 0; i < input.Length; i++)
        {
            unchecked
            {
                input[i] = (byte)(input[i] ^ message[i]);
            }
        }
    }

    #endregion

    #region Key Generation

    /// <remarks>EMVco Book C-2 Section 8.2 </remarks>
    private static int GetPermanentSlotIdLength(DataStorageId id) => id.GetValueByteCount();

    /// <exception cref="PlayInternalException"></exception>
    private static void ResolveObjectId(
        DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info, DataStorageSlotManagementControl? control, Span<byte> buffer)
    {
        if (control is null || (!control?.IsPermanent() ?? false) || !info.IsVolatile())
            operatorId.EncodeValue().CopyTo(buffer);
    }

    /// <exception cref="PlayInternalException"></exception>
    private static void ResolveLeftKey(ReadOnlySpan<byte> objectId, DataStorageId dataStorageId, Span<byte> buffer)
    {
        ReadOnlySpan<byte> dataStorageIdContentOctets = dataStorageId.EncodeValue();

        for (int i = 0; i < 6; i++)
        {
            unchecked
            {
                buffer[i] = (byte) ((((dataStorageIdContentOctets[i] / 16) * 10) + (dataStorageIdContentOctets[i] % 16)) * 2);
            }
        }

        objectId[4..6].CopyTo(buffer[^2..]);
    }

    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="OverflowException"></exception>
    private static void ResolveRightKey(ReadOnlySpan<byte> objectId, DataStorageId dataStorageId, Span<byte> buffer)
    {
        ReadOnlySpan<byte> dataStorageIdContentOctets = dataStorageId.EncodeValue();
        int permanentSlotIdLength = GetPermanentSlotIdLength(dataStorageId);

        for (int i = 0; i < 6; i++)
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