using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.Templates;

namespace Play.Emv.Templates.Tests;

internal class TestTemplateHelper
{
    #region Instance Members

    /// <summary>
    ///     GetEncodedTemplate
    /// </summary>
    /// <param name="constructedValue"></param>
    /// <param name="dataElements"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public static byte[] GetEncodedTemplate(Template constructedValue, Dictionary<Tag, byte[]> dataElements)
    {
        TagLengthValue tlv = new(constructedValue.GetTag(), GetContentOctets(constructedValue.GetChildTags(), dataElements));

        return tlv.EncodeTagLengthValue();
    }

    private static byte[] GetContentOctets(Tag[] index, Dictionary<Tag, byte[]> dataElements)
    {
        if (dataElements.Count > index.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index),
                                                  $"The argument {nameof(index)} has fewer items than argument {nameof(dataElements)}. Please ensure that all {nameof(dataElements)} children have been indexed");
        }

        List<byte> result = new();

        for (nint i = 0, j = 0; i < index.Length; i++)
        {
            if (dataElements.All(x => x.Key != index[i]))
                continue;

            result.AddRange(dataElements.First(x => x.Key == index[i]).Value);
        }

        if (result.Count < dataElements.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index),
                                                  $"The argument {nameof(index)} has is missing an item in the {nameof(dataElements)} argument. Please ensure that all {nameof(dataElements)} children have been indexed");
        }

        return result.ToArray();
    }

    #endregion
}