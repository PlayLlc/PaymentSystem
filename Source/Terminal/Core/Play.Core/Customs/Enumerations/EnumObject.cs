using System;
using System.Collections.Generic;

using Play.Unmanaged;

namespace Play.Core;

/// <remarks>
///     The concrete implementation of this base class must not expose a constructor. There should only be publicly static
///     instances. No instantiation from outside the derived class should be allowed
/// </remarks>
public abstract record EnumObject<_> : IEquatable<_>, IEqualityComparer<_>, IComparable<_>, IEqualityComparer<EnumObject<_>>, IComparable<EnumObject<_>>
    where _ : unmanaged
{
    #region Instance Values

    protected readonly _ _Value;

    #endregion

    #region Constructor

    protected EnumObject()
    {
        _Value = default;
    }

    protected EnumObject(_ value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public abstract EnumObject<_>[] GetAll();
    public abstract bool TryGet(_ value, out EnumObject<_>? result);

    #endregion

    #region Equality

    public bool Equals(_ x, _ y) => UnmanagedConverter.Equals(x, y);

    public bool Equals(EnumObject<_>? x, EnumObject<_>? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return UnmanagedConverter.Equals(x._Value, y._Value);
    }

    public bool Equals(_ other) => UnmanagedConverter.Equals(other, _Value);
    public int GetHashCode(_ obj) => UnmanagedConverter.GetHashCode(obj);
    public int GetHashCode(EnumObject<_> obj) => UnmanagedConverter.GetHashCode(obj._Value);
    public int GetHashCode(int hash) => unchecked(hash * _Value.GetHashCode());
    public int CompareTo(_ other) => UnmanagedConverter.CompareTo(_Value, other);

    public int CompareTo(EnumObject<_>? other)
    {
        if (other is null)
            return 1;

        return UnmanagedConverter.CompareTo(_Value, other);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(EnumObject<_> left, _ right) => left!._Value.Equals(right);
    public static bool operator ==(_ left, EnumObject<_> right) => right!._Value.Equals(left);
    public static implicit operator _(EnumObject<_> enumObject) => enumObject._Value;
    public static bool operator !=(EnumObject<_> left, _ right) => !left!._Value.Equals(right);
    public static bool operator !=(_ left, EnumObject<_> right) => !right!._Value.Equals(left);

    #endregion
}