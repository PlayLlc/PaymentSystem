using Play.Core.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.Exceptions;

internal class Check
{
    #region Static Metadata

    public static readonly CheckCore Core = new();

    #endregion

    public class DataField
    {
        #region Instance Members

        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForExactLength<T>(T[] value, int length, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length != length)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(DataFieldId)} {dataFieldId} could not be initialized because the byte length provided was out of range. The byte length was: [{value.Length}] but must be {length} bytes in length");
            }
        }

        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForExactLength<T>(ReadOnlySpan<T> value, int length, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length != length)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(DataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was: [{value.Length}] but must be {length} bytes in length");
            }
        }

        /// <summary>
        ///     ForExactLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForExactLength<T>(ReadOnlyMemory<T> value, int length, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length != length)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(DataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was: [{value.Length}] but must be {length} bytes in length");
            }
        }

        /// <summary>
        ///     ForMinimumLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMinimumLength(byte value, int minLength, DataFieldId dataFieldId)
        {
            if (value < minLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be greater than {minLength}");
            }
        }

        /// <summary>
        ///     ForMinimumLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMinimumLength(ushort value, int minLength, DataFieldId dataFieldId)
        {
            if (value < minLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be greater than {minLength}");
            }
        }

        /// <summary>
        ///     ForMinimumLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMinimumLength<T>(ReadOnlyMemory<T> value, int minLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length < minLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {minLength} bytes in length");
            }
        }

        /// <summary>
        ///     ForMinimumLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMinimumLength<T>(ReadOnlySpan<T> value, int minLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length < minLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {minLength} bytes in length");
            }
        }

        /// <summary>
        ///     ForMinimumLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMinimumLength<T>(ICollection<T> value, int minLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Count < minLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was {value.Count} but must be less than {minLength} bytes in length");
            }
        }

        /// <summary>
        ///     Throws an exception if the sequence's length is greater than the maximum length allowed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMaximumLength<T>(ICollection<T> value, int maxLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Count > maxLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was {value.Count} but must not exceed {maxLength} bytes in length");
            }
        }

        /// <summary>
        ///     ForMaximumLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMaximumLength<T>(ReadOnlyMemory<T> value, int maxLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length > maxLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was: [{value.Length}] but must not exceed {maxLength} bytes in length");
            }
        }

        /// <summary>
        ///     Throws an exception if the sequence's length is greater than the maximum length allowed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMaximumLength<T>(ReadOnlySpan<T> value, int maxLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length > maxLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was: [{value.Length}] but must not exceed {maxLength} bytes in length");
            }
        }

        /// <summary>
        ///     ForMaximumLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMaximumLength(byte value, int maxLength, DataFieldId dataFieldId)
        {
            if (value > maxLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxLength}");
            }
        }

        /// <summary>
        ///     ForMaximumLength
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="dataFieldId"></param>
        /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
        public static void ForMaximumLength(ushort value, int maxLength, DataFieldId dataFieldId)
        {
            if (value > maxLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The {nameof(InterchangeDataField)} with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the value was out of range. The value provided was: [{value}] but must be no greater than {maxLength}");
            }
        }

        #endregion
    }
}