using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.Serialization;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;

namespace Play.Emv.Ber;

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
        _DefaultPrimitiveMap = GetDefaultPrimitiveValues().ToDictionary(a => a.GetTag(), b => b).ToImmutableSortedDictionary();
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
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(PrimitiveValue))))
            codecs.Add((PrimitiveValue) FormatterServices.GetUninitializedObject(type));

        List<Tag> tags = new();

        foreach (PrimitiveValue? prim in codecs)
        {
            try
            {
                tags.Add(prim.GetTag());
            }
            catch (Exception e)
            {
                Console.WriteLine("");
            }
        }

        HashSet<Tag> distinct = new(tags);
        IEnumerable<Tag> duplicates = tags.Except(distinct.ToList());

        foreach (Tag tag in duplicates)
            Console.WriteLine($"{tag}");

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

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public PrimitiveValue[] DecodePrimitiveValuesAtRuntime(ReadOnlySpan<byte> value)
    {
        TagLengthValue[] siblings = _Codec.DecodeTagLengthValues(value);
        PrimitiveValue[] result = new PrimitiveValue[siblings.Count(a => _DefaultPrimitiveMap.ContainsKey(a.GetTag()))];

        for (int i = 0, j = 0; i < siblings.Length; i++)
        {
            if (!_DefaultPrimitiveMap.ContainsKey(siblings[i].GetTag()))
                continue;

            result[j] = _DefaultPrimitiveMap[siblings[i].GetTag()].Decode(siblings[i]);
            j++;
        }

        return result;
    }

    #endregion
}