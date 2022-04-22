using System;
using System.Collections.Generic;

using Play.Unmanaged;

namespace Play.Core;

/// <remarks>
///     The concrete implementation of this base class must not expose a constructor. There should only be publicly static
///     instances. No instantiation from outside the derived class should be allowed
/// </remarks>
public abstract record EnumObject<T> : IEquatable<T>, IEqualityComparer<T>, IComparable<T>, IEqualityComparer<EnumObject<T>>, IComparable<EnumObject<T>>
    where T : unmanaged
{
    #region Instance Values

    protected readonly T _Value;

    #endregion

    #region Constructor

    protected EnumObject()
    {
        _Value = default;
    }

    protected EnumObject(T value) : base()
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public abstract EnumObject<T>[] GetAll();
    public abstract bool TryGet(T value, out EnumObject<T>? result);

    #endregion

    #region Equality

    public bool Equals(T x, T y) => UnmanagedConverter.Equals(x, y);

    public bool Equals(EnumObject<T>? x, EnumObject<T>? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return UnmanagedConverter.Equals(x._Value, y._Value);
    }

    public bool Equals(T other) => UnmanagedConverter.Equals(other, _Value);
    public int GetHashCode(T obj) => UnmanagedConverter.GetHashCode(obj);
    public int GetHashCode(EnumObject<T> obj) => UnmanagedConverter.GetHashCode(obj._Value);
    public int GetHashCode(int hash) => unchecked(hash * _Value.GetHashCode());
    public int CompareTo(T other) => UnmanagedConverter.CompareTo(_Value, other);

    public int CompareTo(EnumObject<T>? other)
    {
        if (other is null)
            return 1;

        return UnmanagedConverter.CompareTo(_Value, other);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(EnumObject<T> left, T right) => left!._Value.Equals(right);
    public static bool operator ==(T left, EnumObject<T> right) => right!._Value.Equals(left);
    public static implicit operator T(EnumObject<T> enumObject) => enumObject._Value;
    public static bool operator !=(EnumObject<T> left, T right) => !left!._Value.Equals(right);
    public static bool operator !=(T left, EnumObject<T> right) => !right!._Value.Equals(left);

    #endregion
}