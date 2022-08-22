using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Contracts;

public class KernelPersistentConfiguration
{
    #region Instance Values

    protected readonly KernelId _KernelId;
    protected readonly List<PrimitiveValue> _PersistentValues;

    #endregion

    #region Constructor

    public KernelPersistentConfiguration(KernelId kernelId, PrimitiveValue[] persistentValues)
    {
        _KernelId = kernelId;
        _PersistentValues = new List<PrimitiveValue>(persistentValues);
    }

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

    public KernelId GetKernelId() => _KernelId;
    public IEnumerable<PrimitiveValue> GetPersistentValues() => _PersistentValues;

    #endregion
}