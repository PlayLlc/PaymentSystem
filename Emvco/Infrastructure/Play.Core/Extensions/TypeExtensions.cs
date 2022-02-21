using System;
using System.Collections;
using System.Linq;
using System.Numerics;

namespace Play.Core.Extensions;

public static class TypeExtensions
{
    #region Instance Members

    public static string GetLanguageTypeName(this Type type)
    {
        if (type == typeof(bool))
            return "bool";
        if (type == typeof(byte))
            return "byte";
        if (type == typeof(sbyte))
            return "sbyte";
        if (type == typeof(char))
            return "char";
        if (type == typeof(decimal))
            return "decimal";
        if (type == typeof(double))
            return "double";
        if (type == typeof(float))
            return "float";
        if (type == typeof(int))
            return "int";
        if (type == typeof(uint))
            return "uint";
        if (type == typeof(long))
            return "long";
        if (type == typeof(ulong))
            return "ulong";
        if (type == typeof(short))
            return "short";
        if (type == typeof(ushort))
            return "ushort";
        if (type == typeof(string))
            return "string";
        if (type == typeof(object))
            return "object";

        if (type == typeof(bool[]))
            return "bool[]";
        if (type == typeof(byte[]))
            return "byte[]";
        if (type == typeof(sbyte[]))
            return "sbyte[]";
        if (type == typeof(char[]))
            return "char[]";
        if (type == typeof(decimal[]))
            return "decimal[]";
        if (type == typeof(double[]))
            return "double[]";
        if (type == typeof(float[]))
            return "float[]";
        if (type == typeof(int[]))
            return "int[]";
        if (type == typeof(uint[]))
            return "uint[]";
        if (type == typeof(long[]))
            return "long[]";
        if (type == typeof(ulong[]))
            return "ulong[]";
        if (type == typeof(short[]))
            return "short[]";
        if (type == typeof(ushort[]))
            return "ushort[]";
        if (type == typeof(string[]))
            return "string[]";
        if (type == typeof(object[]))
            return "object[]";

        return type.Name;
    }

    public static bool HasDefaultConstructor(this Type t) => t.IsValueType || (t.GetConstructor(Type.EmptyTypes) != null);
    public static bool IsEnumerable(this Type value) => typeof(IEnumerable).IsAssignableFrom(value);

    public static bool IsSignedInteger(this Type value)
    {
        return Type.GetTypeCode(value) switch
        {
            TypeCode.SByte => true,
            TypeCode.Int16 => true,
            TypeCode.Int32 => true,
            TypeCode.Int64 => true,
            _ => false
        };
    }

    public static bool IsNumericType(this Type value)
    {
        const string bigInteger = nameof(BigInteger);

        if (value.Name == bigInteger)
            return true;

        return Type.GetTypeCode(value) switch
        {
            TypeCode.Byte => true,
            TypeCode.SByte => true,
            TypeCode.UInt16 => true,
            TypeCode.UInt32 => true,
            TypeCode.UInt64 => true,
            TypeCode.Int16 => true,
            TypeCode.Int32 => true,
            TypeCode.Int64 => true,
            TypeCode.Decimal => true,
            TypeCode.Double => true,
            TypeCode.Single => true,
            _ => false
        };
    }

    public static bool IsByte(this Type value)
    {
        return Type.GetTypeCode(value) switch
        {
            TypeCode.Byte => true,
            _ => false
        };
    }

    public static bool IsChar(this Type value)
    {
        if (value == typeof(BigInteger))
            return true;

        return Type.GetTypeCode(value) switch
        {
            TypeCode.Char => true,
            _ => false
        };
    }

    public static bool IsString(this Type value) => value.GetProperties().Any(a => a == typeof(string));

    #endregion
}