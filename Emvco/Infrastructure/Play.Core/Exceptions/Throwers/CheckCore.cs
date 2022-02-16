using System;
using System.Collections.Generic;
using System.Linq;

namespace Play.Core.Exceptions;

/// <summary>
///     Helpers for enforcing conditions and throwing when those conditions are not met
/// </summary>
public class CheckCore
{
    #region Instance Members

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<T>(T[] value, string name) where T : struct
    {
        if (value.Length == 0)
            throw new ArgumentOutOfRangeException(name);
    }

    public static void ForEmptySequence<T, TK>(IDictionary<T, TK> value, string name)
    {
        if (value.Count == 0)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<T>(HashSet<T> value, string name)
    {
        if (value.Count == 0)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<T>(ReadOnlySpan<T> value, string name) where T : struct
    {
        if (value == null)
            throw new ArgumentNullException();

        if (value.Length == 0)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<T>(Span<T> value, string name) where T : struct
    {
        if (value.Length == 0)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<T>(ReadOnlyMemory<T> value, string name) where T : struct
    {
        if (value.Length == 0)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<T>(Memory<T> value, string name) where T : struct
    {
        if (value.Length == 0)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForExactLength<T>(T[] value, int length, string name) where T : struct
    {
        if (value.Length != length)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForExactLength<T>(ReadOnlySpan<T> value, int length, string name) where T : struct
    {
        if (value.Length != length)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForExactLength<T>(Span<T> value, int length, string name) where T : struct
    {
        if (value.Length != length)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is greater than the maximum length allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <param name="name"></param>
    public static void ForMaximumLength<T>(IEnumerable<T> value, int maxLength, string name)
    {
        if (value.Count() > maxLength)
        {
            throw new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a maximum length of {maxLength} but did not");
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is greater than the maximum length allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <param name="name"></param>
    public static void ForMaximumLength<T>(T[] value, int maxLength, string name) where T : struct
    {
        if (value.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a maximum length of {maxLength} but did not");
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is greater than the maximum length allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <param name="name"></param>
    public static void ForMaximumLength<T>(Span<T> value, int maxLength, string name) where T : struct
    {
        if (value.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a maximum length of {maxLength} but did not");
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is greater than the maximum length allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <param name="name"></param>
    public static void ForMaximumLength<T>(ReadOnlySpan<T> value, int maxLength, string name) where T : struct
    {
        if (value.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a maximum length of {maxLength} but did not");
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is less than the minimum length allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="minLength"></param>
    /// <param name="name"></param>
    public static void ForMinimumLength<T>(T[] value, int minLength, string name) where T : struct
    {
        if (value.Length < minLength)
        {
            throw new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a minimum length of {minLength} but did not");
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is less than the minimum length allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="minLength"></param>
    /// <param name="name"></param>
    public static void ForMinimumLength<T>(IEnumerable<T> value, int minLength, string name) where T : struct
    {
        if (value.Count() < minLength)
        {
            throw new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a minimum length of {minLength} but did not");
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is less than the minimum length allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="minLength"></param>
    /// <param name="name"></param>
    public static void ForMinimumLength<T>(Span<T> value, int minLength, string name) where T : struct
    {
        if (value.Length < minLength)
        {
            throw new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a minimum length of {minLength} but did not");
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is less than the minimum length allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="minLength"></param>
    /// <param name="name"></param>
    public static void ForMinimumLength<T>(ReadOnlySpan<T> value, int minLength, string name) where T : struct
    {
        if (value.Length < minLength)
        {
            throw new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a minimum length of {minLength} but did not");
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForNullOrEmptySequence<T>(T[] value, string name)
    {
        if (value == null)
            throw new ArgumentNullException(name);
        if (value.Length == 0)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForNullOrEmptySequence<T>(IEnumerable<T> value, string name)
    {
        if (value == null)
            throw new ArgumentNullException(name);
        if (!value.Any())
            throw new ArgumentOutOfRangeException(name);
    }

    /// <summary>
    ///     Throws an ArgumentNullException if the given value is null, otherwise
    ///     return the value to the caller.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void ForNullOrEmptyString(string value, string name)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(name);
    }

    /// <summary>
    ///     Throws an ArgumentNullException if the given value is null, otherwise
    ///     return the value to the caller.
    /// </summary>
    /// <remarks>
    ///     This is equivalent to <see cref="ForNull{T}(T, string)" /> but without the type parameter
    ///     constraint. In most cases, the constraint is useful to prevent you from calling CheckNotNull
    ///     with a value type - but it gets in the way if either you want to use it with a nullable
    ///     value type, or you want to use it with an unconstrained type parameter.
    /// </remarks>
    /// <exception cref="ArgumentNullException"></exception>
    internal static void ForNullValueType<T>(T value, string name)
    {
        if (value == null)
            throw new ArgumentNullException(name);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void ForRange(int value, int maxValue, int minValue, string name)
    {
        if (value > maxValue)
            throw new ArgumentOutOfRangeException(name);

        if (value < minValue)
            throw new ArgumentOutOfRangeException(name);
    }

    /// <exception cref="ArgumentNullException"></exception>
    public static void ForRange(int value, int maxValue, int minValue, string name, string message)
    {
        if (value > maxValue)
            throw new ArgumentOutOfRangeException(name, message);

        if (value < minValue)
            throw new ArgumentOutOfRangeException(name, message);
    }

    #endregion

    #region Equality

    public static void Equals(int expectedValue, int value, string name)
    {
        if (value != expectedValue)
            throw new ArgumentOutOfRangeException(name);

        if (value != expectedValue)
            throw new ArgumentOutOfRangeException(name);
    }

    #endregion
}