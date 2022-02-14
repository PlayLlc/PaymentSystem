using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Exceptions;
using Play.Interchange.Messages.DataFields;
using Play.Interchange.Messages.DataFields.ValueObjects;

namespace Play.Interchange.Exceptions;

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
                    $"The Interchange {nameof(Messages.DataFields.DataField)} with the {nameof(DataFieldId)} {dataFieldId} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {length} bytes in length");
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
                    $"The Interchange {nameof(Messages.DataFields.DataField)} with the {nameof(DataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {length} bytes in length");
            }
        }

        /// <summary>
        ///     Throws an exception if the sequence's length is greater than the maximum length allowed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="dataFieldId"></param>
        public static void ForMaximumLength<T>(IEnumerable<T> value, int maxLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Count() > maxLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The Interchange {nameof(Messages.DataFields.DataField)}  with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was {value.Count()} but must not exceed {maxLength} bytes in length");
            }
        }

        public static void ForMaximumLength<T>(ReadOnlyMemory<T> value, int maxLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length > maxLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The Interchange {nameof(Messages.DataFields.DataField)}  with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must not exceed {maxLength} bytes in length");
            }
        }

        /// <summary>
        ///     Throws an exception if the sequence's length is greater than the maximum length allowed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="dataFieldId"></param>
        public static void ForMaximumLength<T>(ReadOnlySpan<T> value, int maxLength, DataFieldId dataFieldId) where T : struct
        {
            if (value.Length > maxLength)
            {
                throw new InterchangeDataFieldOutOfRangeException(
                    $"The Interchange {nameof(Messages.DataFields.DataField)}  with the {nameof(dataFieldId)}: [{dataFieldId}], could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must not exceed {maxLength} bytes in length");
            }
        }

        #endregion
    }
}