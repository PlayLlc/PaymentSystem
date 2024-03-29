﻿using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Reader.Configuration;

namespace MockPos.Dtos;

public class KernelPersistentConfigurationDto
{
    #region Instance Values

    public byte KernelId { get; set; }
    public List<TagLengthValueDto> TagLengthValues { get; set; } = new();

    #endregion

    #region Instance Members

    private static PrimitiveValue[] DecodeTagLengthValues(IResolveKnownObjectsAtRuntime runtimeCodec, TagLengthValue[] values)
    {
        List<PrimitiveValue> result = new();

        foreach (var tlv in values)
        {
            if (!runtimeCodec.TryDecodingAtRuntime(tlv.GetTag(), tlv.EncodeValue(), out PrimitiveValue? primitiveValueResult))
                continue;

            result.Add(primitiveValueResult!);
        }

        return result.ToArray();
    }

    #endregion

    #region Serialization

    public KernelPersistentConfiguration Decode(IResolveKnownObjectsAtRuntime runtimeCodec)
    {
        List<TagLengthValue> tagLengthValues = new();

        foreach (TagLengthValueDto? tlv in TagLengthValues)
            tagLengthValues.Add(tlv.Decode());

        return new KernelPersistentConfiguration(new KernelId(KernelId), DecodeTagLengthValues(runtimeCodec, tagLengthValues.ToArray()));
    }

    #endregion
}

public class ReaderPersistentConfigurationDto
{
    #region Instance Values

    public byte KernelId { get; set; }
    public List<TagLengthValueDto> TagLengthValues { get; set; } = new();

    #endregion

    #region Instance Members

    private static PrimitiveValue[] DecodeTagLengthValues(IResolveKnownObjectsAtRuntime runtimeCodec, TagLengthValue[] values)
    {
        List<PrimitiveValue> result = new();

        foreach (var tlv in values)
        {
            if (!runtimeCodec.TryDecodingAtRuntime(tlv.GetTag(), tlv.EncodeValue(), out PrimitiveValue? primitiveValueResult))
                continue;

            result.Add(primitiveValueResult!);
        }

        return result.ToArray();
    }

    #endregion

    #region Serialization

    public ReaderPersistentConfiguration Decode(IResolveKnownObjectsAtRuntime runtimeCodec)
    {
        List<PrimitiveValue> tagLengthValues = new();

        foreach (TagLengthValueDto? tlv in TagLengthValues)
        {
            var tlvDecoded = tlv.Decode();

            if (!runtimeCodec.TryDecodingAtRuntime(tlvDecoded.GetTag(), tlvDecoded.EncodeValue(), out PrimitiveValue? result))
                continue;

            tagLengthValues.Add(result!);
        }

        return new ReaderPersistentConfiguration(tagLengthValues.ToArray());
    }

    #endregion
}