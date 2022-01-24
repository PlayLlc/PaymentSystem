using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Long;
using Play.Ber.Identifiers.Short;
using Play.Core.Extensions;

namespace Play.Ber.Tests.TestData;

internal class ShortIdentifierTestValueFactory
{
    #region Static Metadata

    private static readonly List<ClassType> _ClassTypeValues = new()
    {
        ClassType.Application, ClassType.ContextSpecific, ClassType.Private, ClassType.Universal
    };

    private static readonly List<DataObjectType> _DataObjectTypeValues = new() {DataObjectType.Primitive, DataObjectType.Constructed};

    #endregion

    #region Instance Members

    public static byte CreateByte(Random random) =>
        ((byte) random.Next(0, byte.MaxValue)).GetMaskedValue(LongIdentifier.LongIdentifierFlag)
        .SetBits((byte) random.Next(0, ShortIdentifier.TagNumber.MaxValue));

    public static Tag Create(Random random) =>
        new(((byte) random.Next(0, byte.MaxValue)).GetMaskedValue(LongIdentifier.LongIdentifierFlag)
            .SetBits((byte) random.Next(0, ShortIdentifier.TagNumber.MaxValue)));

    public static ClassType GetClassType(Random random) => _ClassTypeValues.ElementAt(random.Next(0, _ClassTypeValues.Count - 1));

    public static DataObjectType GetDataObjectType(Random random) =>
        _DataObjectTypeValues.ElementAt(random.Next(0, _DataObjectTypeValues.Count - 1));

    public static byte GetTagNumber(Random random) =>
        ((byte) random.Next(0, ShortIdentifier.TagNumber.MaxValue)).GetMaskedValue(0b11100000);

    #endregion
}