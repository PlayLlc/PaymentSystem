using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.Ber.Tests.TestDoubles;

public class MockDataObjectListTestFactory
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x1B, 0x9F, 0x66, 0x04, 0x9F, 0x02, 0x06, 0x9F,
        0x03, 0x06, 0x9F, 0x1A, 0x02, 0x95, 0x05, 0x5F,
        0x2A, 0x02, 0x9A, 0x03, 0x9C, 0x01, 0x9F, 0x37,
        0x04, 0x9F, 0x4E, 0x14
    };

    #endregion

    #region Instance Members

    public static MockDataObjectList Create() => MockDataObjectList.Decode(_DefaultContentOctets.AsSpan());

    public static MockDataObjectList Create(params TagLength[] values)
    {
        byte[]? encoded = values[0].Encode();
        byte[]? buffer = values.SelectMany(a => a.Encode()).ToArray();
        MockDataObjectList? helloMoto = MockDataObjectList.Decode(buffer.AsSpan());

        return MockDataObjectList.Decode(buffer.AsSpan());
    }

    public static IEnumerable<TagLength> GetEmptyTagLengths(params Tag[] value)
    {
        foreach (Tag item in value)
            yield return new TagLength(item, Array.Empty<byte>());
    }

    #endregion
}