﻿using Play.Ber.Identifiers;
using Play.Core.Exceptions;

namespace Play.Emv.DataElements.Exceptions;

internal class Check
{
    #region Static Metadata

    public static readonly CheckCore Core = new();

    #endregion

    public class Primitive
    {
        #region Instance Members

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ForExactLength<T>(T[] value, int length, Tag tag) where T : struct
        {
            if (value.Length != length)
            {
                throw new ArgumentOutOfRangeException(
                    $"The Primitive Value with the Tag {tag.ToString()} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {length} bytes in length");
            }
        }

        public static void ForCharLength(nint value, int length, Tag tag)
        {
            if (value != length)
            {
                throw new ArgumentOutOfRangeException(
                    $"The Primitive Value with the Tag {tag.ToString()} could not be initialized because the char length provided was out of range. The char length was {value} but must be {length} bytes in length");
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ForExactLength<T>(ReadOnlySpan<T> value, int length, Tag tag) where T : struct
        {
            if (value.Length != length)
            {
                throw new ArgumentOutOfRangeException(
                    $"The Primitive Value with the Tag {tag.ToString()} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {length} bytes in length");
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ForExactLength<T>(ReadOnlyMemory<T> value, int length, Tag tag) where T : struct
        {
            if (value.Length != length)
            {
                throw new ArgumentOutOfRangeException(
                    $"The Primitive Value with the Tag {tag.ToString()} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {length} bytes in length");
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ForExactLength<T>(IEnumerable<T> value, int length, Tag tag) where T : struct
        {
            if (value.Count() != length)
            {
                throw new ArgumentOutOfRangeException(
                    $"The Primitive Value with the Tag {tag.ToString()} could not be initialized because the byte length provided was out of range. The byte length was {value.Count()} but must be {length} bytes in length");
            }
        }

        /// <summary>
        ///     Throws an exception if the sequence's length is greater than the maximum length allowed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="name"></param>
        public static void ForMaximumLength<T>(IEnumerable<T> value, int maxLength, string name) where T : struct
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
        public static void ForMaximumLength<T>(ReadOnlySpan<T> value, int maxLength, string name) where T : struct
        {
            if (value.Length > maxLength)
            {
                throw new ArgumentOutOfRangeException(name,
                    $"The argument {name} was expected to have a maximum length of {maxLength} but did not");
            }
        }

        #endregion
    }
}