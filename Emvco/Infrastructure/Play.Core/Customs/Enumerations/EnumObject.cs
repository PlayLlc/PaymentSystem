using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Play.Unmanaged;

namespace Play.Core;

// HACK: This object and all the objects implementing this object should be a class not a record. The objects implementing this  class should also implement value equality for its type by implementing IEquatable, IEqualityComparer and overriding operators == and !=. Changing this to a class is a micro-optimization - it would mean that there are no value copies when a reference is passed to a method argument or referenced as a variable, instead every enum object would be a reference object. This is a low priority fix
/// <remarks>
///     The concrete implementation of this base class must not expose a constructor. There should only be publicly static
///     instances. No instantiation from outside the derived class should be allowed
/// </remarks>
public abstract record EnumObject<T> : IEquatable<T>, IEqualityComparer<T>, IComparable<T>, IEqualityComparer<EnumObject<T>>,
    IComparable<EnumObject<T>> where T : unmanaged
{
    #region Instance Values

    protected readonly T _Value;

    #endregion

    #region Constructor

    protected EnumObject()
    { }

    protected EnumObject(T value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public int CompareTo(T other) => UnmanagedConverter.CompareTo(_Value, other);

    public int CompareTo(EnumObject<T>? other)
    {
        if (other is null)
            return 1;

        return UnmanagedConverter.CompareTo(_Value, other);
    }

    /// <summary>
    ///     GetValues
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="TypeInitializationException"></exception>
    protected static Dictionary<T, EnumObject<T>> GetValues(Type type)
    {
        HashSet<EnumObject<T>> rawValues = new();

        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo fieldInfo in fields)
        {
            if (fieldInfo.IsPublic && fieldInfo.IsStatic && fieldInfo.IsInitOnly && (fieldInfo.FieldType.BaseType == typeof(EnumObject<T>)))
            {
                if (!rawValues.Add((EnumObject<T>) fieldInfo.GetRawConstantValue()))
                {
                    throw new TypeInitializationException(type.FullName,
                        new InvalidOperationException(
                            $"The {type.Name} declares two instances with the same underlying {typeof(T)} values. "
                            + "Please ensure unique values for the enum"));
                }
            }
        }

        return rawValues.ToDictionary(a => a._Value, b => b);
    }

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

    #endregion

    #region Operator Overrides

    public static bool operator ==(EnumObject<T> left, T right) => left!._Value.Equals(right);
    public static bool operator ==(T left, EnumObject<T> right) => right!._Value.Equals(left);
    public static implicit operator T(EnumObject<T> enumObject) => enumObject._Value;
    public static bool operator !=(EnumObject<T> left, T right) => !left!._Value.Equals(right);
    public static bool operator !=(T left, EnumObject<T> right) => !right!._Value.Equals(left);

    #endregion
}