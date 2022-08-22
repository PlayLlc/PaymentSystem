using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader.Configuration
{
    public class ReaderPersistentConfiguration
    {
        #region Instance Values

        private readonly Dictionary<Tag, PrimitiveValue> _Configurations;

        #endregion

        #region Constructor

        public ReaderPersistentConfiguration(IResolveKnownObjectsAtRuntime runtimeCodec, TagLengthValue[] values)
        {
            _Configurations = new Dictionary<Tag, PrimitiveValue>();

            foreach (var value in DecodeTagLengthValues(runtimeCodec, values))
                _Configurations.Add(value.GetTag(), value);
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

        public PrimitiveValue[] GetPersistentConfigurations() => _Configurations.Values.ToArray();

        #endregion
    }
}