using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader
{
    public class KernelConfigurations
    {
        #region Instance Values

        private readonly Dictionary<KernelId, PrimitiveValue[]> _Configurations;

        #endregion

        #region Constructor

        public KernelConfigurations(IResolveKnownObjectsAtRuntime runtimeCodec, KernelPersistentConfiguration[] values)
        {
            _Configurations = new Dictionary<KernelId, PrimitiveValue[]>();

            foreach (var value in values)
                _Configurations.Add(value.GetKernelId(), DecodeTagLengthValues(runtimeCodec, value.GetPersistentValues()));
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

        public PrimitiveValue[] Get(KernelId kernelId)
        {
            if (!_Configurations.TryGetValue(kernelId, out PrimitiveValue[]? primitiveValueResult))
                return Array.Empty<PrimitiveValue>();

            return primitiveValueResult;
        }

        #endregion
    }
}