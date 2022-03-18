using System;

namespace Play.Core.Extensions;

public static partial class ReadOnlySpanExtensions
{

    public static bool IsValueEqual(this ReadOnlySpan<char> value, ReadOnlySpan<char> other)
    {
        if (value.Length != other.Length)
            return false;

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != other[i])
                return false;
        }

        return true;
    }
}