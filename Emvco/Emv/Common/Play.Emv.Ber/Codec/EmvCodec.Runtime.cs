using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Serialization;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber
{
    public partial class EmvCodec : BerCodec
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<Tag, PrimitiveValue> _DefaultPrimitiveMap;

        #endregion

        #region Constructor

        // BUG ===========================================================================================================
        // WARNING =======================================================================================================
        // HACK - THIS IS NOT A RESPONSIBLE WAY TO INITIALIZE THE DICTIONARY. INITIALIZE THE DICTIONARY WITHOUT REFLECTION
        // WARNING =======================================================================================================
        // BUG ===========================================================================================================

        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="ReflectionTypeLoadException"></exception>
        static EmvCodec()
        {
            _DefaultPrimitiveMap = GetDefaultPrimitiveValues().ToImmutableSortedDictionary(a => a.GetTag(), b => b);
        }

        #endregion

        #region Instance Members

        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="ReflectionTypeLoadException"></exception>
        private static HashSet<PrimitiveValue> GetDefaultPrimitiveValues()
        {
            Assembly uniqueAssemblies = typeof(EmvCodec).Assembly;

            HashSet<PrimitiveValue> codecs = new();

            foreach (Type type in uniqueAssemblies.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(DataElement<>))))
                codecs.Add((PrimitiveValue) FormatterServices.GetUninitializedObject(type));

            return codecs;
        }

        /// <exception cref="BerParsingException"></exception>
        public PrimitiveValue DecodePrimitiveValueAtRuntime(ReadOnlySpan<byte> value)
        {
            TagLengthValue tagLengthValue = _Codec.DecodeTagLengthValue(value);

            return _DefaultPrimitiveMap[tagLengthValue.GetTag()].Decode(tagLengthValue);
        }

        public bool TryDecodingPrimitiveValueAtRuntime(ReadOnlySpan<byte> value, out PrimitiveValue? result)
        {
            try
            {
                TagLengthValue tagLengthValue = _Codec.DecodeTagLengthValue(value);

                result = _DefaultPrimitiveMap[tagLengthValue.GetTag()].Decode(tagLengthValue);

                return true;
            }
            catch (BerParsingException)
            {
                // logging
                result = null;

                return false;
            }
            catch (Exception)
            {
                // logging
                result = null;

                return false;
            }
        }

        /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public IEnumerable<PrimitiveValue> DecodePrimitiveValuesAtRuntime(ReadOnlyMemory<byte> value)
        {
            TagLengthValue[] siblings = _Codec.DecodeTagLengthValues(value);

            for (int i = 0; i < siblings.Length; i++)
            {
                if (!_DefaultPrimitiveMap.ContainsKey(siblings[i].GetTag()))
                    continue;

                yield return _DefaultPrimitiveMap[siblings[i].GetTag()].Decode(siblings[i]);
            }
        }

        #endregion
    }
}