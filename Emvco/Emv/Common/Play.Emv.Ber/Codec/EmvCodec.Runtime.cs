using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Exceptions;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber
{
    public readonly ref struct Test
    { }

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

        private PrimitiveValue DecodePrimitiveValueAtRuntime(TagLengthValue value)
        {
            if (!_DefaultPrimitiveMap.ContainsKey(value.GetTag()))
            {
                throw new
                    PlayInternalException($"A {nameof(EmvCodec)} configuration error has occurred. An attempt to parse a {nameof(PrimitiveValue)} was made but there are no decoding configurations for that object");
            }

            return _DefaultPrimitiveMap[value.GetTag()]._Decoder(value[tagLength.GetValueOffset()..]);
        }

        public bool TryDecodingPrimitiveValueAtRuntime(ReadOnlySpan<byte> value, out PrimitiveValue? result)
        {
            try
            {
                TagLength tagLength = _Codec.DecodeTagLength(value.Span);

                result = _DefaultPrimitiveMap[tagLength.GetTag()]._Decoder(value[tagLength.GetValueOffset()..]);

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
        public IEnumerable<PrimitiveValue> DecodeSiblingsAtRuntime(ReadOnlyMemory<byte> value)
        {
            EncodedTlvSiblings siblings = _Codec.DecodeChildren(value);
            uint[] tags = siblings.GetTags();

            for (int i = 0; i < siblings.SiblingCount(); i++)
            {
                if (!_DefaultPrimitiveMap.ContainsKey(tags[i]))
                    continue;

                if (!siblings.TryGetValueOctetsOfChild(tags[i], out ReadOnlyMemory<byte> childContentOctets))
                    continue;

                yield return _DefaultPrimitiveMap[tags[i]]._Decoder(childContentOctets);
            }
        }

        #endregion
    }
}