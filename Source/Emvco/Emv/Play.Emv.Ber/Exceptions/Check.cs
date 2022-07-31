using System.Numerics;

using Play.Ber.Identifiers;
using Play.Core.Exceptions;

namespace Play.Emv.Ber.Exceptions;

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
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForExactLength<T>(T[] value, int length, Tag tag) where T : struct
        {
            if (value.Length != length)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {length} bytes in length");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinCharLength(nint value, int minLength, Tag tag)
        {
            if (value < minLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the char length provided was out of range. The char length was {value} but must be at least {minLength} bytes in length");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaxCharLength(nint value, int maxLength, Tag tag)
        {
            if (value > maxLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the char length provided was out of range. The char length was {value} but must be less than {maxLength} bytes in length");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaxCharLength(ulong value, byte maxLength, Tag tag)
        {
            if (value > maxLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the char length provided was out of range. The char length was {value} but must be less than {maxLength} bytes in length");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaxCharLength(ushort value, byte maxLength, Tag tag)
        {
            if (value > maxLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the char length provided was out of range. The char length was {value} but must be less than {maxLength} bytes in length");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForCharLength(nint value, int length, Tag tag)
        {
            if (value != length)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the char length provided was out of range. The char length was {value} but must be {length} bytes in length");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinimumLength(int value, int minLength, Tag tag)
        {
            if (value < minLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be greater than {minLength}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinimumLength(ushort value, int minLength, Tag tag)
        {
            if (value < minLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be greater than {minLength}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinimumLength(uint value, int minLength, Tag tag)
        {
            if (value < minLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be greater than {minLength}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumLength(byte value, nint maxLength, Tag tag)
        {
            if (value > maxLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxLength}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumLength(ushort value, nint maxLength, Tag tag)
        {
            if (value > maxLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxLength}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumLength(BigInteger value, nint maxLength, Tag tag)
        {
            if (value > maxLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxLength}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumLength(uint value, nint maxLength, Tag tag)
        {
            if (value > maxLength)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxLength}");
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForExactLength<T>(ReadOnlySpan<T> value, int length, Tag tag) where T : struct
        {
            if (value.Length != length)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {length} bytes in length");
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForExactLength<T>(ReadOnlyMemory<T> value, int length, Tag tag) where T : struct
        {
            if (value.Length != length)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {length} bytes in length");
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="name"></param>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForExactLength<T>(ICollection<T> value, int length, Tag tag) where T : struct
        {
            if (value.Count != length)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the Tag {tag} could not be initialized because the byte length provided was out of range. The byte length was {value.Count} but must be {length} bytes in length");
            }
        }

        /// <summary>
        ///     Throws an exception if the sequence's length is greater than the maximum length allowed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="tag"></param>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumLength<T>(ICollection<T> value, int maxLength, Tag tag) where T : struct
        {
            if (value.Count > maxLength)
            {
                throw new DataElementParsingException(
                    $"The primitive value with the Tag {tag} was expected to have a maximum length of {maxLength} but did not");
            }
        }

        /// <summary>
        ///     Throws an exception if the sequence's length is greater than the maximum length allowed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="tag"></param>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumLength<T>(ReadOnlySpan<T> value, int maxLength, Tag tag) where T : struct
        {
            if (value.Length > maxLength)
            {
                throw new DataElementParsingException(
                    $"The primitive value with the Tag {tag} was expected to have a maximum length of {maxLength} but did not");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinimumLength<T>(ReadOnlySpan<T> value, int minLength, Tag tag) where T : struct
        {
            if (value.Length < minLength)
            {
                throw new DataElementParsingException(
                    $"The primitive value with the Tag {tag} was expected to have a minimum length of {minLength} but did not");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumValue(byte value, byte maxValue, Tag tag)
        {
            if (value > maxValue)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxValue}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumValue(ushort value, ushort maxValue, Tag tag)
        {
            if (value > maxValue)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxValue}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumValue(uint value, uint maxValue, Tag tag)
        {
            if (value > maxValue)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxValue}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMaximumValue(ulong value, ulong maxValue, Tag tag)
        {
            if (value > maxValue)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxValue}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinimumValue(byte value, byte minValue, Tag tag)
        {
            if (value < minValue)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {minValue}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinimumValue(ushort value, ushort minValue, Tag tag)
        {
            if (value < minValue)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no less than {minValue}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinimumValue(uint value, uint minValue, Tag tag)
        {
            if (value < minValue)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no less than {minValue}");
            }
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public static void ForMinimumValue(ulong value, ulong minValue, Tag tag)
        {
            if (value < minValue)
            {
                throw new DataElementParsingException(
                    $"The Primitive Value with the {nameof(tag)}: [{tag}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no less than {minValue}");
            }
        }

        #endregion
    }
}