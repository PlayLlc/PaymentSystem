using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;

[assembly: InternalsVisibleTo("Play.Ber.Tests")]
[assembly: InternalsVisibleTo("Play.Ber.Emv.Tests")]
[assembly: InternalsVisibleTo("Play.Emv.TestData")]
[assembly: InternalsVisibleTo("Play.Icc.Emv.Tests")]

namespace Play.Ber;

public class BerConfiguration
{
    #region Instance Values

    internal IDictionary<BerEncodingId, BerPrimitiveCodec> _PrimitiveCodecMap;

    #endregion

    #region Constructor

    /// <summary>
    ///     The configuration used to instantiate a <see cref="BerCodec" /> object
    /// </summary>
    /// <param name="assembliesToMap">
    ///     The assemblies that contain classes that derive from <see cref="PrimitiveValue" />,
    ///     <see cref="IConstructedValue" />, or
    ///     <see cref="BerPrimitiveCodec" />. The assemblies will be scanned for the derived classes and an internal encoding
    ///     map for those types will be created.
    /// </param>
    /// <exception cref="BerException"></exception>
    public BerConfiguration(IList<Assembly> assembliesToMap)
    {
        Assembly[] assemblies = assembliesToMap?.ToArray() ?? throw new BerFormatException(nameof(assembliesToMap));

        List<BerPrimitiveCodec> berPrimitiveCodecs = GetBerPrimitiveCodecs(assemblies);

        _PrimitiveCodecMap = berPrimitiveCodecs.ToDictionary(a => a.GetIdentifier(), b => b);
    }

    public BerConfiguration(IDictionary<BerEncodingId, BerPrimitiveCodec> primitiveCodecMap)
    {
        _PrimitiveCodecMap = primitiveCodecMap;
    }

    #endregion

    #region Instance Members

    private List<BerPrimitiveCodec> GetBerPrimitiveCodecs(Assembly[] assemblies)
    {
        HashSet<Assembly> uniqueAssemblies = new(assemblies.Select(a => a));

        List<BerPrimitiveCodec> codecs = new();

        foreach (Type type in uniqueAssemblies.SelectMany(a => a.GetTypes())
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BerPrimitiveCodec))))
            codecs.Add((BerPrimitiveCodec) FormatterServices.GetUninitializedObject(type));

        return codecs;
    }

    private static List<ConstructedValue> GetConstructedValues(params Assembly[] constructorArgs)
    {
        HashSet<Assembly> uniqueAssemblies = new(constructorArgs.Select(a => a));

        List<ConstructedValue> codecs = new();

        foreach (Type type in uniqueAssemblies.SelectMany(a => a.GetTypes())
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ConstructedValue))))
            codecs.Add((ConstructedValue) FormatterServices.GetUninitializedObject(type));

        return codecs;
    }

    private static List<PrimitiveValue> GetPrimitiveValues(params Assembly[] constructorArgs)
    {
        HashSet<Assembly> uniqueAssemblies = new(constructorArgs.Select(a => a));

        List<PrimitiveValue> codecs = new();

        foreach (Type type in uniqueAssemblies.SelectMany(a => a.GetTypes())
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(PrimitiveValue))))
            codecs.Add((PrimitiveValue) FormatterServices.GetUninitializedObject(type));

        return codecs;
    }

    /// <remarks>
    ///     This method is generic so that each list can be validated independently so that the exception can be thrown
    ///     as soon as it is found for each type being validated
    /// </remarks>
    /// <exception cref="BerException"></exception>
    private void Validate(IList<BerPrimitiveCodec> values)
    {
        ValidateBerPrimitiveCodecIdentifiersAreUnique(values);
        ValidateBerEncodingIdsAreUnique(values.Select(a => a.GetIdentifier()).ToList());
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void Validate(IList<BerPrimitiveCodec> berPrimitiveCodecs, IList<PrimitiveValue> primitiveValues)
    {
        ValidatePrimitiveValuesAreAllMappedToABerPrimitiveCodec(new HashSet<BerPrimitiveCodec>(berPrimitiveCodecs),
                                                                new HashSet<PrimitiveValue>(primitiveValues));
    }

    /// <exception cref="BerException"></exception>
    private static void ValidateBerEncodingIdsAreUnique(IList<BerEncodingId> mappingValue)
    {
        List<BerEncodingId> distinct = mappingValue.Select(a => a).Distinct().ToList();

        if (distinct.Count == mappingValue.Count)
            return;

        List<string> duplicateValues = mappingValue.Except(distinct).Select(a => a.GetType().Name).ToList();

        const string stringSeparator = ", ";

        throw new
            BerFormatException($"Duplicate {nameof(BerEncodingId)} - There was a problem found while building the encoding mapping for {nameof(BerPrimitiveCodec)} because there were duplicate values. "
                               + $"Here is a list of the {nameof(BerPrimitiveCodec)} that have a {nameof(BerEncodingId)} that is not unique: [{string.Join(stringSeparator, duplicateValues)}");
    }

    /// <summary>
    ///     ValidateBerPrimitiveCodecIdentifiersAreUnique
    /// </summary>
    /// <param name="berPrimitiveCodecs"></param>
    /// <exception cref="BerException"></exception>
    private static void ValidateBerPrimitiveCodecIdentifiersAreUnique(IList<BerPrimitiveCodec> berPrimitiveCodecs)
    {
        List<BerEncodingId> allTags = berPrimitiveCodecs.Select(a => a.GetIdentifier()).ToList();
        HashSet<BerEncodingId> uniqueIdentifiers = new();
        List<BerEncodingId> duplicateIdentifiers = new();

        foreach (BerEncodingId tag in allTags)
        {
            if (!uniqueIdentifiers.Add(tag))
                duplicateIdentifiers.Add(tag);
        }

        if (uniqueIdentifiers.Count == berPrimitiveCodecs.Count)
            return;

        Dictionary<BerEncodingId, string[]> codecsWithTheSameIdentifiers = new();

        foreach (BerEncodingId identifier in duplicateIdentifiers)
        {
            codecsWithTheSameIdentifiers.Add(identifier,
                                             berPrimitiveCodecs.Where(a => a.GetIdentifier() == identifier).Select(a => a.GetType().Name)
                                                 .ToArray());
        }

        StringBuilder builder = new();

        foreach (KeyValuePair<BerEncodingId, string[]> keyValue in codecsWithTheSameIdentifiers)
        {
            const string stringSeparator = ", ";
            builder.Append($"[0x{keyValue.Key}: ");
            builder.Append($"{string.Join(stringSeparator, keyValue.Value)}] \n");
        }

        throw new
            BerFormatException($"Duplicate Identifiers - There was a problem found while building the encoding mapping for {nameof(BerPrimitiveCodec)} because some of the types use the same identifier. The Fully Qualified Name of each {nameof(BerPrimitiveCodec)} implementation must be unique. Here is a list of the types that implement the same Identifier: {builder}");
    }

    /// <exception cref="InvalidOperationException"></exception>
    private static void ValidatePrimitiveValuesAreAllMappedToABerPrimitiveCodec(
        HashSet<BerPrimitiveCodec> codecs,
        HashSet<PrimitiveValue> primitiveValues)
    {
        HashSet<BerEncodingId> encodingIds = new(codecs.Select(a => a.GetIdentifier()));
        Dictionary<BerEncodingId, List<string>> unmappedPrimitiveValues = new();

        foreach (PrimitiveValue value in primitiveValues)
        {
            if (encodingIds.Contains(value.GetBerEncodingId()))
                continue;

            if (!unmappedPrimitiveValues.ContainsKey(value.GetBerEncodingId()))
                unmappedPrimitiveValues.Add(value.GetBerEncodingId(), new List<string> {value.GetType().Name});
            else
                unmappedPrimitiveValues[value.GetBerEncodingId()].Add(value.GetType().Name);
        }

        StringBuilder builder = new();

        foreach (KeyValuePair<BerEncodingId, List<string>> keyValue in unmappedPrimitiveValues)
        {
            const string stringSeparator = ", ";
            builder.Append($"[0x{keyValue.Key}: ");
            builder.Append($"{string.Join(stringSeparator, keyValue.Value)}] \n");
        }

        throw new
            BerInternalException($"There was a runtime error when validating that all {nameof(PrimitiveValue)} subclasses are correctly mapped to a {nameof(BerPrimitiveCodec)}. Please check the {nameof(BerConfiguration)} for any errors. Here's a list of unmapped {nameof(PrimitiveValue)} subclasses: {builder}");
    }

    /// <summary>
    ///     This method is to be used to test that the compiled ASN.1 objects will all map correctly during serialization. The
    ///     ASN.1 objects (i.e. subclasses of <see cref="PrimitiveValue" /> and <see cref="IConstructedValue" />) will be
    ///     mapped
    ///     to specific formatting codecs, <see cref="BerPrimitiveCodec" />, based on the values supplied to the constructor in
    ///     this class
    /// </summary>
    /// <warning>
    ///     NOTE: This method is NOT intended to be run in a production environment
    /// </warning>
    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void ValidateRuntimeConfiguration(IList<Assembly> asn1CompiledAssemblies)
    {
        List<PrimitiveValue>? primitiveValues = GetPrimitiveValues(asn1CompiledAssemblies.ToArray());
        List<ConstructedValue>? constructedValues = GetConstructedValues(asn1CompiledAssemblies.ToArray());
        List<BerPrimitiveCodec> berPrimitiveCodecs = _PrimitiveCodecMap.Select(a => a.Value).ToList();
        Validate(berPrimitiveCodecs);
        Validate(berPrimitiveCodecs, primitiveValues);
    }

    /// <summary>
    ///     This method is to be used to test that the compiled ASN.1 objects will all map correctly during serialization. The
    ///     ASN.1 objects (i.e. subclasses of <see cref="PrimitiveValue" /> and <see cref="IConstructedValue" />) will be
    ///     mapped
    ///     to specific formatting codecs, <see cref="BerPrimitiveCodec" />, based on the values supplied to the constructor in
    ///     this class
    /// </summary>
    /// <warning>
    ///     NOTE: This method is NOT intended to be run in a production environment
    /// </warning>
    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void ValidateRuntimeConfiguration(IList<Assembly> primitiveValueAssemblies, IList<Assembly> constructedValueAssemblies)
    {
        List<PrimitiveValue>? primitiveValues = GetPrimitiveValues(primitiveValueAssemblies.ToArray());
        List<ConstructedValue>? constructedValues = GetConstructedValues(constructedValueAssemblies.ToArray());
        List<BerPrimitiveCodec> berPrimitiveCodecs = _PrimitiveCodecMap.Select(a => a.Value).ToList();
        Validate(berPrimitiveCodecs);
        Validate(berPrimitiveCodecs, primitiveValues);
    }

    #endregion
}