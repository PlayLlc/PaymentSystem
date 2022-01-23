namespace Play.Unmanaged;

public static class UnmanagedConverter
{
    #region Instance Members

    /// <summary>
    ///     Compares two unmanaged types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>
    ///     A signed integer that indicates the relative order of the unmanaged type x compared to y
    ///     Less than zero - y is less than x
    ///     Zero - x is equal to y
    ///     Greater than zero - x is greater than y
    /// </returns>
    public static unsafe int CompareTo<T>(T x, T y) where T : unmanaged
    {
        int byteSize = sizeof(T);
        byte* xPointer = (byte*) &x;
        byte* yPointer = (byte*) &y;

        for (nint i = 0; i < byteSize; i++, xPointer++, yPointer++)
        {
            if (*xPointer > *yPointer)
                return 1;
            if (*xPointer < *yPointer)
                return -1;
        }

        return 0;
    }

    #endregion

    #region Equality

    public static unsafe bool Equals<T>(T x, T y) where T : unmanaged
    {
        int byteSize = sizeof(T);
        byte* xPointer = (byte*) &x;
        byte* yPointer = (byte*) &y;

        for (nint i = 0; i < byteSize; i++, xPointer++, yPointer++)
        {
            if (*xPointer != *yPointer)
                return false;
        }

        return true;
    }

    public static unsafe int GetHashCode<T>(T value) where T : unmanaged
    {
        int byteSize = sizeof(T);
        byte* pointer = (byte*) &value;

        unchecked
        {
            int result = 0;

            for (nint i = 0; i < byteSize; i++, pointer++)
                result += (*pointer).GetHashCode();

            return result;
        }
    }

    public static unsafe int GetHashCode<T>(T value, int hasher) where T : unmanaged
    {
        int byteSize = sizeof(T);
        byte* pointer = (byte*) &value;

        unchecked
        {
            int result = 0;

            for (nint i = 0; i < byteSize; i++, pointer++)
                result += hasher * (*pointer).GetHashCode();

            return result;
        }
    }

    #endregion
}