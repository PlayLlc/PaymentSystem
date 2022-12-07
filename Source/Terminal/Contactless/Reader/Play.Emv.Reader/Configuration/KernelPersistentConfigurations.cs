using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader
{
    public class KernelPersistentConfigurations
    {
        #region Instance Values

        private readonly Dictionary<KernelId, KernelPersistentConfiguration> _Configurations;

        #endregion

        #region Constructor

        public KernelPersistentConfigurations(KernelPersistentConfiguration[] values)
        {
            _Configurations = values.ToDictionary(a => a.GetKernelId(), b => b);
        }

        #endregion

        #region Instance Members

        public IEnumerable<PrimitiveValue> Get(KernelId kernelId)
        {
            if (!_Configurations.TryGetValue(kernelId, out KernelPersistentConfiguration? primitiveValueResult))
                return Array.Empty<PrimitiveValue>();

            return primitiveValueResult.GetPersistentValues();
        }

        #endregion
    }
}