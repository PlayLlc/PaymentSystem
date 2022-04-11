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
using Play.Codecs;

[assembly: InternalsVisibleTo("Play.Ber.Tests")]
[assembly: InternalsVisibleTo("Play.Emv.Ber.Tests")]
[assembly: InternalsVisibleTo("Play.Emv.TestData")]
[assembly: InternalsVisibleTo("Play.Emv.Icc.Tests")]

namespace Play.Ber;

public class BerConfiguration
{
    #region Instance Values

    internal IDictionary<PlayEncodingId, PlayCodec> _PlayCodecMap;

    #endregion

    #region Constructor

    /// <summary>
    ///     The configuration used to instantiate a <see cref="BerCodec" /> object
    /// </summary>
    /// <param name="assembliesToMap">
    ///     The assemblies that contain classes that derive from <see cref="PrimitiveValue" />,
    ///     <see cref="ConstructedValue" />, or
    ///     <see cref="PlayCodec" />. The assemblies will be scanned for the derived classes and an internal encoding
    ///     map for those types will be created.
    /// </param>
    /// <exception cref="BerParsingException"></exception>
    public BerConfiguration(IList<Assembly> assembliesToMap)
    {
        Assembly[] assemblies = assembliesToMap?.ToArray() ?? throw new BerParsingException(nameof(assembliesToMap));

        List<PlayCodec> playCodecs = GetPlayCodecs(assemblies);

        _PlayCodecMap = playCodecs.ToDictionary(a => a.GetEncodingId(), b => b);
    }

    public BerConfiguration(IDictionary<PlayEncodingId, PlayCodec> playCodecMap)
    {
        _PlayCodecMap = playCodecMap;
    }

    #endregion

    #region Instance Members

    private List<PlayCodec> GetPlayCodecs(Assembly[] assemblies)
    {
        HashSet<Assembly> uniqueAssemblies = new(assemblies.Select(a => a));

        List<PlayCodec> codecs = new();

        foreach (Type type in uniqueAssemblies.SelectMany(a => a.GetTypes())
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(PlayCodec))))
            codecs.Add((PlayCodec) FormatterServices.GetUninitializedObject(type));

        return codecs;
    }

    private static List<ConstructedValue> GetConstructedValues(params Assembly[] constructorArgs)
    {
        HashSet<Assembly> uniqueAssemblies = new(constructorArgs.Select(a => a));

        List<ConstructedValue> codecs = new();

        foreach (Type type in uniqueAssemblies.SelectMany(a => a.GetTypes()).Where(myType =>
            myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ConstructedValue))))
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
    /// <exception cref="BerParsingException"></exception>
    private void Validate(IList<PlayCodec> values)
    {
        ValidatePlayCodecIdentifiersAreUnique(values);
        ValidateBerEncodingIdsAreUnique(values.Select(a => a.GetEncodingId()).ToList());
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void Validate(IList<PlayCodec> playCodecs, IList<PrimitiveValue> primitiveValues)
    {
        ValidatePrimitiveValuesAreAllMappedToAPlayCodec(new HashSet<PlayCodec>(playCodecs), new HashSet<PrimitiveValue>(primitiveValues));
    }

    /// <exception cref="BerParsingException"></exception>
    private static void ValidateBerEncodingIdsAreUnique(IList<PlayEncodingId> mappingValue)
    {
        List<PlayEncodingId> distinct = mappingValue.Select(a => a).Distinct().ToList();

        if (distinct.Count == mappingValue.Count)
            return;

        List<string> duplicateValues = mappingValue.Except(distinct).Select(a => a.GetType().Name).ToList();

        const string stringSeparator = ", ";

        throw new BerParsingException(
            $"Duplicate {nameof(PlayEncodingId)} - There was a problem found while building the encoding mapping for {nameof(PlayCodec)} because there were duplicate values. "
            + $"Here is a list of the {nameof(PlayCodec)} that have a {nameof(PlayEncodingId)} that is not unique: [{string.Join(stringSeparator, duplicateValues)}");
    }

    /// <summary>
    ///     ValidatePlayCodecIdentifiersAreUnique
    /// </summary>
    /// <param name="playCodecs"></param>
    /// <exception cref="BerParsingException"></exception>
    private static void ValidatePlayCodecIdentifiersAreUnique(IList<PlayCodec> playCodecs)
    {
        List<PlayEncodingId> allTags = playCodecs.Select(a => a.GetEncodingId()).ToList();
        HashSet<PlayEncodingId> uniqueIdentifiers = new();
        List<PlayEncodingId> duplicateIdentifiers = new();

        foreach (PlayEncodingId tag in allTags)
        {
            if (!uniqueIdentifiers.Add(tag))
                duplicateIdentifiers.Add(tag);
        }

        if (uniqueIdentifiers.Count == playCodecs.Count)
            return;

        Dictionary<PlayEncodingId, string[]> codecsWithTheSameIdentifiers = new();

        foreach (PlayEncodingId identifier in duplicateIdentifiers)
        {
            codecsWithTheSameIdentifiers.Add(identifier,
                playCodecs.Where(a => a.GetEncodingId() == identifier).Select(a => a.GetType().Name).ToArray());
        }

        StringBuilder builder = new();

        foreach (KeyValuePair<PlayEncodingId, string[]> keyValue in codecsWithTheSameIdentifiers)
        {
            const string stringSeparator = ", ";
            builder.Append($"[0x{keyValue.Key}: ");
            builder.Append($"{string.Join(stringSeparator, keyValue.Value)}] \n");
        }

        throw new BerParsingException(
            $"Duplicate Identifiers - There was a problem found while building the encoding mapping for {nameof(PlayCodec)} because some of the types use the same identifier. The Fully Qualified Name of each {nameof(PlayCodec)} implementation must be unique. Here is a list of the types that implement the same Identifier: {builder}");
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private static void ValidatePrimitiveValuesAreAllMappedToAPlayCodec(HashSet<PlayCodec> codecs, HashSet<PrimitiveValue> primitiveValues)
    {
        HashSet<PlayEncodingId> encodingIds = new(codecs.Select(a => a.GetEncodingId()));
        Dictionary<PlayEncodingId, List<string>> unmappedPrimitiveValues = new();

        foreach (PrimitiveValue value in primitiveValues)
        {
            if (encodingIds.Contains(value.GetEncodingId()))
                continue;

            if (!unmappedPrimitiveValues.ContainsKey(value.GetEncodingId()))
                unmappedPrimitiveValues.Add(value.GetEncodingId(), new List<string> {value.GetType().Name});
            else
                unmappedPrimitiveValues[value.GetEncodingId()].Add(value.GetType().Name);
        }

        StringBuilder builder = new();

        foreach (KeyValuePair<PlayEncodingId, List<string>> keyValue in unmappedPrimitiveValues)
        {
            const string stringSeparator = ", ";
            builder.Append($"[0x{keyValue.Key}: ");
            builder.Append($"{string.Join(stringSeparator, keyValue.Value)}] \n");
        }

        throw new BerParsingException(
            $"There was a runtime error when validating that all {nameof(PrimitiveValue)} subclasses are correctly mapped to a {nameof(PlayCodec)}. Please check the {nameof(BerConfiguration)} for any errors. Here's a list of unmapped {nameof(PrimitiveValue)} subclasses: {builder}");
    }

    /// <summary>
    ///     This method is to be used to test that the compiled ASN.1 objects will all map correctly during serialization. The
    ///     ASN.1 objects (i.e. subclasses of <see cref="PrimitiveValue" /> and <see cref="ConstructedValue" />) will be
    ///     mapped
    ///     to specific formatting codecs, <see cref="PlayCodec" />, based on the values supplied to the constructor in
    ///     this class
    /// </summary>
    /// <warning>
    ///     NOTE: This method is NOT intended to be run in a production environment
    /// </warning>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void ValidateRuntimeConfiguration(IList<Assembly> asn1CompiledAssemblies)
    {
        List<PrimitiveValue>? primitiveValues = GetPrimitiveValues(asn1CompiledAssemblies.ToArray());
        List<ConstructedValue>? constructedValues = GetConstructedValues(asn1CompiledAssemblies.ToArray());
        List<PlayCodec> playCodecs = _PlayCodecMap.Select(a => a.Value).ToList();
        Validate(playCodecs);
        Validate(playCodecs, primitiveValues);
    }

    /// <summary>
    ///     This method is to be used to test that the compiled ASN.1 objects will all map correctly during serialization. The
    ///     ASN.1 objects (i.e. subclasses of <see cref="PrimitiveValue" /> and <see cref="IConstructedValue" />) will be
    ///     mapped
    ///     to specific formatting codecs, <see cref="PlayCodec" />, based on the values supplied to the constructor in
    ///     this class
    /// </summary>
    /// <warning>
    ///     NOTE: This method is NOT intended to be run in a production environment
    /// </warning>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void ValidateRuntimeConfiguration(IList<Assembly> primitiveValueAssemblies, IList<Assembly> constructedValueAssemblies)
    {
        List<PrimitiveValue>? primitiveValues = GetPrimitiveValues(primitiveValueAssemblies.ToArray());
        List<ConstructedValue>? constructedValues = GetConstructedValues(constructedValueAssemblies.ToArray());
        List<PlayCodec> playCodecs = _PlayCodecMap.Select(a => a.Value).ToList();
        Validate(playCodecs);
        Validate(playCodecs, primitiveValues);
    }

    #endregion
}