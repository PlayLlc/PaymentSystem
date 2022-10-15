using System;
using System.Collections.Generic;

namespace Play.Core;

public abstract record EnumObjectString : IEquatable<string>, IEqualityComparer<string>, IComparable<string>, IEqualityComparer<EnumObjectString>,
    IComparable<EnumObjectString>
{
    #region Instance Values

    protected readonly string _Value;

    #endregion

    #region Constructor

    protected EnumObjectString()
    {
        _Value = string.Empty;
    }

    protected EnumObjectString(string value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public abstract EnumObjectString[] GetAll();
    public abstract bool TryGet(string value, out EnumObjectString? result);

    #endregion

    #region Equality

    public bool Equals(string? x, string? y) => x.Equals(y);

    public bool Equals(EnumObjectString? x, EnumObjectString? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x._Value == y._Value;
    }

    public bool Equals(string? other)
    {
        if (other is null)
            return false;

        return other == _Value;
    }

    public int GetHashCode(string obj)
    {
        const int hash = 17;

        int result = hash;

        for (int i = 0; obj.Length > 0; i++)
        {
            result *= hash;
            result += obj[i];
        }

        return result;
    }

    public int GetHashCode(EnumObjectString obj) => obj.GetHashCode();
    public int GetHashCode(int hash) => unchecked(hash * _Value.GetHashCode());

    public int CompareTo(EnumObjectString? other)
    {
        if (other is null)
            return 1;

        return string.Compare(other._Value, _Value, StringComparison.InvariantCulture);
    }

    public int CompareTo(string? other)
    {
        if (other is null)
            return 1;

        return string.Compare(other, _Value, StringComparison.InvariantCulture);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(EnumObjectString left, string right) => left!._Value!.Equals(right);
    public static bool operator ==(string left, EnumObjectString right) => right!._Value!.Equals(left);
    public static implicit operator string(EnumObjectString enumObjectObject) => enumObjectObject._Value;
    public static bool operator !=(EnumObjectString left, string right) => !left!._Value!.Equals(right);
    public static bool operator !=(string left, EnumObjectString right) => !right!._Value!.Equals(left);

    #endregion
}