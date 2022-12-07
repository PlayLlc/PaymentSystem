using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Play.Core.Tests.Data.TestDoubles;

public sealed record TestEnumObject : EnumObject<byte>
{
    #region Static Metadata

    public static readonly TestEnumObject Empty = new();
    private static readonly ImmutableSortedDictionary<byte, TestEnumObject> _ValueObjectMap;
    public static readonly TestEnumObject First;
    public static readonly TestEnumObject Second;

    #endregion

    #region Constructor

    public TestEnumObject()
    { }

    /// <exception cref="TypeInitializationException"></exception>
    static TestEnumObject()
    {
        const byte first = 1;
        const byte second = 2;
        First = new TestEnumObject(first);
        Second = new TestEnumObject(second);

        _ValueObjectMap = new Dictionary<byte, TestEnumObject> {{second, Second}, {first, First}}.ToImmutableSortedDictionary(a => a.Key, b => b.Value);
    }

    private TestEnumObject(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override TestEnumObject[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out TestEnumObject? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool TryGet(byte value, out TestEnumObject? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(TestEnumObject? other) => other is not null && (_Value == other._Value);

    public bool Equals(TestEnumObject? x, TestEnumObject? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TestEnumObject other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(_Value.GetHashCode() * 31771);

    #endregion
}