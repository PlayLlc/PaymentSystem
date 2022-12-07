using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;

namespace Play.Emv.Reader
{
    public partial class ReaderDatabase : ITlvReaderAndWriter
    {
        #region Instance Values

        private readonly Dictionary<Tag, PrimitiveValue?> _TransactionDatabase;
        private TransactionSessionId? _TransactionSessionId;

        #endregion

        #region Instance Members

        public bool TryGetTransactionSessionId(out TransactionSessionId? result)
        {
            result = _TransactionSessionId;

            return result is not null;
        }

        #endregion

        #region Write

        public void Initialize(Tag tag)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Initialize)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

            if (!IsKnown(tag))
                return;

            _TransactionDatabase.Add(tag, null);
        }

        public void Initialize(params Tag[] tag)
        {
            for (int i = 0; i < tag.Length; i++)
            {
                if (!IsKnown(tag[i]))
                    continue;

                Initialize(tag[i]);
            }
        }

        /// <summary>
        ///     Updates the database with the
        ///     <param name="value"></param>
        ///     if it is a recognized object and discards the value if it is not recognized
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Update(PrimitiveValue value)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Update)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

            if (_TransactionDatabase.ContainsKey(value.GetTag()))
                _TransactionDatabase[value.GetTag()] = value;
            else
                _TransactionDatabase.Add(value.GetTag(), value);
        }

        /// <summary>
        ///     Updates the the database with all recognized
        ///     <param name="values" />
        ///     provided it is not recognized
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Update(PrimitiveValue[] values)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Update)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

            for (int i = 0; i < values.Length; i++)
                Update(values[i]);
        }

        public void Clear()
        {
            foreach (KeyValuePair<Tag, PrimitiveValue?> a in _TransactionDatabase)
                _TransactionDatabase[a.Key] = null;

            Seed();
        }

        #endregion

        #region Read

        public TransactionSessionId? GetKernelSessionId() => _TransactionSessionId;
        public bool IsKnown(Tag tag) => true;

        /// <summary>
        ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
        ///     zero
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        public bool IsPresent(Tag tag)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(IsPresent)} cannot be accessed because {nameof(ReaderDatabase)} is not active");

            return _TransactionDatabase.ContainsKey(tag);
        }

        /// <summary>
        ///     Gets the <see cref="PrimitiveValue" /> associated with the <see cref="Tag" /> in the arg
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        public T Get<T>(Tag tag) where T : PrimitiveValue
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Get)} cannot be accessed because {nameof(ReaderDatabase)} is not active");

            if (!_TransactionDatabase.TryGetValue(tag, out PrimitiveValue? result))
                throw new TerminalDataException($"The argument {nameof(tag)} provided does not exist in {nameof(ReaderDatabase)}");

            return (T) result!;
        }

        /// <summary>
        ///     Gets the <see cref="PrimitiveValue" /> associated with the <see cref="Tag" /> in the arg
        /// </summary>
        /// <param name="tag"></param>
        /// <exception cref="TerminalDataException">
        ///     This exception gets thrown internally because something was coded or incorrectly configured in our code base. An
        ///     assumption was made that the database value was present when it was not.
        /// </exception>
        /// <exception cref="TerminalDataException"></exception>
        public PrimitiveValue Get(Tag tag)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Get)} cannot be accessed because {nameof(ReaderDatabase)} is not active");

            if (!_TransactionDatabase.TryGetValue(tag, out PrimitiveValue? result))
                throw new TerminalDataException($"The argument {nameof(tag)} with the value: [{tag}] provided does not exist in {nameof(ReaderDatabase)}");

            return result!;
        }

        /// <summary>
        ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
        ///     <see cref="PrimitiveValue" /> in an out parameter
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <exception cref="TerminalDataException"></exception>
        public bool TryGet(Tag tag, out PrimitiveValue? result)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

            if (!_TransactionDatabase.TryGetValue(tag, out PrimitiveValue? databaseValue))
            {
                result = null;

                return false;
            }

            result = databaseValue;

            return true;
        }

        /// <summary>
        ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
        ///     <see cref="PrimitiveValue" /> in an out parameter
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <exception cref="TerminalDataException"></exception>
        public bool TryGet<T>(Tag tag, out T? result) where T : PrimitiveValue
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

            if (!_TransactionDatabase.TryGetValue(tag, out PrimitiveValue? databaseValue))
            {
                result = null;

                return false;
            }

            result = (T) databaseValue!;

            return true;
        }

        public bool IsPresentAndNotEmpty(Tag tag)
        {
            if (!IsActive())
            {
                throw new TerminalDataException($"The method {nameof(IsPresentAndNotEmpty)} cannot be accessed because {nameof(ReaderDatabase)} is not active");
            }

            return IsPresent(tag) && (_TransactionDatabase[tag] != null);
        }

        #endregion
    }
}