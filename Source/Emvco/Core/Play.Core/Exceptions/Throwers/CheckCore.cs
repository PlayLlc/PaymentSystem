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

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<_>(_[] value, string name) where _ : struct
    {
        if (value.Length == 0)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    public static void ForEmptySequence<_, _Tk>(IDictionary<_, _Tk> value, string name)
    {
        if (value.Count == 0)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="_"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<_>(HashSet<_> value, string name)
    {
        if (value.Count == 0)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    public static void ForEmptySequence<_>(ReadOnlySpan<_> value, string name) where _ : struct
    {
        if (value == null)
            throw new PlayInternalException(new ArgumentNullException(name));

        if (value.Length == 0)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="_"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<_>(Span<_> value, string name) where _ : struct
    {
        if (value.Length == 0)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="_"></typeparam>
    /// <param name="value"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForEmptySequence<_>(ReadOnlyMemory<_> value, string name) where _ : struct
    {
        if (value.Length == 0)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void ForEmptySequence<_>(Memory<_> value, string name) where _ : struct
    {
        if (value.Length == 0)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void ForExactLength<_>(_[] value, int length, string name) where _ : struct
    {
        if (value.Length != length)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void ForExactLength<_>(ReadOnlySpan<_> value, int length, string name) where _ : struct
    {
        if (value.Length != length)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void ForExactLength<_>(Span<_> value, int length, string name) where _ : struct
    {
        if (value.Length != length)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is greater than the maximum length allowed
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForMaximumLength<_>(ICollection<_> value, int maxLength, string name)
    {
        if (value.Count > maxLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a maximum length of {maxLength} but did not"));
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is greater than the maximum length allowed
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForMaximumLength<_>(_[] value, int maxLength, string name) where _ : struct
    {
        if (value.Length > maxLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a maximum length of {maxLength} but did not"));
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is greater than the maximum length allowed
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForMaximumLength<_>(Span<_> value, int maxLength, string name) where _ : struct
    {
        if (value.Length > maxLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a maximum length of {maxLength} but did not"));
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is greater than the maximum length allowed
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForMaximumLength<_>(ReadOnlySpan<_> value, int maxLength, string name) where _ : struct
    {
        if (value.Length > maxLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a maximum length of {maxLength} but did not"));
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is less than the minimum length allowed
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForMinimumLength<_>(_[] value, int minLength, string name) where _ : struct
    {
        if (value.Length < minLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a minimum length of {minLength} but did not"));
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is less than the minimum length allowed
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForMinimumLength<_>(ICollection<_> value, int minLength, string name) where _ : struct
    {
        if (value.Count < minLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a minimum length of {minLength} but did not"));
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is less than the minimum length allowed
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForMinimumLength<_>(Span<_> value, int minLength, string name) where _ : struct
    {
        if (value.Length < minLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a minimum length of {minLength} but did not"));
        }
    }

    /// <summary>
    ///     Throws an exception if the sequence's length is less than the minimum length allowed
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForMinimumLength<_>(ReadOnlySpan<_> value, int minLength, string name) where _ : struct
    {
        if (value.Length < minLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(name,
                $"The argument {name} was expected to have a minimum length of {minLength} but did not"));
        }
    }

    /// <exception cref="PlayInternalException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ForNullOrEmptySequence<_>(_[] value, string name)
    {
        if (value == null)
            throw new PlayInternalException(new ArgumentNullException(name));
        if (value.Length == 0)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void ForNullOrEmptySequence<_>(ICollection<_> value, string name)
    {
        if (value == null)
            throw new PlayInternalException(new ArgumentNullException(name));
        if (!value.Any())
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <summary>
    ///     Throws an ArgumentNullException if the given value is null, otherwise
    ///     return the value to the caller.
    /// </summary>
    /// <exception cref="PlayInternalException"></exception>
    public static void ForNullOrEmptyString(string value, string name)
    {
        if (string.IsNullOrEmpty(value))
            throw new PlayInternalException(new ArgumentNullException(name));
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
    /// <exception cref="PlayInternalException"></exception>
    internal static void ForNullValueType<_>(_ value, string name)
    {
        if (value == null)
            throw new PlayInternalException(new ArgumentNullException(name));
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void ForRange(int value, int maxValue, int minValue, string name)
    {
        if (value > maxValue)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));

        if (value < minValue)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    /// <exception cref="PlayInternalException"></exception>
    public static void ForRange(int value, int maxValue, int minValue, string name, string message)
    {
        if (value > maxValue)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));

        if (value < minValue)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    #endregion

    #region Equality

    public static void Equals(int expectedValue, int value, string name)
    {
        if (value != expectedValue)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));

        if (value != expectedValue)
            throw new PlayInternalException(new ArgumentOutOfRangeException(name));
    }

    #endregion
}